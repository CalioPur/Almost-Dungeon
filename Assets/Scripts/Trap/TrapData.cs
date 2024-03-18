using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class TrapData : MonoBehaviour
{
    public static event Action<int, AttackType> OnTrapAttackEvent; 
    public static event Action<TrapData> OnTrapStunEvent; 
    public abstract void TakeDamage(int damage,  AttackType attackType);
    
    public bool isDead;
    public int indexX;
    public int indexY;
    [HideInInspector] public MapManager mapManager;
    [SerializeField] protected SpriteRenderer sprite;
    
    [SerializeField] protected EnemySo SO;
    protected int entityId;
    
    protected bool isStunned;
    protected TrapData web;
    
    protected abstract void OnTick();
    protected abstract void Init();
    
    protected virtual void OnDead()
    {
        SoundManagerIngame.Instance.PlayDialogueSFX(SO.deathSound);
        mapManager.GetWorldPosFromTilePos(new Vector2Int(indexX, indexY) , out Vector3 worldPos);
        mapManager.RemoveEnemyOnTile(
            new Vector2Int(indexX, indexY), this, worldPos);
        isDead = true;
        Destroy(gameObject, 0.3f);
    }

    private IEnumerator AttackDelayed(int damage, AttackType attackType, float delay)
    {
        yield return new WaitForSeconds(delay * TickManager.Instance.calculateBPM());
        SoundManagerIngame.Instance.PlayDialogueSFX(SO.attackSound);
        InvokeTrapAttackEvent(damage, attackType);
    }
    
    public void Attack(int damage, AttackType attackType, float delay)
    {
        StartCoroutine(AttackDelayed(damage, attackType, delay));
    }
    
    public void Stun()
    {
        OnTrapStunEvent?.Invoke(this);
        SoundManagerIngame.Instance.PlayDialogueSFX("SpiderStun");
    }
    
    public EnemySo GetSO()
    {
        return SO;
    }
    
    public void StartListenTick(MovementType movementType)
    {
        TickManager.Instance.SubscribeToMovementEvent(movementType, OnTick, out entityId);
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

    protected virtual void OnStart()
    {
        
    }
    
    void Start()
    {
        Init();
        OnStart();
    }

    protected void InvokeTrapAttackEvent(int damage, AttackType attackType)
    {
        OnTrapAttackEvent?.Invoke(damage, attackType);
    }

    public static void ClearEvent()
    {
        OnTrapAttackEvent = null;
    }

    public void Stunned(TrapData _web)
    {
            isStunned = true;
            web = _web;
    }
}
