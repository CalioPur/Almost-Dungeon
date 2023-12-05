using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    [SerializeField] static int currentLevel = 0;
    [SerializeField] private List<DungeonSO> dungeonSos;
    [SerializeField] private MapManager mapManager;
    
    DeckManager cardsManager;
    TickManager tickManager;
    GameManager gameManager;
    
    
    private void Awake()
    {
        LoadLevel(currentLevel);
    }
    
    
    private void LoadLevel(int level)
    {
        print(currentLevel);
        if (level >= dungeonSos.Count)
        {
            Debug.LogError("Level is too high");
            return;
        }
        var dungeonSo = dungeonSos[level];
        cardsManager = FindObjectOfType<DeckManager>();
        cardsManager.deckToBuild = dungeonSo.Deck;
        cardsManager.nbCardOnStartToDraw = dungeonSo.initialNbCardInHand;
        
        tickManager = FindObjectOfType<TickManager>();
        tickManager.actionsTime = dungeonSo.tickData;
        
        gameManager = FindObjectOfType<GameManager>();
        gameManager.currentHero = dungeonSo.HeroesInfo;
        gameManager.heroHealthPoint = dungeonSo.nbHealthHeroInitial;
        gameManager.normsSpawnX = dungeonSo.clampedSpawnEnterDungeonX;
        gameManager.normsSpawnY = dungeonSo.clampedSpawnEnterDungeonY;
        
        mapManager.SpawnPresets(dungeonSo.dungeonPreset);
    }
    public void LoadNextLevel()
    {
        currentLevel++;
        //LoadLevel(currentLevel);
    }
}
