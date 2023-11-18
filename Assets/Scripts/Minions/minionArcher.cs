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