using Tree = BehaviourTree.Tree;
using System;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public static event Action<Vector2Int> OnGivePosBackEvent;
    public static event Action<int, bool> OnTakeDamageEvent;
    public static event Action<int> OnPopUpEvent;
    public static event Action<Hero> OnMovedOnEmptyCardEvent;
    
    
    public MapManager mapManager { get; private set; }
    public HeroInstance info { get; private set; }
    
    [SerializeField] private Transform heroTr;
    [SerializeField] private SpriteRenderer Sprite;
    [SerializeField] private Tree bt;
    [SerializeField] private AnimationQueue animQueue;
    [field:SerializeField] public HeroBlackboard HeroBlackboard { get; private set; }
    [field: SerializeField] private EmotesManager emotesManager;
    
    private int entityId;
    private Vector2Int IndexHeroPos = new (0, 0);

    public void Move(Transform targetTr, Vector3 offset, float delay)
    {
        // mapManager.GetTilePosFromWorldPos(pos, out int x, out int y);
        // Transform tileTransform = mapManager.GetTileDataAtPosition(x, y).transform;
        //
        // Vector3 tilePos = mapManager.GetTileDataAtPosition(indexX, indexY).transform.position;
        // Vector3 offset = tilePos - transform.position;
        animQueue.AddAnim(new AnimToQueue(heroTr, targetTr,  offset , false, delay));
        
        GivePosBack();
    }
    
    public Vector2Int GetIndexHeroPos()
    {
        return IndexHeroPos;
    }
    
    public void AddIndexX(int x)
    {
        IndexHeroPos.x += x;
    }
    
    public void AddIndexY(int y)
    {
        IndexHeroPos.y += y;
    }

    void OnTick()
    {
        if (!bt) return;
        bt.getOrigin().Evaluate(bt.getOrigin());
    }
    
    public void Init(HeroInstance instance, int _indexHeroX, int _indexHeroY, MapManager manager)
    {
        IndexHeroPos = new Vector2Int(_indexHeroX, _indexHeroY);
        mapManager = manager;
        entityId = GetHashCode();
        info = instance;

        TrapData.OnTrapAttackEvent += TakeDamage;
        Sprite.sprite = info.So.Img;
        OnPopUpEvent?.Invoke(info.CurrentHealthPoint);
        MinionData.OnHeroPosAsked+= GivePosBack;
        PathFinding.OnNoPathFound += PlayEmoteStuck;
    }

    private void PlayEmoteStuck()
    {
        emotesManager.PlayEmote(EmoteType.Stuck);
    }

    private void GivePosBack()
    {
        OnGivePosBackEvent?.Invoke(IndexHeroPos);
    }
    private void OnBeginToMove()
    {
        TickManager.SubscribeToMovementEvent(MovementType.Hero, OnTick, out entityId);
    }

    public void OutOfMap()
    {
        OnMovedOnEmptyCardEvent?.Invoke(this);
    }

    private void OnDestroy()
    {
        TickManager.UnsubscribeFromMovementEvent(MovementType.Hero, gameObject.GetInstanceID());
        OnGivePosBackEvent = null;
        OnTakeDamageEvent = null;
        OnPopUpEvent = null;
        OnMovedOnEmptyCardEvent = null;
    }

    private void IsDead()
    {
        //TODO: t'as gagne bg :*
    }

    public void TakeDamage(int soAttackPoint)
    {
        info.CurrentHealthPoint -= soAttackPoint;
        OnTakeDamageEvent?.Invoke(info.CurrentHealthPoint, true);
        if (info.CurrentHealthPoint <= 0)
        {
            IsDead();
        }
    }

    private void Start()
    {
        OnBeginToMove();
    }

    private void OnDisable()
    {
        TrapData.OnTrapAttackEvent -= TakeDamage;
        TrapData.ClearEvent();
        MinionData.OnHeroPosAsked -= GivePosBack;
        MinionData.ClearSubscribes();
        PathFinding.OnNoPathFound -= PlayEmoteStuck;
    }

    public void AddAnim(AnimToQueue animToQueue)
    {
        animQueue.AddAnim(animToQueue);
    }
}