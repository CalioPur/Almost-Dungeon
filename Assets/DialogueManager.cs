using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using LogicUI.FancyTextRendering;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class DialogueManager : MonoBehaviour
{
    
    public static DialogueManager _instance;
    public List<TextAsset> interludeDialogues;
    
    public Sprite HeroSprite;
    public Sprite archerSprite;
    public Sprite mageSprite;
    public Sprite barbareSprite;
    public Sprite MinionSprite;

    [SerializeField] private HeroesInfo knightClass;
    [SerializeField] private HeroesInfo archerClass;
    [SerializeField] private HeroesInfo mageClass;
    [SerializeField] private HeroesInfo barbareClass;
    
    [SerializeField] private List<DeckSO> decks;
    
    public List<TextAsset> dialogues;
    private Story story;
    private DeckManager cardsManager;
    private GameObject dialogueBox;
    private GameObject arrowKnight;
    private GameObject arrowDragon;
    private GameObject choice1;
    private GameObject choice2;
    private MarkdownRenderer dialogueText;
    private Button nextButton;
    private Image otherImage;
    private static event Action OnEndDialogEvent; 
    private int dialogueIndex = -1;
    
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        
        DontDestroyOnLoad(this);
    }
    
    private void GetUiElements()
    {
        dialogueBox = GameObject.Find("DialoguePanel");
        arrowKnight = GameObject.Find("ArrowKnight");
        arrowDragon = GameObject.Find("ArrowDragon");
        choice1 = GameObject.Find("Choice1");
        choice2 = GameObject.Find("Choice2");
        otherImage = GameObject.Find("InterlocutorImg").GetComponent<Image>();
        
        dialogueText = dialogueBox.GetComponentInChildren<MarkdownRenderer>();
        nextButton = dialogueBox.GetComponentInChildren<Button>();
        nextButton.onClick.AddListener(NextDialogue);
    }
    
    
    public void PlayAllThreeDialogues(TextAsset terrainDialogue, TextAsset deckDialogue, List<TextAsset> heroDialogue, DeckManager cardsManager)
    {
        Debug.LogWarning("PlayAllThreeDialogues");
        print("HERO DIALOGUE COUNT : "+heroDialogue.Count);
        dialogueIndex = -1;
        GetUiElements();
        dialogues = new List<TextAsset>();
        if(terrainDialogue != null)
            dialogues.Add(terrainDialogue);
        if (heroDialogue != null)
        {
            
            if (dialogues.Count > 0)
                dialogues.Add(interludeDialogues[Random.Range(0, interludeDialogues.Count)]);
            if(heroDialogue.Count > 0)
                dialogues.Add(heroDialogue[Random.Range(0, heroDialogue.Count)]);
        }
        if (deckDialogue != null)
        {
            if (dialogues.Count > 0)
                dialogues.Add(interludeDialogues[Random.Range(0, interludeDialogues.Count)]);
            dialogues.Add(deckDialogue);
        }
        
        
        OnEndDialogEvent+=PlayNextDialogue;
        PlayNextDialogue();
        this.cardsManager = cardsManager;
    }

    private void PlayNextDialogue()
    {
        dialogueIndex++;
        if(dialogueIndex >= dialogues.Count)
        {
            OnEndDialogEvent = null;
            dialogueBox.SetActive(false);
            nextButton.onClick.RemoveAllListeners();
            Time.timeScale = 1;
            return;
        }
        
        StartDialogue(dialogues[dialogueIndex]);
        
    }

    private void StartDialogue(TextAsset currentDialogue)
    {

        if (currentDialogue == null)
        {
            dialogueBox.SetActive(false);
            dialogueIndex++;
            OnEndDialogEvent?.Invoke();
            return;
        }

        try
        {
            story = new Story(currentDialogue.text);
        }
        catch{
            dialogueBox.SetActive(false);
            Time.timeScale = 1;
            return; 
        }
        
        
        dialogueBox.SetActive(true);
        choice1.SetActive(false);
        choice2.SetActive(false);
        RefreshView();
        Time.timeScale = 0;
    }

    public void NextDialogue()
    {
        RefreshView();
    }

    void RefreshView()
    {
        var choices = story.currentChoices.Count;
        if (story.canContinue)
        {
            dialogueText.Source = story.Continue();
            EvaluateTags();
        }
        else
        {
            if(choices > 0)
            {
                DisplayChoices();
                return;
            }
            dialogueBox.SetActive(false);
            story = null;
            OnEndDialogEvent?.Invoke();
        }
    }

    private void EvaluateTags()
    {
        foreach (var lineTag in story.currentTags)
        {
            if (lineTag.Contains(':'))
            {
                var split = lineTag.Split(':');

                switch (split[0])
                {
                    case "chara":
                        switch (split[1])
                        {
                            case "dragon":
                                ShowArrowDragon();
                                break;
                            case "knight":
                                ShowArrowKnight();
                                otherImage.sprite = HeroSprite;
                                break;
                            case "archer":
                                ShowArrowKnight();
                                otherImage.sprite = archerSprite;
                                break;
                            case "mage":
                                ShowArrowKnight();
                                otherImage.sprite = mageSprite;
                                break;
                            case "barbare":
                                ShowArrowKnight();
                                otherImage.sprite = barbareSprite;
                                break;
                            case "minion":
                                ShowArrowKnight();
                                otherImage.sprite = MinionSprite;
                                break;
                        }
                        break;
                    case "damages":
                        var damage = int.Parse(split[1]);
                        GameManager._instance.heroCurrentHealthPoint -= damage;
                        break;
                    case "hpplus":
                        var heal = int.Parse(split[1]);
                        GameManager._instance.heroHealthPoint += heal;
                        GameManager._instance.heroCurrentHealthPoint += heal;
                        break;
                    case "changepers":
                        switch (split[1])
                        {
                            case ("courageux"):
                                GameManager._instance.currentAggressivity = Aggressivity.COURAGEUX;
                                break;
                            case ("peureux"):
                                GameManager._instance.currentAggressivity = Aggressivity.PEUREUX;
                                break;
                            case("clairvoyant"):
                                GameManager._instance.currentVisionType = VisionType.CLAIRVOYANT;
                                break;
                            case("explorateur"):
                                //PTDR
                                break;
                        }
                        break;
                    
                    case "playSFX":
                        switch (split[1])
                        {
                            case (not null) :
                                SoundManagerIngame.Instance.PlayDialogueSFX(split[1]);
                                break;
                            case (null) :
                                Debug.Log("sound not assigned");
                                break;
                        }
                        break;
                    
                    case "changeclass":
                        switch (split[1])
                        {
                            case ("knight"):
                                GameManager._instance.currentHero = knightClass;
                                break;
                            case ("archer"):
                                GameManager._instance.currentHero = archerClass;
                                break;
                            case("mage"):
                                GameManager._instance.currentHero = mageClass;
                                break;
                            case("barbare"):
                                GameManager._instance.currentHero = barbareClass;
                                break;
                        }
                        break;
                    case "changedeck":
                        //recuperer les nom des decks et les comprarer avec le split[1]
                        foreach (var deck in decks)
                        {
                            if (deck.name == split[1])
                            {
                                print("DECK FOUND");
                                cardsManager.deckToBuild = deck.deck;
                                break;
                            }
                        }
                        break;
                    case "dragonHeal":
                        var healDragon = int.Parse(split[1]);
                        FindObjectOfType<UI_Dragon>().Heal(healDragon);
                        break;
                }
            }
        }
    }

    private void DisplayChoices()
    {
        DisplayChoices(story.currentChoices);
    }

    private void DisplayChoices(List<Choice> storyCurrentChoices)
    {
        choice1.SetActive(true);
        choice2.SetActive(true);
        nextButton.gameObject.SetActive(false);
        choice1.GetComponentInChildren<MarkdownRenderer>().Source = storyCurrentChoices[0].text;
        choice2.GetComponentInChildren<MarkdownRenderer>().Source = storyCurrentChoices[1].text;
        choice1.GetComponentInChildren<Button>().onClick.AddListener(delegate { OnClickChoiceButton(storyCurrentChoices[0]); });
        choice2.GetComponentInChildren<Button>().onClick.AddListener(delegate { OnClickChoiceButton(storyCurrentChoices[1]); });
    }
    
    void OnClickChoiceButton (Choice choice) {
        story.ChooseChoiceIndex (choice.index);
        choice1.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
        choice2.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
        choice1.SetActive(false);
        choice2.SetActive(false);
        nextButton.gameObject.SetActive(true);
        RefreshView();
    }
    
    void ShowArrowKnight()
    {
        arrowDragon.SetActive(false);
        arrowKnight.SetActive(true);
    }

    void ShowArrowDragon()
    {
        arrowDragon.SetActive(true);
        arrowKnight.SetActive(false);
    }
}
