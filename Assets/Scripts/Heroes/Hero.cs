using System.Collections;
using System;
using DG.Tweening;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public int indexHeroX { get; set; }
    public int indexHeroY { get; set; }
    private int entityId;
    public MapManager mapManager { get; private set; }
    public bool isDead { get; set; }
    public HeroInstance info{ get; private set; }

    public static event Action OnMovedOnEmptyCardEvent;
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
        Sprite.sprite = info.So.Img;
        StartCoroutine(WaitForBeginToMove());
    }

    private IEnumerator WaitForBeginToMove()
    {
        yield return new WaitForSeconds(info.So.delayBeginToMove);
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

    public void TakeDamage(int soAttackPoint)
    {
        info.CurrentHealthPoint -= soAttackPoint;
        if (info.CurrentHealthPoint <= 0)
        {
            isDead = true;
            //TODO: t'as gagne bg :*
        }
    }
}