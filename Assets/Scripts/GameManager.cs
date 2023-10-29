using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private MapManager mapManager;
    [SerializeField] private TickManager tick;
    
    [Header("Data")]
    [SerializeField] private List<HeroesInfo> heroesInfos;
    [SerializeField] private CardInfo enterDungeonInfo;
    [SerializeField] private Hero heroRenderer;


    void Start()
    {
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
}