using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private Transform heroTr;
    [SerializeField] private SpriteRenderer Sprite;
    
    private int indexHeroX;
    private int indexHeroY;
    private MapManager mapManager;
    private HeroInstance info;
    
    IEnumerator Move()
    {
        yield return new WaitForSeconds(0.5f);
        Vector3 pos;
        mapManager.GetWorldPosFromTilePos(indexHeroX + 1, indexHeroY, out pos);
        heroTr.DOMove(pos + new Vector3(1, 0.1f, 1), 0.5f);
        StartCoroutine(Move());
    }

    public void Init(HeroInstance instance, int _indexHeroX, int _indexHeroY, MapManager manager)
    {
        indexHeroX = _indexHeroX;
        indexHeroY = _indexHeroY;
        mapManager = manager;
        info = instance;
        Sprite.sprite = info.So.Img;
        StartCoroutine(Move());
    }
}
