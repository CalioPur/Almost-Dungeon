using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static Action OnBeginToMoveEvent;
    
    [Header("Managers")]
    [SerializeField] private MapManager mapManager;
    [SerializeField] private TickManager tick;
    
    [Header("Data")]
    [SerializeField] private List<HeroesInfo> heroesInfos;
    [SerializeField] private CardInfo enterDungeonInfo;
    [SerializeField] private Hero heroRenderer;

    private bool WaitForBeginToMove = true;


    void Start()
    {
        MapManager.OnCardTryToPlaceEvent += CheckIsFirstMove; 
        
        mapManager.InitMap();
        Vector3 pos;
        int indexHeroX;
        int indexHeroY;
        mapManager.InitEnterDungeon(enterDungeonInfo.CreateInstance(), out pos, out indexHeroX, _y: out indexHeroY);
        pos += new Vector3(1, 0.1f, 1); //pour que le hero soit au dessus du sol
        mapManager.AddRandomCard();
        int randomHero = Random.Range(0, heroesInfos.Count);
        HeroInstance current = heroesInfos[randomHero].CreateInstance();
        
        Instantiate(heroRenderer, pos, heroRenderer.transform.rotation).Init(current, indexHeroX, indexHeroY, mapManager);
    }

    private void CheckIsFirstMove(TileData _, CardHand __, bool ___)
    {
        WaitForBeginToMove = false;
        MapManager.OnCardTryToPlaceEvent -= CheckIsFirstMove;
        OnBeginToMoveEvent?.Invoke();
    }
}