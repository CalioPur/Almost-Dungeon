
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pyke : TrapData
{
    
    private EnemyInstance pykeInstance;
    private Vector2Int heroPos = new Vector2Int(-9999, -9999);
    
    public override void TakeDamage(int damage)
    {
        Debug.LogError("Pyke TakeDamage ! is not normal");
    }

    protected override void OnTick()
    {
        //je check si le hero ou les minions sont sur ma case
        mapManager.GetNbMonstersOnPos(new Vector2Int(indexX, indexY), out List<TrapData> minions);
        if (minions.Count > 0)
        {
            foreach (var minion in minions)
            {
                minion.TakeDamage(pykeInstance.damagePoint);
            }
        }
        if (heroPos.x == indexX && heroPos.y == indexY)
        {
            Attack(pykeInstance.damagePoint);
        }
    }

    protected override void Init()
    {
        pykeInstance = SO.CreateInstance();
        TickManager.SubscribeToMovementEvent(MovementType.Trap, OnTick, entityId);
        Hero.OnGivePosBackEvent += GetHeroPosOnTile;
    }

    private void GetHeroPosOnTile(Vector2Int pos)
    {
        heroPos = pos;
    }

    protected override void OnDead()
    {
        Debug.LogError("Pyke is dead ! is not normal");
    }
}
