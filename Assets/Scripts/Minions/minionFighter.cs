using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class minionFighter : MinionData
{

    private void GetHeroPos(Vector2Int pos)
    {
        bt.blackboard.heroPosition = pos;
    }


    

    protected override void OnTick()
    {
        if (!bt) return;
        bt.getOrigin().Evaluate(bt.getOrigin());
    }
     

     protected override void Init()
     {
         base.Init();
         Hero.OnGivePosBackEvent += GetHeroPos;
     }

    protected override void OnDead()
    {
        mapManager.RemoveEnemyOnTile(new Vector2Int(indexMinionX, indexMinionY), this);
        Destroy(gameObject);
    }
}
