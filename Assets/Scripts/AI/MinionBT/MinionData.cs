using System;
using UnityEngine;
using UnityEngine.Serialization;

public class MinionData : TrapData
{
    public static event Action OnHeroPosAsked;
    public static event Action<MinionData> OnMinionDying;

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
    
    private void GetHeroPos(Vector2Int pos)
    {
        bt.blackboard.heroPosition = pos;
    }
    
    public static void ClearSubscribes()
    {
        OnHeroPosAsked = null;
    }
    
    protected override void OnTick()
    {
        if (!bt || isDead) return;
        Debug.Log("OnTick");
        bt.getOrigin().Evaluate(bt.getOrigin());
    }

    public void Move(Transform targetTr, Vector3 offset, float delay)
    {
        animQueue.AddAnim(new AnimToQueue(tr, targetTr,  offset , false, delay));
        print("Move");
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

    public void Revive()
    {
        Init();
        mapManager.AddMinionOnTile(new Vector2Int(indexX, indexY), this, out indexOffsetTile);
    }

    protected override void Init()
    {
        bt.blackboard.Reset();
        gameObject.SetActive(true);
        isDead = false;
        minionInstance = SO.CreateInstance();
        if (GameManager.isGameStarted)
            StartListenTick();
        else
            GameManager.OnGameStartEvent += StartListenTick;
        Hero.OnGivePosBackEvent += GetHeroPos;
    }

    public void StartListenTick()
    {
        TickManager.SubscribeToMovementEvent(MovementType.Monster, OnTick, out entityId);
    }

    public void addAnim(AnimToQueue animToQueue)
    {
        animQueue.AddAnim(animToQueue);
    }

    protected override void OnDead()
    {
        base.OnDead();
        OnMinionDying?.Invoke(this);
        TickManager.UnsubscribeFromMovementEvent(MovementType.Monster, entityId);
    }

    public void PlayEmote(EmoteType emote)
    {
        emotesManager.PlayEmote(emote);
    }

    private void OnDisable()
    {
        GameManager.OnGameStartEvent -= StartListenTick;
    }
}