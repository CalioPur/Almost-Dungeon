using System;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class MinionData : TrapData
{
    public static event Action OnHeroPosAsked;

    public EnemyInstance minionInstance;
    [HideInInspector] public int indexOffsetTile;
    
    [SerializeField] protected Transform tr;
    [SerializeField] protected MinionBTBase bt;
    [SerializeField] protected AnimationQueue animQueue;
    
    public void GetHeroPos()
    {
        OnHeroPosAsked?.Invoke();
    }
    
    public static void ClearSubscribes()
    {
        OnHeroPosAsked = null;
    }

    public void Move(Vector3 pos, float delay)
    {
        animQueue.AddAnim(new AnimToQueue(tr, pos + new Vector3(1, 0.1f, 1), false, delay));
    }

    public override void TakeDamage(int damage)
    {
        if (isDead) return;

        minionInstance.CurrentHealthPoint -= damage;
        if (minionInstance.CurrentHealthPoint <= 0)
        {
            isDead = true;
            OnDead();
        }
    }

    protected override void Init()
    {
        minionInstance = SO.CreateInstance();
        if (GameManager.isGameStarted)
            StartListenTick();
        else
            GameManager.OnGameStartEvent += StartListenTick;
    }

    public void StartListenTick()
    {
        TickManager.SubscribeToMovementEvent(MovementType.Monster, OnTick, entityId);
    }

    public void addAnim(AnimToQueue animToQueue)
    {
        animQueue.AddAnim(animToQueue);
    }
}