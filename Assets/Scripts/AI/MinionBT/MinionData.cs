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
    [SerializeField] protected EmotesManager emotesManager;
    
    public void GetHeroPos()
    {
        OnHeroPosAsked?.Invoke();
    }
    
    public static void ClearSubscribes()
    {
        OnHeroPosAsked = null;
    }

    public void Move(Transform targetTr, Vector3 offset, float delay)
    {
        // mapManager.GetTilePosFromWorldPos(pos, out int x, out int y);
        // Transform tileTransform = mapManager.GetTileDataAtPosition(x, y).transform;
        //
        // Vector3 tilePos = mapManager.GetTileDataAtPosition(indexX, indexY).transform.position;
        // Vector3 offset = tilePos - transform.position;
        animQueue.AddAnim(new AnimToQueue(tr, targetTr,  offset , false, delay));
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

    protected override void OnDead()
    {
        base.OnDead();
        TickManager.UnsubscribeFromMovementEvent(MovementType.Monster, entityId);
    }

    public void PlayEmote(EmoteType emote)
    {
        emotesManager.PlayEmote(emote);
    }
}