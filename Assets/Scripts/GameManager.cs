using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static Action OnGameStartEvent;

    [Header("Managers")] [SerializeField] private MapManager mapManager;
    [SerializeField] private TickManager tick;

    [Header("Data")] [SerializeField] private List<HeroesInfo> heroesInfos;
    [SerializeField] private CardInfo enterDungeonInfo;
    [SerializeField] private Hero heroRenderer;
    [SerializeField] private Vector2Int startPosHero;

    private Vector3 pos;

    void Start()
    {
        MapManager.OnCardTryToPlaceEvent += CheckIsFirstMove;

        mapManager.InitMap();
        mapManager.InitEnterDungeon(enterDungeonInfo.CreateInstance(), out pos, startPosHero);
        pos += new Vector3(1, 0.1f, 1); //pour que le hero soit au dessus du sol
        mapManager.AddRandomCard();
    }

    private void SpawnHero()
    {
        int randomHero = Random.Range(0, heroesInfos.Count);
        HeroInstance current = heroesInfos[randomHero].CreateInstance();

        Instantiate(heroRenderer, pos, heroRenderer.transform.rotation)
            .Init(current, startPosHero.x, startPosHero.y, mapManager);
    }

    private void CheckIsFirstMove(TileData _, CardHand __, bool canBePlaced)
    {
        if (!canBePlaced) return;
        MapManager.OnCardTryToPlaceEvent -= CheckIsFirstMove;
        SpawnHero();
        OnGameStartEvent?.Invoke();
    }
}