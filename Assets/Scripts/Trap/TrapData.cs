using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class TrapData : MonoBehaviour
{
    public static event Action<int> OnTrapAttackEvent; 
    public static event Action OnTrapStunEvent; 
    public abstract void TakeDamage(int damage);
    
    public bool isDead;
    public int indexX;
    public int indexY;
    [HideInInspector] public MapManager mapManager;
    [SerializeField] protected SpriteRenderer sprite;
    
    [SerializeField] protected EnemySo SO;
    protected int entityId;
    
    protected bool isStunned;
    
    protected abstract void OnTick();
    protected abstract void Init();

    IEnumerator Dying()
    {
        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
    }
    protected virtual void OnDead()
    {
        mapManager.GetWorldPosFromTilePos(new Vector2Int(indexX, indexY) , out Vector3 worldPos);
        mapManager.RemoveEnemyOnTile(
            new Vector2Int(indexX, indexY), this, worldPos);
        isDead = true;
        StartCoroutine(Dying());
    }

    public void Attack(int damage)
    {
        InvokeTrapAttackEvent(damage);
    }
    
    public void Stun()
    {
        OnTrapStunEvent?.Invoke();
    }
    
    public EnemySo GetSO()
    {
        return SO;
    }
    
    public void StartListenTick(MovementType movementType)
    {
        TickManager.SubscribeToMovementEvent(movementType, OnTick, out entityId);
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

    public static void ClearEvent()
    {
        OnTrapAttackEvent = null;
    }

    public void Stunned()
    {
        
    }
}
