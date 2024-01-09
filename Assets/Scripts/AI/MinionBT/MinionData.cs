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
    [field: SerializeField] private AttackFX animFX;

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
        if (isStunned)
        {
            isStunned = false;
            return;
        }

        Debug.Log("OnTick");
        bt.getOrigin().Evaluate(bt.getOrigin());
    }

    public void Move(Transform targetTr, Vector3 offset, float delay)
    {
        animQueue.AddAnim(new AnimToQueue(tr, targetTr, offset, false, delay));
        print("Move");
    }

    public override void TakeDamage(int damage, AttackType attackType)
    {
        if (isDead) return;

        minionInstance.CurrentHealthPoint -= damage;
        if (minionInstance.CurrentHealthPoint <= 0)
        {
            OnDead();
        }
    }

    public void Revive()
    {
        if (!isDead) return;
        Init();
        if (!mapManager.AddMinionOnTile(new Vector2Int(indexX, indexY), this, ref indexOffsetTile))
        {
            OnDead();
        }
    }

    protected override void Init()
    {
        isStunned = false;
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
        isDead = true;
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

    public void PlayAttackFX(Transform targetTr, float delay, DirectionToMove direction)
    {
        if (animFX == null) return;
        AttackFX fx = Instantiate(animFX, targetTr.position, animFX.transform.rotation);
        fx.Init(targetTr, transform, delay, direction);
        fx.Launch();
    }
}