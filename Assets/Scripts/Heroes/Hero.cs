using System.Collections;
using System;
using DG.Tweening;
using UnityEngine;

public class Hero : MonoBehaviour
{    
    public int indexHeroX { get; private set; }
    public int indexHeroY { get; private set; }
    public MapManager mapManager { get; private set; }
    public static event Action OnMovedOnEmptyCardEvent;
    [SerializeField] private Transform heroTr;
    [SerializeField] private SpriteRenderer Sprite;
    [SerializeField] private SimpleHeroBT bt;

    private HeroInstance info;

    public void Move(DirectionToMove directionToMove)
    {
        Vector3 pos = Vector3.zero;
        switch (directionToMove)
        {
            case DirectionToMove.Up:
                mapManager.GetWorldPosFromTilePos(indexHeroX, indexHeroY + 1, out pos);
                indexHeroY++;
                break;
            case DirectionToMove.Down:
                mapManager.GetWorldPosFromTilePos(indexHeroX, indexHeroY - 1, out pos);
                indexHeroY--;
                break;
            case DirectionToMove.Left:
                mapManager.GetWorldPosFromTilePos(indexHeroX - 1, indexHeroY, out pos);
                indexHeroX--;
                break;
            case DirectionToMove.Right:
                mapManager.GetWorldPosFromTilePos(indexHeroX + 1, indexHeroY, out pos);
                indexHeroX++;
                break;
        }

        heroTr.DOMove(pos + new Vector3(1, 0.1f, 1), 0.5f);
        
    }

    void OnTick(int currentDivision)
    {
        if (currentDivision == 0)
        {
            if (!bt) return;
            bt.getOrigin().Evaluate(bt.getOrigin());
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
    }

    public void OutOfMap()
    {
        OnMovedOnEmptyCardEventEvent?.Invoke();
    }
}