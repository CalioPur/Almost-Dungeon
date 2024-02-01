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
    
    private Vector2Int heroPos = new Vector2Int(-9999, -9999);
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
        SO.CreateInstance();
        TickManager.SubscribeToMovementEvent(MovementType.Trap, OnTick, out entityId);
    }
    
    protected override void OnTick()
    {
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
