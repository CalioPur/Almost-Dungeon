
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pyke : TrapData
{
    
    private EnemyInstance pykeInstance;
    private Vector2Int heroPos = new Vector2Int(-9999, -9999);
    
    public override void TakeDamage(int damage, AttackType attackType)
    {
        Debug.LogError("Pyke TakeDamage ! is not normal");
    }

    protected override void OnTick()
    {
        //je check si le hero ou les minions sont sur ma case
        mapManager.GetMonstersOnPos(new Vector2Int(indexX, indexY), out List<TrapData> minions);
        if (minions.Count > 0)
        {
            foreach (var minion in minions)
            {
                minion.TakeDamage(pykeInstance.So.damage, AttackType.Fire);
            }
        }
        if (heroPos.x == indexX && heroPos.y == indexY)
        {
            Attack(pykeInstance.So.damage, AttackType.Fire);
        }
    }

    protected override void Init()
    {
        pykeInstance = SO.CreateInstance();
        TickManager.SubscribeToMovementEvent(MovementType.Trap, OnTick, out entityId);
        Hero.OnGivePosBackEvent += GetHeroPosOnTile;
    }

    private void GetHeroPosOnTile(Vector2Int pos)
    {
        heroPos = pos;
    }

    private void OnDisable()
    {
        TickManager.UnsubscribeFromMovementEvent(MovementType.Trap, entityId);
    }

    protected override void OnDead()
    {
        Debug.LogError("Pyke is dead ! is not normal");
        
    }
}
