using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class TrapData : MonoBehaviour
{
    public static event Action<int> OnTrapAttackEvent; 
    public abstract void TakeDamage(int damage);
    
    public bool isDead;
    public int indexX;
    public int indexY;
    public MapManager mapManager;
    
    [SerializeField] protected EnemySo SO;
    protected int entityId;
    
    protected abstract void OnTick();
    protected abstract void Init();
    protected abstract void OnDead();

    public void Attack(int damage)
    {
        InvokeTrapAttackEvent(damage);
    }
    
    public EnemySo GetSO()
    {
        return SO;
    }
    
    public void StartListenTick(MovementType movementType)
    {
        TickManager.SubscribeToMovementEvent(movementType, OnTick, entityId);
    }
    
    void Start()
    {
        Init();
    }

    protected void InvokeTrapAttackEvent(int damage)
    {
        OnTrapAttackEvent?.Invoke(damage);
    }
}
