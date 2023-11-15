using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class MinionData : TrapData
{
    public static event Action OnHeroPosAsked;

    [SerializeField] protected Transform tr;
    [SerializeField] protected MinionBTBase bt;
    [SerializeField] protected AnimationQueue animQueue;
    private bool isExec = false;

    public EnemyInstance minionInstance;

    public void GetHeroPos()
    {
        OnHeroPosAsked?.Invoke();
    }

    public void Move(Vector3 pos, float delay)
    {
        animQueue.AddAnim(new AnimToQueue(tr, pos + new Vector3(1, 0.1f, 1), delay));
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