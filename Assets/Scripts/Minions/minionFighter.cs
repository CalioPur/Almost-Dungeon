using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class minionFighter : MinionData
{
    protected override void MinionDie()
    {
        Debug.Log("i'm ded");
        mapManager.RemoveMinionOnTile(new Vector2Int(indexMinionX, indexMinionY), this);
        Destroy(gameObject);
    }

    void Start()
    {
        Init();
        GameManager.OnBeginToMoveEvent += StartListenTick;
        Hero.OnGivePosBackEvent += GetHeroPos;
    }

    private void GetHeroPos(Vector2Int pos)
    {
        bt.blackboard.heroPosition = pos;
    }


    protected override void OnTick()
    {
        if (!bt) return;
        bt.getOrigin().Evaluate(bt.getOrigin());
    }
}
