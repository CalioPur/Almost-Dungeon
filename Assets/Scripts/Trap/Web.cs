using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Web : TrapData
{
    private EnemyInstance webInstance;

    private void Start()
    {
        Init();
    }

    public override void TakeDamage(int damage, AttackType attackType)
    {
        webInstance.CurrentHealthPoint -= damage;
        if (webInstance.CurrentHealthPoint <= 0)
        {
            OnDead();
        }
    }

    IEnumerator FX_Catch()
    {
        isDead = true;
        TileData tileData = mapManager.GetTileDataAtPosition(indexX, indexY);
        tileData.transform.DOShakeScale(0.5f, 0.5f, 10, 90, false);
        yield return new WaitForSeconds(1f);
        isDead = false;
    }

    protected override void OnTick()
    {
        if (isDead) return;
        mapManager.GetMonstersOnPos(new Vector2Int(indexX, indexY), out List<TrapData> minions);
        Vector2Int heroPos = GameManager.Instance.GetHeroPos();
        if (minions.Count > 0)
        {
            minions[0].Stunned(this);
            StartCoroutine(FX_Catch());
        }
        else
        {
            if (heroPos.x == indexX && heroPos.y == indexY)
            {
                Stun();
                StartCoroutine(FX_Catch());
            }
        }
    }

    protected override void Init()
    {
        webInstance = SO.CreateInstance();
        TickManager.SubscribeToMovementEvent(MovementType.Trap, OnTick, out entityId);
    }
}