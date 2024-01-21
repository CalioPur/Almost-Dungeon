using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
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
    public SaveSystem saveSystem;
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
    
    [SerializeField] private List<Sprite> customSprites;
    
    public List<TextAsset> dialogues;
    private Story story;
    [SerializeField] private DeckManager cardsManager;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject arrowKnight;
    [SerializeField] private GameObject arrowDragon;
    [SerializeField] private GameObject arrowMinion;
    [SerializeField] private GameObject choice1;
    [SerializeField] private GameObject choice2;
    [SerializeField] private MarkdownRenderer dialogueText;
    [SerializeField] private Button nextButton;
    [SerializeField] private Image otherImage;
    [SerializeField] private GameObject canvaDragon;
    [SerializeField] private GameObject canvaHero;
    [SerializeField] private GameObject timer;
    [SerializeField] private GameObject minionToken;
    [SerializeField] private GameObject lightDragon;
    [SerializeField] private GameObject lightHero;
    [SerializeField] private GameObject slots;
    [SerializeField] private TMP_Text heroName;
    
    DialogueVariable dialogueVariable;
    
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
        
        

        dialogueVariable = DungeonManager._instance.dialogueVariable;
        if (DungeonManager._instance.currentLevel < 7)
        {
            PlayAllThreeDialogues(DungeonManager._instance.terrainData.terrainDialogue, DungeonManager._instance.deckData.deckDialogue,
                DungeonManager._instance.heroData.heroDialogues, DungeonManager._instance.cardsManager);
        }
    }
    
    private void GetUiElements()
    {
        dialogueText = dialogueBox.GetComponentInChildren<MarkdownRenderer>();
        
        nextButton.onClick.AddListener(NextDialogue);
    }
    
    
    public void PlayAllThreeDialogues(TextAsset terrainDialogue, TextAsset deckDialogue, List<TextAsset> heroDialogue, DeckManager cardsManager)
    {
        GameManager._instance.isInDialogue = true;
        Debug.LogWarning("PlayAllThreeDialogues");
        print("HERO DIALOGUE COUNT : "+heroDialogue.Count);
        dialogueIndex = -1;
        GetUiElements();
        CheckHeroClass();
        slots.SetActive(false);
        timer.SetActive(false);
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
            timer.SetActive(true);
            Time.timeScale = 1;
            Transform camTransform = Camera.main.transform;
            camTransform.DOMove(new Vector3(0, 6.71f, -4f), 1f);
            camTransform.DORotate(new Vector3(65.5f, 0, 0), 1f);

            canvaDragon.transform.DOMove(new Vector3(-6.9f, -0.2f, 1f), 1.5f);
            canvaDragon.transform.DORotate(new Vector3(90, 0, 0), 1.5f);
            canvaDragon.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 1.5f);
            
            canvaHero.transform.DOMove(new Vector3(6.9f, -0.2f, 1.1f), 1.5f);
            canvaHero.transform.DORotate(new Vector3(90,0,0), 1.5f);
            canvaHero.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 1.5f);

            minionToken.SetActive(false);
            slots.SetActive(true);
            GameManager._instance.isInDialogue = false;
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
            dialogueVariable.StartListening(story);
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
            dialogueVariable.StopListening(story);
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
                            case "barbarian":
                                ShowArrowKnight();
                                otherImage.sprite = barbareSprite;
                                break;
                            case "minion":
                                ShowArrowMinion();
                                //otherImage.sprite = MinionSprite;
                                //action a faire pour voir un minion a l'ecran
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
                            
                            case("bigleux"):
                                GameManager._instance.currentHero.visionType = VisionType.BIGLEUX;
                                break;
                            case("visionBase"):
                                GameManager._instance.currentHero.visionType = VisionType.LIGNEDROITE;
                                break;
                            case("clairvoyant"):
                                GameManager._instance.currentHero.visionType = VisionType.CLAIRVOYANT;
                                break;
                            case ("peureux"):
                                GameManager._instance.currentHero.aggressivity = Aggressivity.PEUREUX;
                                break;
                            case ("agroBase"):
                                GameManager._instance.currentHero.aggressivity = Aggressivity.NONE;
                                break;
                            case ("courageux"):
                                GameManager._instance.currentHero.aggressivity = Aggressivity.COURAGEUX;
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
                                GameManager._instance.currentHero.classe = knightClass;
                                break;
                            case ("archer"):
                                GameManager._instance.currentHero.classe = archerClass;
                                break;
                            case("mage"):
                                GameManager._instance.currentHero.classe = mageClass;
                                break;
                            case("barbarian"):
                                GameManager._instance.currentHero.classe = barbareClass;
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
                    case "minion":
                        print("MINION");
                        switch (split[1])
                        {
                            case "in":
                                print("IN");
                                minionToken.transform.DOMove(new Vector3(-4.5f, 0.8f, 8), 0.5f).SetUpdate(true);
                                break;
                            case "out":
                                minionToken.transform.DOMove(new Vector3(-10f, 0.8f, 8), 0.5f).SetUpdate(true);
                                break;
                        }
                        break;
                    case "name":
                        heroName.text = split[1];
                        break;
                    case "time":
                        DungeonManager._instance.tickManager.BPM = int.Parse(split[1]);
                        break;
                    case "customSprite":
                        foreach (var sprite in customSprites)
                        {
                            if (sprite.name == split[1])
                            {
                                otherImage.sprite = sprite;
                                break;
                            }
                        }
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
        arrowMinion.SetActive(false);
        
        lightHero.SetActive(true);
        lightDragon.SetActive(false);
    }

    void ShowArrowDragon()
    {
        arrowDragon.SetActive(true);
        arrowKnight.SetActive(false);
        arrowMinion.SetActive(false);
        
        lightHero.SetActive(false);
        lightDragon.SetActive(true);
    }
    
    void ShowArrowMinion()
    {
        arrowDragon.SetActive(false);
        arrowKnight.SetActive(false);
        arrowMinion.SetActive(true);
        
        lightHero.SetActive(false);
        lightDragon.SetActive(false);
    }

    void CheckHeroClass()
    {
        print("CHECK HERO CLASS");
        otherImage.sprite = GameManager._instance.currentHero.classe.Img;
    }


    /*public void SetGlobalInkFile(string json)
    {
        globalsInkFile = new TextAsset(json);
    }*/
}
