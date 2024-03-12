using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Dialogue;
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
    [SerializeField] private DialogueChoice choice1;
    [SerializeField] private DialogueChoice choice2;
    [SerializeField] private MarkdownRenderer dialogueText;
    [SerializeField] private MarkdownRenderer markdownRenderer;
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
    
    public static event Action OnConversationEndedEvent;
    private static event Action OnEndDialogEvent;

    [Header("TextCoroutine")] [SerializeField]
    private float typingSpeed = 0.04f;

    private Coroutine displayLineCoroutine;
    [SerializeField] private bool canContinueToNextLine = false;

    private List<string> openTags = new();
    private List<string> closeTags = new();
    private string sourceText;
    private string completeText;

    private int dialogueIndex = -1;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    private void Start()
    {
        dialogueVariable = DungeonManager._instance.dialogueVariable;
        if (displayLineCoroutine != null)
            StopCoroutine(displayLineCoroutine);
        displayLineCoroutine = null;
        dialogueText.Source = completeText;
        if (DungeonManager._instance.currentLevel < 7 &&
            DungeonManager._instance.terrainData &&
            DungeonManager._instance.deckData &&
            DungeonManager._instance.heroData &&
            DungeonManager._instance.cardsManager)
        {
            PlayAllThreeDialogues(DungeonManager._instance.terrainData.terrainDialogue,
                DungeonManager._instance.deckData.deckDialogue,
                DungeonManager._instance.heroData.languages,
                DungeonManager._instance.cardsManager);
        }
    }

    private void GetUiElements()
    {
        dialogueText = markdownRenderer;

        nextButton.onClick.AddListener(NextDialogue);
    }


    public void PlayAllThreeDialogues(TextAsset[] terrainDialogues, TextAsset[] deckDialogues, Language[] languesHero,
        DeckManager cardsManager)
    {
        GameManager.Instance.isInDialogue = true;
        dialogueIndex = -1;
        GetUiElements();
        CheckHeroClass();
        slots.SetActive(false);
        timer.SetActive(false);
        dialogues = new List<TextAsset>();
        if (terrainDialogues.Length > PlayerPrefs.GetInt("langue", 0) &&
            terrainDialogues[PlayerPrefs.GetInt("langue", 0)] != null)
            dialogues.Add(terrainDialogues[PlayerPrefs.GetInt("langue", 0)]);
        if (languesHero.Length > PlayerPrefs.GetInt("langue", 0) &&
            languesHero[PlayerPrefs.GetInt("langue", 0)].heroDialogues != null)
        {
            var heroDialogue = languesHero[PlayerPrefs.GetInt("langue", 0)].heroDialogues;
            if (dialogues.Count > 0)
                dialogues.Add(interludeDialogues[Random.Range(0, interludeDialogues.Count)]);
            if (heroDialogue.Count > 0)
                dialogues.Add(heroDialogue[Random.Range(0, heroDialogue.Count)]);
        }

        if (deckDialogues.Length > PlayerPrefs.GetInt("langue", 0) &&
            deckDialogues[PlayerPrefs.GetInt("langue", 0)] != null)
        {
            if (dialogues.Count > 0)
                dialogues.Add(interludeDialogues[Random.Range(0, interludeDialogues.Count)]);
            dialogues.Add(deckDialogues[PlayerPrefs.GetInt("langue", 0)]);
        }


        OnEndDialogEvent += PlayNextDialogue;

        PlayNextDialogue();
        this.cardsManager = cardsManager;
    }

    private void PlayNextDialogue()
    {
        dialogueIndex++;
        if (dialogueIndex >= dialogues.Count)
        {
            OnEndDialogEvent = null;
            dialogueBox.SetActive(false);
            nextButton.onClick.RemoveAllListeners();
            timer.SetActive(true);
            OnConversationEndedEvent?.Invoke();
            Transform camTransform = Camera.main.transform;
            camTransform.DOMove(new Vector3(0, 6.71f, -4f), 1f);
            camTransform.DORotate(new Vector3(65.5f, 0, 0), 1f);

            canvaDragon.transform.DOMove(new Vector3(-6.9f, -0.2f, 1f), 1.5f);
            canvaDragon.transform.DORotate(new Vector3(90, 0, 0), 1.5f);
            canvaDragon.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 1.5f);

            canvaHero.transform.DOMove(new Vector3(6.9f, -0.2f, 1.1f), 1.5f);
            canvaHero.transform.DORotate(new Vector3(90, 0, 0), 1.5f);
            canvaHero.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 1.5f);

            minionToken.SetActive(false);
            slots.SetActive(true);
            GameManager.Instance.isInDialogue = false;
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
            OnConversationEndedEvent?.Invoke();
            return;
        }

        try
        {
            story = new Story(currentDialogue.text);
            dialogueVariable.StartListening(story);
        }
        catch
        {
            dialogueBox.SetActive(false);
            return; 
        }


        dialogueBox.SetActive(true);
        choice1.gameObject.SetActive(false);
        choice2.gameObject.SetActive(false);
        RefreshView();
        TickManager.Instance.PauseTick(true);
    }

    public void NextDialogue()
    {
        RefreshView();
    }

    private void RefreshView()
    {
        var choices = story.currentChoices.Count;

        if (displayLineCoroutine != null)
        {
            StopCoroutine(displayLineCoroutine);
            displayLineCoroutine = null;
            dialogueText.Source = completeText;
            return;
        }

        if (story.canContinue)
        {
            displayLineCoroutine = StartCoroutine(DisplayLineRoutine(story.Continue()));
            //dialogueText.Source = story.Continue();
            EvaluateTags();
        }

        else
        {
            if (choices > 0)
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

    private IEnumerator DisplayLineRoutine(string line)
    {
        yield return null;
        openTags.Clear();
        sourceText = string.Empty;
        closeTags.Clear();
        dialogueText.Source = string.Empty;
        var settings = dialogueText.RenderSettings;
        completeText = line;
        Time.timeScale = 1;
        for (var index = 0; index < line.Length; index++)
        {
            sourceText += CharacterToAppend();

            var suffixesText = closeTags.Aggregate(string.Empty, (current, s) => current + s);

            dialogueText.Source = $"{sourceText}{suffixesText}";
            
            yield return new WaitForSeconds(typingSpeed);
            
            continue;
            
            string CharacterToAppend()
            {
                var letter = line[index];
                var characterToAppend = letter.ToString();
                var tempTag = characterToAppend;

                if (letter == ' ')
                {
                    index++;
                    characterToAppend += CharacterToAppend();
                    return characterToAppend;
                }

                if (characterToAppend.IsInTag(settings))
                {
                    if (CheckForMultiCharTag(out characterToAppend))
                    {
                        index += characterToAppend.Length - 1;
                    }
                }

                if (characterToAppend.IsOpenTag(settings, out var closeTag) && !openTags.Contains(characterToAppend))
                {
                    index++;
                    openTags.Add(characterToAppend);
                    closeTags.Add(closeTag);
                    characterToAppend += CharacterToAppend();
                    return characterToAppend;
                }

                if (characterToAppend.IsCloseTag(settings, out var openTag))
                {
                    openTags.Remove(openTag);
                    closeTags.Remove(characterToAppend);
                    return characterToAppend;
                }

                return characterToAppend;

                bool CheckForMultiCharTag(out string markdownTag)
                {
                    markdownTag = string.Empty;
                    while (tempTag.IsInTag(settings))
                    {
                        if (index + tempTag.Length >= line.Length)
                        {
                            return false;
                        }

                        tempTag += line[index + tempTag.Length];
                    }

                    markdownTag = tempTag.Remove(tempTag.Length - 1, 1);
                    return markdownTag.IsInTag(settings);
                }
            }
        }

        displayLineCoroutine = null;
        dialogueText.Source = completeText;
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
                                WiggleCard(canvaDragon.transform, -1);
                                break;
                            case "knight":
                                ShowArrowKnight();
                                otherImage.sprite = HeroSprite;
                                WiggleCard(canvaHero.transform, 1);
                                break;
                            case "archer":
                                ShowArrowKnight();
                                otherImage.sprite = archerSprite;
                                WiggleCard(canvaHero.transform, 1);
                                break;
                            case "mage":
                                ShowArrowKnight();
                                otherImage.sprite = mageSprite;
                                WiggleCard(canvaHero.transform, 1);
                                break;
                            case "barbarian":
                                ShowArrowKnight();
                                otherImage.sprite = barbareSprite;
                                WiggleCard(canvaHero.transform, 1);
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
                        //GameManager.Instance.heroCurrentHealthPoint -= damage;
                        Hero.Instance.TakeDamage(damage, AttackType.Fire);
                        print("test Damage" + split[1]);
                        break;
                    case "hpplus":
                        var heal = int.Parse(split[1]);
                        GameManager.Instance.heroHealthPoint += heal;
                        GameManager.Instance.heroCurrentHealthPoint += heal;
                        break;
                    case "changepers":
                        switch (split[1])
                        {
                            case ("bigleux"):
                                GameManager.Instance.currentHero.visionType = VisionType.BIGLEUX;
                                break;
                            case ("visionBase"):
                                GameManager.Instance.currentHero.visionType = VisionType.LIGNEDROITE;
                                break;
                            case ("clairvoyant"):
                                GameManager.Instance.currentHero.visionType = VisionType.CLAIRVOYANT;
                                break;
                            case ("peureux"):
                                GameManager.Instance.currentHero.aggressivity = Aggressivity.PEUREUX;
                                break;
                            case ("agroBase"):
                                GameManager.Instance.currentHero.aggressivity = Aggressivity.NONE;
                                break;
                            case ("courageux"):
                                GameManager.Instance.currentHero.aggressivity = Aggressivity.COURAGEUX;
                                break;
                        }
                        DungeonManager._instance.RefreshCard();
                        break;
                    case "playSFX":
                        switch (split[1])
                        {
                            case (not null):
                                SoundManagerIngame.Instance.PlayDialogueSFX(split[1]);
                                break;
                            case (null):
                                break;
                        }

                        break;
                    case "changeclass":
                        switch (split[1])
                        {
                            case ("knight"):
                                GameManager.Instance.currentHero.classe = knightClass;
                                break;
                            case ("archer"):
                                GameManager.Instance.currentHero.classe = archerClass;
                                break;
                            case ("mage"):
                                GameManager.Instance.currentHero.classe = mageClass;
                                break;
                            case ("barbarian"):
                                GameManager.Instance.currentHero.classe = barbareClass;
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
                    case "healDragon":
                        var healDragon = int.Parse(split[1]);
                        FindObjectOfType<UI_Dragon>().Heal(healDragon);
                        break;
                    case "minion":
                        
                        switch (split[1])
                        {
                            case "in":
                                minionToken.transform
                                    .DOMove(new Vector3(-4.5f, 0.8f, 8), TickManager.Instance.calculateBPM())
                                    .SetUpdate(true);
                                break;
                            case "out":
                                minionToken.transform
                                    .DOMove(new Vector3(-10f, 0.8f, 8), TickManager.Instance.calculateBPM())
                                    .SetUpdate(true);
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
                    case "unlockAchievement":
                        AchievmentSteamChecker._instance.UnlockAchievementFromDialogue(split[1]);
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
        choice1.gameObject.SetActive(true);
        choice2.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(false);
        choice1.markdownManager.Source = storyCurrentChoices[0].text;
        choice2.markdownManager.Source = storyCurrentChoices[1].text;
        choice1.Btn.onClick.AddListener(delegate { OnClickChoiceButton(storyCurrentChoices[0]); });
        choice2.Btn.onClick.AddListener(delegate { OnClickChoiceButton(storyCurrentChoices[1]); });
    }

    void OnClickChoiceButton(Choice choice)
    {
        story.ChooseChoiceIndex(choice.index);
        choice1.Btn.onClick.RemoveAllListeners();
        choice2.Btn.onClick.RemoveAllListeners();
        choice1.gameObject.SetActive(false);
        choice2.gameObject.SetActive(false);
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
        //print("CHECK HERO CLASS");
        otherImage.sprite = GameManager.Instance.currentHero.classe.Img;
    }


    public void WiggleCard(Transform card, int mult = 1)
    {
        //print("test WIGGLE");
        card.transform.DOLocalRotate(card.transform.localRotation.eulerAngles + new Vector3(0, mult * 10, 0), 0.1f)
            .SetEase(Ease.Linear).SetLoops(4, LoopType.Yoyo).SetUpdate(true);
    }
}