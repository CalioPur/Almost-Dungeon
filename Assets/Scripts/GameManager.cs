using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<HeroesInfo> heroesInfos;

    [SerializeField] private MapManager mapManager;

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
        
        int randomHero = Random.Range(0, heroesInfos.Count);
        HeroInstance current = heroesInfos[randomHero].CreateInstance();
        
        Instantiate(heroRenderer, pos, heroRenderer.transform.rotation).Init(current, indexHeroX, indexHeroY, mapManager);
    }
}