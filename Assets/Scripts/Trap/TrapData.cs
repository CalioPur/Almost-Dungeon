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
    [SerializeField] protected SpriteRenderer sprite;
    
    [SerializeField] protected EnemySo SO;
    protected int entityId;
    
    protected abstract void OnTick();
    protected abstract void Init();
    protected virtual void OnDead()
    {
        mapManager.GetWorldPosFromTilePos(new Vector2Int(indexX, indexY) , out Vector3 worldPos);
        mapManager.RemoveEnemyOnTile(
            new Vector2Int(indexX, indexY), this, worldPos);
        Destroy(gameObject);
    }

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
    
    public void ShowSprite()
    {
        var spriteColor = sprite.color;
        spriteColor.a = 1;
        sprite.color = spriteColor;
    }
    
    public void AddOnTile(int x, int y)
    {
        indexX = x;
        indexY = y;
        mapManager.AddTrapOnTile(new Vector2Int(x, y), this);
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
