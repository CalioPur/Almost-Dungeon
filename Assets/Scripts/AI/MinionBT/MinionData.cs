using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class MinionData : TrapData
{
    public static event Action OnHeroMoved;
   
    [SerializeField] protected Transform tr;
    [SerializeField] protected SpriteRenderer sprite;
    [SerializeField] protected MinionBTBase bt;
    public Queue<AnimToQueue> animQueue = new Queue<AnimToQueue>();
    private bool isExec = false;
    
    public EnemyInstance minionInstance;
    
    public void GetHeroPos()
    {
        OnHeroMoved?.Invoke();
    }

    public void Move(Vector3 pos)
    {
        
        
        tr.DOMove(pos + new Vector3(1, 0.1f, 1), 0.5f);
        animQueue.Enqueue(new AnimToQueue(tr, pos + new Vector3(1, 0.1f, 1), 0.5f));
        StartCoroutine(doAnim());
    }
    
    public override void TakeDamage(int damage)
    {
        if (isDead) return;
        
        minionInstance.CurrentHealthPoint -= damage;
        if(minionInstance.CurrentHealthPoint<=0)
        {
            isDead = true;
            OnDead();
        }
    }

    protected override void Init()
    {
        minionInstance = SO.CreateInstance();
        GameManager.OnGameStartEvent += StartListenTick;
       
    }
    public void StartListenTick()
    {
        TickManager.SubscribeToMovementEvent(MovementType.Monster, OnTick, entityId);
    }

    public IEnumerator doAnim()
    {
        if (isExec) yield break;
        while (animQueue.Count > 0)
        {
            isExec = true;
            AnimToQueue anim = animQueue.Dequeue();
            tr.DOMove(anim.target, anim.time).SetEase(anim.ease).SetLoops(anim.loop, LoopType.Yoyo);
            yield return new WaitForSeconds(anim.time);
        }
        isExec = false;
        yield return null;
    } 
}
