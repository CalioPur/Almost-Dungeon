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
   
    public static int currentLevel = 0;
    private DeckManager cardsManager;
    private TickManager tickManager;
    private GameManager gameManager;
    private MapManager mapManager;
    public static int SelectedBiome;

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
        if (currentLevel == 0)
            UI_Dragon.currentHealth = UI_Dragon.maxHealth;
        print(UI_Dragon.currentHealth);
        int level = currentLevel;
        GameManager.OnSceneLoadedEvent -= LoadLevel;
        print("nb of level : "+dungeons[SelectedBiome].dungeonSO.levels.Count);
        if (currentLevel >= dungeons[SelectedBiome].dungeonSO.levels.Count)
        {
            Debug.LogWarning("Level is too high");
            SceneManager.LoadScene(0);
            ResetLevelIndex();
            return;
        }

        

        var levelData = dungeons[SelectedBiome].dungeonSO.levels[level];
        
        var terrainData = levelData.terrains[Random.Range(0, levelData.terrains.Count)];
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
        var heroData = heroUnlock[Random.Range(0, heroUnlock.Count)];
        var deckData = levelData.decks[Random.Range(0, levelData.decks.Count)];
        
        UI_Hero heroCard = FindObjectOfType<UI_Hero>();
        heroCard.heroName.text = heroData.nameOfHero;
        heroCard.heroPersonality.text = ToTitleCase(heroData.visionType.ToString() + " " + heroData.aggressivity);
        
        cardsManager = FindObjectOfType<DeckManager>();
        cardsManager.deckToBuild = deckData.deck;
        cardsManager.nbCardOnStartToDraw = levelData.nbCardToDraw;
        
        DialogueManager._instance.PlayAllThreeDialogues(terrainData.terrainDialogue, deckData.deckDialogue,
                heroData.heroDialogues, cardsManager);
        
        
        tickManager = FindObjectOfType<TickManager>();
        tickManager.BPM = heroData.speed;
        
        gameManager = FindObjectOfType<GameManager>();
        gameManager.currentHero = heroData;
        //gameManager.currentPersonality = heroData.personnalities[0]; //a changer a l'avenir, le hero pourra avoir plusieurs personnalité
        gameManager.heroHealthPoint = heroData.health;
        gameManager.heroCurrentHealthPoint = heroData.health;
        gameManager.normsSpawnX = terrainData.ClampSpawnPositionX;
        gameManager.normsSpawnY = terrainData.ClampSpawnPositionY;
        gameManager.rotationOfSpawnTile = terrainData.spawnRotation;
        
        mapManager = FindObjectOfType<MapManager>();

        
        mapManager.SpawnPresets(terrainData.tilePresets);
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

    public static void ResetLevelIndex()
    {
        print("reset!");
        currentLevel = 0;
    }
}
