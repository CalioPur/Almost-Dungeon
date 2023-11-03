using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TrapData : MonoBehaviour
{
    [SerializeField] protected EnemySo SO;
    public abstract void TakeDamage(int damage);
    public bool isDead;
    public int indexMinionX;
    public int indexMinionY;
    public MapManager mapManager;
    
    protected int entityId;
    
    protected abstract void OnTick();
    protected abstract void Init();
    protected abstract void OnDead();
    
    public virtual void Attack()
    {
        
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
    
}
