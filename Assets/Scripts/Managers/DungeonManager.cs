using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public struct Biome
{
    [field: SerializeField] public List<CardToBuild> Deck { get; private set; }
    [SerializeField] public List<DungeonSO> dungeonSos;
    [SerializeField] private string name;
}

public class DungeonManager : MonoBehaviour
{
   
    [SerializeField] private List<Biome> biomes;
   
    private static int currentLevel = 0;
    private DeckManager cardsManager;
    private TickManager tickManager;
    private GameManager gameManager;
    private MapManager mapManager;
    private int SelectedBiome = 0;
    
    
    private void Awake()
    {
        LoadLevel(currentLevel);
    }
    
    
    private void LoadLevel(int level)
    {
        print(currentLevel);
        if (level >= biomes[SelectedBiome].dungeonSos.Count)
        {
            Debug.LogError("Level is too high");
            SceneManager.LoadScene(0);
            return;
        }
        var dungeonSo = biomes[SelectedBiome].dungeonSos[level];
        cardsManager = FindObjectOfType<DeckManager>();
        cardsManager.deckToBuild = biomes[SelectedBiome].Deck;
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
        //LoadLevel(currentLevel);
    }

    public static void ResetLevelIndex()
    {
        currentLevel = 0;
    }
}
