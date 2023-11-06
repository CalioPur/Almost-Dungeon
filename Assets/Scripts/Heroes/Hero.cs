using System.Collections;
using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public static event Action<Vector2Int> OnGivePosBackEvent;
    public static event Action<int, bool> OnTakeDamageEvent;
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
    public Queue<AnimToQueue> animQueue = new Queue<AnimToQueue>();
    private bool isExec = false;

    public void Move(Vector3 pos)
    {
        animQueue.Enqueue(new AnimToQueue(heroTr, pos + new Vector3(1, 0.1f, 1), 0.5f));
        StartCoroutine(doAnim());
        
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

        TrapData.OnTrapAttackEvent += TakeDamage;
        Sprite.sprite = info.So.Img;
        OnPopUpEvent?.Invoke(info.CurrentHealthPoint);
        MinionData.OnHeroPosAsked+= GivePosBack;
    }

    private void GivePosBack()
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
    }

    public void TakeDamage(int soAttackPoint)
    {
        info.CurrentHealthPoint -= soAttackPoint;
        OnTakeDamageEvent?.Invoke(info.CurrentHealthPoint, true);
        if (info.CurrentHealthPoint <= 0)
        {
            IsDead();
        }
    }

    private void Start()
    {
        OnBeginToMove();
    }
    
    public IEnumerator doAnim()
    {
        if (isExec) yield break;
        while (animQueue.Count > 0)
        {
            isExec = true;
            AnimToQueue anim = animQueue.Dequeue();
            heroTr.DOMove(anim.target, anim.time).SetEase(anim.ease).SetLoops(anim.loop, LoopType.Yoyo);
            yield return new WaitForSeconds(anim.time);
        }
        isExec = false;
        yield return null;
    } 
}