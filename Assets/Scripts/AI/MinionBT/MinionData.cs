using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class MinionData : TrapData
{
    public static event Action OnHeroMoved;

    
    public int indexMinionX;
    public int indexMinionY;
    protected int entityId;
    public MapManager mapManager;
    
   
    [SerializeField] protected Transform tr;
    [SerializeField] protected SpriteRenderer sprite;
    [SerializeField] protected MinionBTBase bt;
    [SerializeField] protected minionSOScript minionSO;
    
    public MinionInstance minionInstance;
    
    protected abstract void MinionDie();
    protected abstract void OnTick();
    
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
        
        Debug.Log("ENNEMI TAKE DAMAGE");
        minionInstance.CurrentHealthPoint -= damage;
        if(minionInstance.CurrentHealthPoint<=0)
        {
            isDead = true;
            MinionDie();
        }
    }

    protected void Init()
    {
        minionInstance = minionSO.CreateInstance();
    }
    public void StartListenTick()
    {
        TickManager.SubscribeToMovementEvent(MovementType.Monster, OnTick, entityId);
    }
}
