using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minionArcher : MinionData
{
    protected override void OnTick()
    {
        if (!bt) return;
        bt.getOrigin().Evaluate(bt.getOrigin());
    }

    protected override void OnDead()
    {
        mapManager.RemoveEnemyOnTile(new Vector2Int(indexX, indexY), this);
        Destroy(gameObject, 0.25f);
    }
    
    private void GetHeroPos(Vector2Int pos)
    {
        bt.blackboard.heroPosition = pos;
    }
    
    protected override void Init()
    {
        base.Init();
        Hero.OnGivePosBackEvent += GetHeroPos;
    }
}