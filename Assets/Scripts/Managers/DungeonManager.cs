using System;
using System.Collections;
using System.Collections.Generic;
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
   
    private static int currentLevel = 0;
    private DeckManager cardsManager;
    private TickManager tickManager;
    private GameManager gameManager;
    private MapManager mapManager;
    public static int SelectedBiome;

    private static DungeonManager _instance;
    
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

        DialogueManager._instance.StartDialogue(0);
    }

    public void SetSelectedBiome(int index)
    {
        SelectedBiome = index;
        GameManager.OnSceneLoadedEvent += LoadLevel;
        SceneManager.LoadScene(1);
    }
    
    
    private void LoadLevel()
    {
        if (currentLevel == 0)
            UI_Dragon.currentHealth = UI_Dragon.maxHealth;
        print(UI_Dragon.currentHealth);
        int level = currentLevel;
        GameManager.OnSceneLoadedEvent -= LoadLevel;
        print("nb of level : "+dungeons[SelectedBiome].dungeonSO.levels.Count);
        if (level >= dungeons[SelectedBiome].dungeonSO.levels.Count)
        {
            Debug.LogWarning("Level is too high");
            SceneManager.LoadScene(0);
            ResetLevelIndex();
            return;
        }



        var levelData = dungeons[SelectedBiome].dungeonSO.levels[level];
        
        var terrainData = levelData.terrains[Random.Range(0, levelData.terrains.Count)];
        var heroData = levelData.heros[Random.Range(0, levelData.heros.Count)];
        var deckData = levelData.decks[Random.Range(0, levelData.decks.Count)];
        
        cardsManager = FindObjectOfType<DeckManager>();
        cardsManager.deckToBuild = deckData.deck;
        cardsManager.nbCardOnStartToDraw = levelData.nbCardToDraw;
        
        
        
        
        tickManager = FindObjectOfType<TickManager>();
        tickManager.BPM = heroData.speed;
        
        gameManager = FindObjectOfType<GameManager>();
        gameManager.currentHero = heroData.classe;
        gameManager.currentPersonality = heroData.personnalities[0]; //a changer a l'avenir, le hero pourra avoir plusieurs personnalité
        gameManager.heroHealthPoint = heroData.health;
        gameManager.normsSpawnX = terrainData.ClampSpawnPositionX;
        gameManager.normsSpawnY = terrainData.ClampSpawnPositionY;
        gameManager.rotationOfSpawnTile = terrainData.spawnRotation;
        
        mapManager = FindObjectOfType<MapManager>();

        
        mapManager.SpawnPresets(terrainData.tilePresets);
    }
    public void LoadNextLevel()
    {
        currentLevel++;
        GameManager.OnSceneLoadedEvent += LoadLevel;
    }

    public static void ResetLevelIndex()
    {
        print("reset!");
        currentLevel = 0;
    }
}
