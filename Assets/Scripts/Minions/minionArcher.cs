using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minionArcher : MinionData
{
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