using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Ink.Parsed;
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
   
    [SerializeField] public DungeonSO TutorialDungeon;
    [SerializeField] public List<Dungeon> dungeons;
   
    public int currentLevel = 0;
    public DeckManager cardsManager;
    public TickManager tickManager;
    private GameManager gameManager;
    private MapManager mapManager;
    public static int SelectedBiome;
    
    public TilePresetSO terrainData;
    public HeroSO heroData;
    public DeckSO deckData;
    
    
    public DialogueVariable dialogueVariable;
    [Header("Le truc ink la")]
    [SerializeField] private TextAsset globalsInkFile;

    public static DungeonManager _instance;
    public bool TutorialDone = true;
    
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
        SelectedBiome = index;
        GameManager.OnSceneLoadedEvent += LoadLevel;
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
            PlayerPrefs.SetInt("LevelUnlock"+ (SelectedBiome+1), 1); //on unlock le biome suivant;
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
        heroData = heroUnlock[Random.Range(0, heroUnlock.Count)];
        deckData = levelData.decks[Random.Range(0, levelData.decks.Count)];
        
        UI_Hero heroCard = FindObjectOfType<UI_Hero>();
        heroCard.heroName.text = heroData.nameOfHero;
        heroCard.HealthBarText.text = heroData.health.ToString();
        string personality = "";
        if (heroData.visionType != VisionType.LIGNEDROITE) personality += ToTitleCase(heroData.visionType.ToString()) + " ";
        if (heroData.aggressivity != Aggressivity.NONE) personality += ToTitleCase(heroData.aggressivity.ToString());
        // heroCard.heroPersonality.text = ToTitleCase(heroData.visionType.ToString() + " " + heroData.aggressivity);
        heroCard.heroPersonality.text = personality;
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

        GameManager.OnSceneLoadedEvent += LoadLevel;
    }

    public void ResetLevelIndex()
    {
        print("reset!");
        currentLevel = 0;
    }
}
