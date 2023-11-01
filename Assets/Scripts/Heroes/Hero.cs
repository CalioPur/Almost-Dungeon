using System.Collections;
using System;
using DG.Tweening;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public static event Action<Vector2Int> OnGivePosBackEvent;
    public static event Action OnMovedOnEmptyCardEvent;

    public int indexHeroX { get; private set; }
    public int indexHeroY { get; private set; }
    
    private int entityId;
    public MapManager mapManager { get; private set; }
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

    void OnTick()
    {
        if (!bt) return;
        bt.getOrigin().Evaluate(bt.getOrigin());
    }
    

    public void Init(HeroInstance instance, int _indexHeroX, int _indexHeroY, MapManager manager)
    {
        indexHeroX = _indexHeroX;
        indexHeroY = _indexHeroY;
        mapManager = manager;
        entityId = GetHashCode();
        info = instance;

        MinionData.OnHeroMoved += SendPos;
    }
    private void SendPos()
    {
        OnGivePosBackEvent?.Invoke(new Vector2Int(indexHeroX, indexHeroY));
    }

    private void OnBeginToMove()
    {
        TickManager.SubscribeToMovementEvent(MovementType.Hero, OnTick, entityId);
    }

    public void OutOfMap()
    {
        OnMovedOnEmptyCardEvent?.Invoke();
    }
    
    private void OnDestroy()
    {
        TickManager.UnsubscribeFromMovementEvent(MovementType.Hero, gameObject.GetInstanceID());
        GameManager.OnBeginToMoveEvent -= OnBeginToMove;
    }

    private void Start()
    {
        GameManager.OnBeginToMoveEvent += OnBeginToMove;
    }
}