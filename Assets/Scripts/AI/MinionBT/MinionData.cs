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
    
    public EnemyInstance minionInstance;
    
    public void GetHeroPos()
    {
        OnHeroMoved?.Invoke();
    }

    public void Move(Vector3 pos)
    {
        
        
        tr.DOMove(pos + new Vector3(1, 0.1f, 1), 0.5f);
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
}
