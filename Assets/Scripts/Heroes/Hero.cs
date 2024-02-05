using Tree = BehaviourTree.Tree;
using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Hero : MonoBehaviour, IFlippable
{
    public static event Action<int, bool> OnTakeDamageEvent;
    public static event Action<int> OnPopUpEvent;
    public static event Action<Hero> OnMovedOnEmptyCardEvent;
    public static event Action<DirectionToMove> OnDragonAttackEvent;
    
    public static Hero Instance;
    
    
    public MapManager mapManager { get; private set; }
    public HeroInstance info { get; private set; }
    
    public AttackType attackType = AttackType.Physical;
    
    [SerializeField] private Transform heroTr;
    [SerializeField] private SpriteRenderer Sprite;
    [SerializeField] private Tree bt;
    [SerializeField] private AnimationQueue animQueue;
    [field:SerializeField] public HeroBlackboard HeroBlackboard { get; private set; }
    [field: SerializeField] public EmotesManager emotesManager;
    [field: SerializeField] private AttackFX animFX;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    
    [SerializeField] private GameObject threeDeeHero;

    private int entityId;
    private Vector2Int IndexHeroPos = new (0, 0);
    public AudioClip[] attackClip;
    private bool isStunned;
    private TrapData web;

    public void Move(Transform targetTr, Vector3 offset, float delay)
    {
        animQueue.AddAnim(new AnimToQueue(heroTr, targetTr,  offset , false, delay));
        animator.SetTrigger("Move");
        GameManager.Instance.UpdateHeroPos(GetIndexHeroPos());
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
        if (isStunned)
        {
            isStunned = false;
            web.TakeDamage(999, AttackType.Physical);
            return;
        }
        bt.getOrigin().Evaluate(bt.getOrigin());
    }
    
    public void Init(HeroInstance instance, int _indexHeroX, int _indexHeroY, MapManager manager)
    {
        IndexHeroPos = new Vector2Int(_indexHeroX, _indexHeroY);
        mapManager = manager;
        entityId = GetHashCode();
        info = instance;
        isStunned = false;

        TrapData.ClearEvent();
        TrapData.OnTrapAttackEvent += TakeDamage;
        TrapData.OnTrapStunEvent += Stun;
        Sprite.sprite = info.So.Img;
        OnPopUpEvent?.Invoke(info.CurrentHealthPoint);
        RageScript.OnNoPathFound += PlayEmoteStuck;
        OnDragonAttackEvent +=AttackDragon;
        UI_Dragon.OnDragonTakeDamageEvent+= PlayAttackClip;
    }

    private void AttackDragon(DirectionToMove obj)
    {
        Vector3 dragonDir = Vector3.zero;
        switch (obj)
        {
            case DirectionToMove.Up:
                dragonDir = Vector3.forward;
                break;
            case DirectionToMove.Down:
                dragonDir = Vector3.back;
                break;
            case DirectionToMove.Left:
                dragonDir = Vector3.left;
                break;
            case DirectionToMove.Right:
                dragonDir = Vector3.right;
                break;
            case DirectionToMove.None:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        GameManager.Instance.AttackPoint.position = transform.position + dragonDir;
        AnimToQueue animToQueue = new AnimToQueue(heroTr, GameManager.Instance.AttackPoint , Vector3.zero, true, 0.5f, Ease.InBack, 2);
        AddAnim(animToQueue);
        PlayAttackFX(GameManager.Instance.AttackPoint, 0.5f, obj);
    }


    private void PlayEmoteStuck()
    {
        emotesManager.PlayEmote(EmoteType.Stuck);
    }
    
    private void OnBeginToMove()
    {
        TickManager.SubscribeToMovementEvent(MovementType.Hero, OnTick, out entityId);
    }

    public void OutOfMap(DirectionToMove blackboardDirectionToMove)
    {
        OnMovedOnEmptyCardEvent?.Invoke(this);
        OnDragonAttackEvent?.Invoke(blackboardDirectionToMove);
    }

    private void OnDestroy()
    {
        TickManager.UnsubscribeFromMovementEvent(MovementType.Hero, gameObject.GetInstanceID());
        OnTakeDamageEvent = null;
        OnPopUpEvent = null;
        OnMovedOnEmptyCardEvent = null;
        OnDragonAttackEvent = null;
        UI_Dragon.OnDragonTakeDamageEvent -= PlayAttackClip;
    }
    
    IEnumerator FXDeath()
    {
        Debug.Log("FX Death");
        Material[] mats = threeDeeHero.GetComponent<MeshRenderer>().materials;
        foreach (var t in mats)
        {
            t.DOFloat(0.6f, "_Level", 2f).SetEase(Ease.InBack);
        }
        yield return new WaitForSeconds(2f);
        FXTakeDamage();
    }
    
    private void IsDead()
    {
        //TODO: t'as gagne bg :*
        Vector3 pos = transform.position;
        pos.y += 3f;
        Camera.main.transform.DOMove(pos, 1f).SetEase(Ease.InBack);
        Camera.main.transform.DORotate(new Vector3(90, 0, 0), 1f).SetEase(Ease.InBack);
        StartCoroutine(FXDeath());
    }


    private void FXTakeDamage()
    {
        // Sprite.DOColor(Color.red, 0.2f).SetEase(Ease.InBack).OnComplete(() =>
        // {
        //     Sprite.DOColor(Color.white, 0.2f).SetEase(Ease.InBack);
        // });
        animator.SetTrigger("TakeDamage");
        OnTakeDamageEvent?.Invoke(info.CurrentHealthPoint, true);
    }

    public void TakeDamage(int soAttackPoint, AttackType attackType)
    {
        if (info.CurrentHealthPoint - soAttackPoint <= 0)
        {
            Debug.Log("Hero is dead");
            info.CurrentHealthPoint = 0;
            FXTakeDamage();
            IsDead();
            TickManager.OnEndGame();
        }
        else
        {
            info.CurrentHealthPoint -= soAttackPoint;
            Debug.Log("Hero take damage and now have " + info.CurrentHealthPoint + " hp");
            FXTakeDamage();
        }
        
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        OnBeginToMove();
    }

    private void OnDisable()
    {
        TrapData.OnTrapAttackEvent -= TakeDamage;
        TrapData.OnTrapStunEvent -= Stun;
        TrapData.ClearEvent();
        MinionData.ClearSubscribes();
        RageScript.OnNoPathFound -= PlayEmoteStuck;
    }

    private void Stun(TrapData _web)
    {
        isStunned = true;
        web = _web;
    }

    public void AddAnim(AnimToQueue animToQueue)
    {
        animQueue.AddAnim(animToQueue);
    }
    
    
    public void PlayAttackFX(Transform targetTr, float delay, DirectionToMove direction)
    {
        if (animFX == null) return;
        AttackFX fx = Instantiate(animFX, targetTr.position, animFX.transform.rotation);
        fx.Init(targetTr, transform, delay, direction);
        fx.Launch();
    }

    
    public void PlayAttackClip()
    {
        audioSource.PlayOneShot(attackClip[Random.Range(0,attackClip.Length)]);
    }

    public void Flip()
    {
        animator.SetTrigger("Flip");
    }
}