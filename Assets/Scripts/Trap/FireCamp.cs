using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireCamp : TrapData
{
    public static event Action<float> OnBeginNightFireCamp;
    public static event Action<float> OnEndNightFireCamp;
    
    private static List<MinionData> MinionDatas = new ();
    
    private EnemyInstance firecampInstance;
    private bool isReviving = false;

     public static void StockMinions(MinionData minion)
    {
        MinionDatas.Add(minion);
    }
    
    public static void ClearMinions()
    {
        if (MinionDatas == null) MinionDatas = new List<MinionData>();
        MinionDatas.Clear();
    }

    IEnumerator ReviveAnimation()
    {
        TickManager.PauseTick(true);
        OnBeginNightFireCamp?.Invoke(1f);
        yield return new WaitForSeconds(2f);
        foreach (var minion in MinionDatas)
        {
            if (minion == null) continue;
            mapManager.RemoveEnemyOnTile(new Vector2Int(minion.indexX, minion.indexY), minion, minion.transform.position);
           Destroy(minion.gameObject);
        }
        MinionDatas.Clear();
        mapManager.Revive();
        OnEndNightFireCamp?.Invoke(1);
        yield return new WaitForSeconds(2.0f);
        TickManager.PauseTick(false);
    }
    
    public void Revive()
    {
        SoundManagerIngame.Instance.PlayDialogueSFX("CampFireSound");
        StartCoroutine(ReviveAnimation());
    }

    public override void TakeDamage(int damage,  AttackType attackType)
    {
        Debug.LogError("FireCamp TakeDamage ! is not normal");
    }

    protected override void Init()
    {
        firecampInstance = SO.CreateInstance();
        TickManager.SubscribeToMovementEvent(MovementType.Trap, OnTick, out entityId);
    }
    
    protected override void OnTick()
    {
        Vector2Int heroPos = GameManager.Instance.GetHeroPos();
        //je check si le hero est sur ma case        
        if (heroPos.x == indexX && heroPos.y == indexY)
        {
            if (!isReviving)
            {
                isReviving = true;
                Revive();
            }
        }
        else if (isReviving)
        {
            isReviving = false;
        }
    }

    protected override void OnDead()
    {
        Debug.LogError("Pyke is dead ! is not normal");
        
    }
}
