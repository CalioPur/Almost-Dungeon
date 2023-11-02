using System.Collections;
using System;
using DG.Tweening;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public static event Action<Vector2Int> OnGivePosBackEvent;
    public static event Action OnTakeDamageEvent;
    public static event Action<int> OnPopUpEvent;
    
    
    public static event Action OnMovedOnEmptyCardEvent;
    public int indexHeroX { get; set; }
    public int indexHeroY { get; set; }
    private int entityId;
    public MapManager mapManager { get; private set; }
    public bool isDead { get; set; }
    public HeroInstance info { get; private set; }
    [SerializeField] private Transform heroTr;
    [SerializeField] private SpriteRenderer Sprite;
    [SerializeField] private SimpleHeroBT bt;


    public void Move(Vector3 pos)
    {
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
        Sprite.sprite = info.So.Img;
        OnPopUpEvent?.Invoke(info.CurrentHealthPoint);
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
    }

    private void IsDead()
    {
        isDead = true;
        //TODO: t'as gagne bg :*
        OnTakeDamageEvent?.Invoke();
    }

    public void TakeDamage(int soAttackPoint)
    {
        info.CurrentHealthPoint -= soAttackPoint;
        if (info.CurrentHealthPoint <= 0)
        {
            IsDead();
        }
    }

    private void Start()
    {
        OnBeginToMove();
    }
}