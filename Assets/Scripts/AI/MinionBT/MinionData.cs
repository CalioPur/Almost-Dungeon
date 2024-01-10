using System;
using UnityEngine;
using UnityEngine.Serialization;

public class MinionData : TrapData, IFlippable
{
    public static event Action OnHeroPosAsked;
    public EnemyInstance minionInstance;
    [HideInInspector] public int indexOffsetTile;

    [SerializeField] protected Transform tr;
    [SerializeField] protected MinionBTBase bt;
    [SerializeField] protected AnimationQueue animQueue;
    [SerializeField] protected EmotesManager emotesManager;
    [SerializeField] protected Animator animator;
    [field: SerializeField] private AttackFX animFX;
    
    private Vector2Int SpawnIndex;

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
        bt.getOrigin().Evaluate(bt.getOrigin());
    }

    public void Move(Transform targetTr, Vector3 offset, float delay)
    {
        animQueue.AddAnim(new AnimToQueue(tr, targetTr, offset, false, delay));
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

    public void Flip()
    {
        animator.SetTrigger("Flip");
    }
}