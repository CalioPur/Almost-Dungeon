using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Web : TrapData
{
    private EnemyInstance webInstance;
    private bool stunned = false;

    private void Start()
    {
        Init();
    }

    public override void TakeDamage(int damage, AttackType attackType)
    {
        return;
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
        if (minions.Count > 0)
        {
            if (stunned) return;
            stunned = true;
            minions[0].Stunned(this);
            StartCoroutine(FX_Catch());
            return;
        }

        Vector2Int heroPos = GameManager.Instance.GetHeroPos();
        if (heroPos.x == indexX && heroPos.y == indexY)
        {
            if (stunned) return;
            stunned = true;
            Stun();
            StartCoroutine(FX_Catch());
            return;
        }

        stunned = false;
    }

    protected override void Init()
    {
        webInstance = SO.CreateInstance();
        TickManager.Instance.SubscribeToMovementEvent(MovementType.Trap, OnTick, out entityId);
    }
}