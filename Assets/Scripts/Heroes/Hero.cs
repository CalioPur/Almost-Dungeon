using System.Collections;
using System;
using DG.Tweening;
using UnityEngine;

public class Hero : MonoBehaviour
{
    
    public static event Action OnMovedOnEmptyCardEvent;
    [SerializeField] private Transform heroTr;
    [SerializeField] private SpriteRenderer Sprite;
    
    public int indexHeroX;
    public int indexHeroY;
    private MapManager mapManager;
    private HeroInstance info;
    
    IEnumerator Move()
    {
        yield return new WaitForSeconds(5f);
        
        
        StartCoroutine(Move());
    }
    
    void OnTick(int currentDivision)
    {
        if (currentDivision == 0)
        {
            Vector3 pos;

            if (mapManager.CheckIfTileIsFree(indexHeroX + 1, indexHeroY))
            {
                indexHeroX++;
                mapManager.GetWorldPosFromTilePos(indexHeroX, indexHeroY, out pos);
                heroTr.DOMove(pos + new Vector3(1, 0.1f, 1), 0.5f);
            }
            else
            {
                
                OnMovedOnEmptyCardEventEvent?.Invoke();
            }
        }
    }

    public void Init(HeroInstance instance, int _indexHeroX, int _indexHeroY, MapManager manager)
    {
        indexHeroX = _indexHeroX;
        indexHeroY = _indexHeroY;
        mapManager = manager;
        info = instance;
        Sprite.sprite = info.So.Img;
        TickManager.OnTick += OnTick;
        //StartCoroutine(Move());
    }
}
