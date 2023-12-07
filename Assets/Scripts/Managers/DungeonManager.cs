using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public struct Dungeon
{
    [field: SerializeField] public List<CardToBuild> Deck { get; private set; }
    [SerializeField] public List<LevelSO> dungeonSos;
    [SerializeField] public string name;
}

public class DungeonManager : MonoBehaviour
{
   
    [SerializeField] public List<Dungeon> dungeons;
   
    private static int currentLevel = 0;
    private DeckManager cardsManager;
    private TickManager tickManager;
    private GameManager gameManager;
    private MapManager mapManager;
    private int SelectedBiome = 0;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        gameManager = GameManager._instance;
        
    }

    public void SetSelectedBiome(int index)
    {
        SelectedBiome = index;
        SceneManager.LoadScene(1);
        GameManager.OnSceneLoadedEvent += LoadLevel;
    }
    
    
    private void LoadLevel()
    {
        print(currentLevel);
        int level = currentLevel;
        GameManager.OnSceneLoadedEvent -= LoadLevel;
        if (level >= dungeons[SelectedBiome].dungeonSos.Count)
        {
            Debug.LogWarning("Level is too high");
            SceneManager.LoadScene(0);
            ResetLevelIndex();
            return;
        }
        var dungeonSo = dungeons[SelectedBiome].dungeonSos[level];
        cardsManager = FindObjectOfType<DeckManager>();
        cardsManager.deckToBuild = dungeons[SelectedBiome].Deck;
        cardsManager.nbCardOnStartToDraw = dungeonSo.initialNbCardInHand;
        
        tickManager = FindObjectOfType<TickManager>();
        tickManager.actionsTime = dungeonSo.tickData;
        
        gameManager = FindObjectOfType<GameManager>();
        gameManager.currentHero = dungeonSo.HeroesInfo;
        gameManager.heroHealthPoint = dungeonSo.nbHealthHeroInitial;
        gameManager.normsSpawnX = dungeonSo.clampedSpawnEnterDungeonX;
        gameManager.normsSpawnY = dungeonSo.clampedSpawnEnterDungeonY;
        
        mapManager = FindObjectOfType<MapManager>();

        
        mapManager.SpawnPresets(dungeonSo.dungeonPreset);
    }
    public void LoadNextLevel()
    {
        currentLevel++;
        GameManager.OnSceneLoadedEvent += LoadLevel;
    }

    public static void ResetLevelIndex()
    {
        currentLevel = 0;
    }
}
