using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
        TileData tileData = mapManager.GetTileDataAtPosition(indexX, indexY);
        // yield return tileData.transform.DOScaleX(6f, 0.33f).SetEase(Ease.InElastic).onComplete += () =>
        // {
        //     tileData.transform.DOScaleX(4f, 0.33f).SetEase(Ease.InElastic);
        // };
        // yield return tileData.transform.DOScaleY(6f, 0.33f).SetEase(Ease.InElastic).onComplete += () =>
        // {
        //     tileData.transform.DOScaleY(4f, 0.33f).SetEase(Ease.InElastic);
        // };
        // yield return tileData.transform.DOScaleZ(6f, 0.33f).SetEase(Ease.InElastic).onComplete += () =>
        // {
        //     tileData.transform.DOScaleZ(4f, 0.33f).SetEase(Ease.InElastic);
        // };
        
        tileData.transform.DOShakeScale(0.5f, 0.5f, 10, 90, false);
        
        // tileData.transform.DOScaleY(6f, 0.33f).SetEase(Ease.InElastic);
        // tileData.transform.DOScaleZ(6f, 0.33f).SetEase(Ease.InElastic);
        // yield return new WaitForSeconds(0.4f);
        // tileData.transform.DOScaleX(2f, 0.33f).SetEase(Ease.InElastic);
        // tileData.transform.DOScaleY(2f, 0.33f).SetEase(Ease.InElastic);
        // tileData.transform.DOScaleZ(2f, 0.33f).SetEase(Ease.InElastic);
        // yield return new WaitForSeconds(0.4f);
        // tileData.transform.DOScaleX(4f, 0.33f).SetEase(Ease.InElastic);
        // tileData.transform.DOScaleY(4f, 0.33f).SetEase(Ease.InElastic);
        // tileData.transform.DOScaleZ(4f, 0.33f).SetEase(Ease.InElastic);
        yield return new WaitForSeconds(1f);
        TakeDamage(999, AttackType.Physical);
    }

    protected override void OnTick()
    {
        if (isDead) return;
        mapManager.GetMonstersOnPos(new Vector2Int(indexX, indexY), out List<TrapData> minions);
        if (minions.Count > 0)
        {
            minions[0].Stunned();
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