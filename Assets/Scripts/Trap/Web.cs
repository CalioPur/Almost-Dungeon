using System;
using System.Collections.Generic;
using UnityEngine;

public class Web : TrapData
{
    private EnemyInstance webInstance;
    private Vector2Int heroPos = new Vector2Int(-9999, -9999);

    private void Start()
    {
        Init();
    }

    private void OnEnable()
    {
        Hero.OnGivePosBackEvent += GetHeroPosOnTile;
    }

    private void OnDisable()
    {
        Hero.OnGivePosBackEvent -= GetHeroPosOnTile;
    }

    private void GetHeroPosOnTile(Vector2Int pos)
    {
        heroPos = pos;
    }


    public override void TakeDamage(int damage,  AttackType attackType)
    {
        webInstance.CurrentHealthPoint -= damage;
        if (webInstance.CurrentHealthPoint <= 0)
        {
            OnDead();
        }
    }

    protected override void OnTick()
    {
        if (isDead) return;
        mapManager.GetMonstersOnPos(new Vector2Int(indexX, indexY), out List<TrapData> minions);
        if (minions.Count > 0)
        {
           minions[0].Stunned();
           TakeDamage(999, AttackType.Physical);
        }
        else
        {
            if (heroPos.x == indexX && heroPos.y == indexY)
            {
                Stun();
                TakeDamage(999, AttackType.Physical);
            }
        }
    }

    protected override void Init()
    {
        webInstance = SO.CreateInstance();
        TickManager.SubscribeToMovementEvent(MovementType.Trap, OnTick, out entityId);
    }
}