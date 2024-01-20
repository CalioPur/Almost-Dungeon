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
    

    public static DungeonManager _instance;
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        
        DontDestroyOnLoad(this);
        gameManager = GameManager._instance;

        
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
        PlayerPrefs.SetInt("LevelUnlock" + 1, 1); //on unlock le niveau 2 au lancement d'un donjon, qui sera forcement le niveau 1
        
        if (currentLevel == 0)
            UI_Dragon.currentHealth = UI_Dragon.maxHealth;
        print(UI_Dragon.currentHealth);
        int level = currentLevel;
        GameManager.OnSceneLoadedEvent -= LoadLevel;
        print("nb of level : "+dungeons[SelectedBiome].dungeonSO.levels.Count);
        if (currentLevel == 5)//si on a battu le niveau milestonne
        {
            PlayerPrefs.SetInt("LevelUnlock"+ (SelectedBiome+1), 1); //on unlock le biome suivant;
        }
        if (currentLevel >= dungeons[SelectedBiome].dungeonSO.levels.Count) //le donjon a été parcouru en entier
        {
            PlayerPrefs.SetInt("LevelBeaten" + SelectedBiome, 1); //on sauvegarde le donjon comme battu
            PlayerPrefs.SetInt("LevelVictory" + SelectedBiome, PlayerPrefs.GetInt("LevelVictory" + SelectedBiome, 0) + 1); //on incremente la valeur de victoire du donjon
            
            Debug.LogWarning("Level is too high");
            SceneManager.LoadScene(0);
            ResetLevelIndex();
            return;
        }

        

        var levelData = dungeons[SelectedBiome].dungeonSO.levels[level];
        
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
        string personality = "";
        if (heroData.visionType != VisionType.LIGNEDROITE) personality += ToTitleCase(heroData.visionType.ToString()) + " ";
        if (heroData.aggressivity != Aggressivity.NONE) personality += ToTitleCase(heroData.aggressivity.ToString());
        // heroCard.heroPersonality.text = ToTitleCase(heroData.visionType.ToString() + " " + heroData.aggressivity);
        heroCard.heroPersonality.text = personality;
        cardsManager = FindObjectOfType<DeckManager>();
        cardsManager.deckToBuild = deckData.deck;
        cardsManager.nbCardOnStartToDraw = levelData.nbCardToDraw;
        
        
        
        tickManager = FindObjectOfType<TickManager>();
        tickManager.BPM = heroData.speed;
        
        gameManager = GameManager._instance;
        gameManager.currentHero = heroData;
        //gameManager.currentPersonality = heroData.personnalities[0]; //a changer a l'avenir, le hero pourra avoir plusieurs personnalité
        gameManager.heroHealthPoint = heroData.health;
        gameManager.heroCurrentHealthPoint = heroData.health;
        gameManager.normsSpawnX = terrainData.ClampSpawnPositionX;
        gameManager.normsSpawnY = terrainData.ClampSpawnPositionY;
        gameManager.rotationOfSpawnTile = terrainData.spawnRotation;
        
        mapManager = FindObjectOfType<MapManager>();

        
        mapManager.SpawnPresets(terrainData.tilePresets);
        
        MovementManager.Instance.isDragNDrop = PlayerPrefs.GetInt("DragNDrop", 0) == 1;
    }
    public void LoadNextLevel()
    {
        foreach (var key in dungeons[SelectedBiome].dungeonSO.levels[currentLevel].keysElementsToUnlock)
        {
            PlayerPrefs.SetInt(key, 1);
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
