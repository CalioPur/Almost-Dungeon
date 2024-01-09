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
    private Vector2Int heroPos = new Vector2Int(-9999, -9999);

    private void Awake()
    {
        MinionData.OnMinionDying += StockMinions;
    }

    public void StockMinions(MinionData minion)
    {
        MinionDatas.Add(minion);
    }

    IEnumerator ReviveAnimation()
    {
        TickManager.PauseTick(true);
        OnBeginNightFireCamp?.Invoke(1f);
        yield return new WaitForSeconds(2f);
        foreach (var minion in MinionDatas)
        {
            minion.Revive();
        }
        MinionDatas.Clear();
        OnEndNightFireCamp?.Invoke(1);
        yield return new WaitForSeconds(2.0f);
        TickManager.UnsubscribeFromMovementEvent(MovementType.Trap, entityId);
        TickManager.PauseTick(false);
        Destroy(gameObject);
    }
    
    public void Revive()
    {
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
        Hero.OnGivePosBackEvent += GetHeroPosOnTile;
    }

    private void GetHeroPosOnTile(Vector2Int pos)
    {
        heroPos = pos;
    }
    
    protected override void OnTick()
    {
        //je check si le hero est sur ma case        
        if (heroPos.x == indexX && heroPos.y == indexY)
        {
            Revive();
        }
    }

    protected override void OnDead()
    {
        Debug.LogError("Pyke is dead ! is not normal");
        
    }
}
