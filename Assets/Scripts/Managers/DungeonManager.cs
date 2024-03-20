using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Ink.Parsed;
using JimmysUnityUtilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


[Serializable]
public struct Dungeon
{
    
    [SerializeField] public DungeonSO dungeonSO;
    [SerializeField] public string name;
    [SerializeField] public bool isLocked;
}

public class DungeonManager : MonoBehaviour
{
    public static event Action OnLevelLoaded;
    
    [SerializeField] public DungeonSO TutorialDungeon;
    [SerializeField] public List<Dungeon> dungeons;
    [Header("Endless LevelSo")]
    public LevelSO endlessLevel;
    public List<HeroesInfo> classes;
    private int leastPV = 10;
    private int mostPV = 50;
    private float slowest = 1.5f;
    private float fastest = 3f;
    private float expFactor = 0.1f;
    // genere une liste de nom de hero
    private List<string> names = new List<string>() { "Roger", 
        "Alice",
        "Bob",
        "Claire",
        "David",
        "Emma",
        "Fabrice",
        "Gabrielle",
        "Hugo",
        "Isabelle",
        "Julien",
        "Kelly",
        "Lucas",
        "Marie",
        "Nathan",
        "Olivia",
        "Pierre",
        "Quentin",
        "Rachel",
        "Simon",
        "Tiffany"};
    
    private float[] modePVRelatif = {1f, 1.1f, 1.2f};
    private float[] modeSpeedRelatif = {1f, 1.5f, 2f};
    
    private float[] ClassePVRelatif = {1f, 0.7f, 0.9f,0.6f};
    private float[] ClasseSpeedRelatif = {1f, 1f, 0.9f, 0.9f};
    
    private float[] VisionPVRelatif = {1f, 1.1f, 0.8f};
    private float[] VisionSpeedRelatif = {1f, 1.2f, 0.8f};
    
    private float[] AggressivityPVRelatif = {1f, 1.1f, 0.8f};
    private float[] AggressivitySpeedRelatif = {1f, 1.1f, 0.9f};
    
    private float[] PersonalityPVRelatif = {1f, 1.1f, 0.7f};
    private float[] PersonalitySpeedRelatif = {1f, 1.1f, 0.8f};
    
    
    [Header("Infos")]
    public int currentLevel = 0;
    public DeckManager cardsManager;
    public TickManager tickManager;
    private GameManager gameManager;
    private MapManager mapManager;
    public static int SelectedBiome;
    
    public TilePresetSO terrainData;
    public HeroSOInstance heroData;
    public DeckSO deckData;
    
    
    public DialogueVariable dialogueVariable;
    [Header("Le truc ink la")]
    [SerializeField] private TextAsset globalsInkFile;

    public static DungeonManager _instance;
    public bool TutorialDone = true;

    [HideInInspector] public int machValue;
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        
        DontDestroyOnLoad(this);
        gameManager = GameManager.Instance;

        dialogueVariable = new DialogueVariable(globalsInkFile);
    }

    private void Start()
    {
        TutorialDone = PlayerPrefs.HasKey("FinishedTutorial");
    }

    public void SetSelectedBiome(int index)
    {
        machValue = 0;
        SelectedBiome = index;
        GameManager.OnSceneLoadedEvent += LoadLevel;
        SceneManager.LoadScene(2);
    }
    
    public void SetSelectedMach(int index)
    {
        machValue = index;
        GameManager.OnSceneLoadedEvent += LoadEndlessLevel;
        SceneManager.LoadScene(2);
    }
    
    public void SetSelectedBiomeAndLevelFromSave(int indexBiome, int indexLevel)
    {
        SelectedBiome = indexBiome;
        currentLevel = indexLevel;
        GameManager.OnSceneLoadedEvent += LoadLevel;
        SceneManager.LoadScene(2);
    }
    
    public static string ToTitleCase(string title)
    {
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title.ToLower()); 
    }
    
    private void LoadLevel()
    {
        /*if(TutorialDone)
            PlayerPrefs.SetInt("LevelUnlock" + 1, 1); //on unlock le niveau 2 au lancement d'un donjon, qui sera forcement le niveau 1, sauf si c'est le tutoriel*/
        
        if (currentLevel == 0)
            UI_Dragon.currentHealth = UI_Dragon.maxHealth;
        int level = currentLevel;
        GameManager.OnSceneLoadedEvent -= LoadLevel;

        DungeonSO dungeonSo;
        
        if (TutorialDone)
            dungeonSo = dungeons[SelectedBiome].dungeonSO;
        else
            dungeonSo = TutorialDungeon;
        
        if (currentLevel == 5)//si on a battu le niveau milestonne
        {
            PlayerPrefs.SetInt("LevelJustUnlock"+ (SelectedBiome+1), 1); //on unlock le biome suivant;
        }
        if (currentLevel >= dungeonSo.levels.Count) //le donjon a été parcouru en entier
        {
            if (TutorialDone)
            {
                AchievmentSteamChecker._instance.UnlockEndLevelAchievment(SelectedBiome);
                PlayerPrefs.SetInt("LevelBeaten" + SelectedBiome, 1); //on sauvegarde le donjon comme battu
                PlayerPrefs.SetInt("LevelVictory" + SelectedBiome, PlayerPrefs.GetInt("LevelVictory" + SelectedBiome, 0) + 1); //on incremente la valeur de victoire du donjon
            }
            else
            {
                PlayerPrefs.SetInt("FinishedTutorial", 1);
                TutorialDone = true;
            }

            Debug.LogWarning("Level is too high");
            SceneManager.LoadScene(0);
            ResetLevelIndex();
            return;
        }
        

        var levelData = dungeonSo.levels[level];
        
        terrainData = levelData.terrains[Random.Range(0, levelData.terrains.Count)];
        List<HeroSO> heroUnlock = new List<HeroSO>();
        foreach (var hero in levelData.heros)
        {
            string keyName = hero.keyToUnlock;
            if (keyName == "")
            {
                heroUnlock.Add(hero);
                continue;
            }
            if (PlayerPrefs.HasKey(keyName))
            {
                heroUnlock.Add(hero);
            }
        }

        var heroSo = heroUnlock[Random.Range(0, heroUnlock.Count)];
        heroData = new HeroSOInstance(heroSo);
        deckData = levelData.decks[Random.Range(0, levelData.decks.Count)];
        
        UI_Hero heroCard = FindObjectOfType<UI_Hero>();
        heroCard.heroName.text = heroData.nameOfHero;
        heroCard.HealthBarText.text = heroData.health.ToString();
        RefreshCard(heroData);
        cardsManager = FindObjectOfType<DeckManager>();
        cardsManager.deckToBuild = deckData.deck;
        cardsManager.handToBuild = levelData.PrebuildHand;
        cardsManager.nbCardOnStartToDraw = levelData.nbCardToDraw;
        
        
        
        tickManager = FindObjectOfType<TickManager>();
        
        gameManager = GameManager.Instance;
        gameManager.currentHero = heroData;
        //gameManager.currentPersonality = heroData.personnalities[0]; //a changer a l'avenir, le hero pourra avoir plusieurs personnalité
        gameManager.heroHealthPoint = heroData.health;
        gameManager.heroCurrentHealthPoint = heroData.health;
        gameManager.UpdateHeroPos(terrainData.SpawnPosition);
        gameManager.rotationOfSpawnTile = terrainData.spawnRotation;
        
        mapManager = FindObjectOfType<MapManager>();

        
        mapManager.SpawnPresets(terrainData.tilePresets);
        
        PlayerCardController.Instance.isDragNDrop = PlayerPrefs.GetInt("DragNDrop", 0) == 1;
        
        OnLevelLoaded?.Invoke();
    }
    
    private void LoadEndlessLevel()
    {
        if (currentLevel == 0)
            UI_Dragon.currentHealth = UI_Dragon.maxHealth;
        int level = currentLevel;
        GameManager.OnSceneLoadedEvent -= LoadEndlessLevel;
        terrainData = endlessLevel.terrains[Random.Range(0, endlessLevel.terrains.Count)];
        deckData = endlessLevel.decks[Random.Range(0, endlessLevel.decks.Count)];
        
        
        
        heroData = new HeroSOInstance();
        //nom du hero
        heroData.nameOfHero = names[Random.Range(0, names.Count)];
        //classe du hero
        float classRandom = Random.Range(0, 1f);
        int classIndex;
        if (classRandom<0.7f) classIndex = 0;
        else if (classRandom<0.8f) classIndex = 1;
        else if (classRandom < 0.9f) classIndex = 2;
        else classIndex = 3;
        heroData.classe = classes[classIndex];
        
        //vision du hero
        float visionRandom = Random.Range(0, 1f);
        int visionIndex;
        if (visionRandom<0.8f) visionIndex = 0;
        else if (visionRandom<0.9f) visionIndex = 1;
        else visionIndex = 2;
        heroData.visionType = (VisionType) visionIndex;
        
        //agressivité du hero
        float agressivityRandom = Random.Range(0, 1f);
        int agressivityIndex;
        if (agressivityRandom<0.8f) agressivityIndex = 0;
        else if (agressivityRandom<0.9f) agressivityIndex = 1;
        else agressivityIndex = 2;
        heroData.aggressivity = (Aggressivity) agressivityIndex;
        
        //curiosité du hero
        float curiosityRandom = Random.Range(0, 1f);
        int curiosityIndex;
        if (curiosityRandom<0.8f) curiosityIndex = 0;
        else if (curiosityRandom<0.9f) curiosityIndex = 1;
        else curiosityIndex = 2;
        if(curiosityIndex>0) heroData.personalities.Add((Personnalities) curiosityIndex);
        
        //pv du hero
        heroData.health =(int) (mostPV + (mostPV - leastPV)*Mathf.Exp(-expFactor*-1)*modePVRelatif[machValue]
            *ClassePVRelatif[classIndex]
            *VisionPVRelatif[visionIndex]
            *AggressivityPVRelatif[agressivityIndex]
            *PersonalityPVRelatif[curiosityIndex]);
        
        //vitesse du hero
        heroData.speed = (int) (fastest- (fastest - slowest)*Mathf.Exp(-expFactor*level-1)*modeSpeedRelatif[machValue]
            *ClasseSpeedRelatif[classIndex]
            *VisionSpeedRelatif[visionIndex]
            *AggressivitySpeedRelatif[agressivityIndex]
            *PersonalitySpeedRelatif[curiosityIndex]);
        
        UI_Hero heroCard = FindObjectOfType<UI_Hero>();
        heroCard.heroName.text = heroData.nameOfHero;
        heroCard.HealthBarText.text = heroData.health.ToString();
        RefreshCard(heroData);
        cardsManager = FindObjectOfType<DeckManager>();
        cardsManager.deckToBuild = deckData.deck;
        cardsManager.handToBuild = endlessLevel.PrebuildHand;
        cardsManager.nbCardOnStartToDraw = endlessLevel.nbCardToDraw;
        tickManager = FindObjectOfType<TickManager>();
        gameManager = GameManager.Instance;
        gameManager.currentHero = heroData;
        //gameManager.currentPersonality = heroData.personnalities[0]; //a changer a l'avenir, le hero pourra avoir plusieurs personnalité
        gameManager.heroHealthPoint = heroData.health;
        gameManager.heroCurrentHealthPoint = heroData.health;
        gameManager.UpdateHeroPos(terrainData.SpawnPosition);
        gameManager.rotationOfSpawnTile = terrainData.spawnRotation;
        
        mapManager = FindObjectOfType<MapManager>();
        mapManager.SpawnPresets(terrainData.tilePresets);
        PlayerCardController.Instance.isDragNDrop = PlayerPrefs.GetInt("DragNDrop", 0) == 1;
        OnLevelLoaded?.Invoke();
        
    }
    
    public void LoadNextLevel()
    {
        if (TutorialDone)
        {
            foreach (var key in dungeons[SelectedBiome].dungeonSO.levels[currentLevel].keysElementsToUnlock)
            {
                PlayerPrefs.SetInt(key, 1);
            }
        }
        currentLevel++;  

        if(machValue==0) GameManager.OnSceneLoadedEvent += LoadLevel;
        else GameManager.OnSceneLoadedEvent += LoadEndlessLevel;
    }

    public void RefreshCard(HeroSOInstance heroData)
    {
        UI_Hero heroCard = FindObjectOfType<UI_Hero>();
        heroCard.heroName.text = heroData.nameOfHero;
        heroCard.HealthBarText.text = heroData.health.ToString();
        string personality = "";
        if (heroData.visionType != VisionType.LIGNEDROITE) personality += ToTitleCase(heroData.visionType.ToString()) + " ";
        if (heroData.aggressivity != Aggressivity.NONE) personality += ToTitleCase(heroData.aggressivity.ToString());
        foreach (var perso in heroData.personalities)
        {
            personality += " " + ToTitleCase(perso.ToString());
        }
        heroCard.heroPersonality.text = personality;
        Debug.LogWarning(personality);
    }
    
    public void ResetLevelIndex()
    {
        print("reset!");
        currentLevel = 0;
    }
}
