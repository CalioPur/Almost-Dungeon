using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class minionFighter : MinionData
{
    [SerializeField] private minionSOScript minionSO;

    void Start()
    {
        TickManager.SubscribeToMovementEvent(MovementType.Monster, OnTick, entityId);
        Hero.OnGivePosBackEvent += GetHeroPos;
    }

    private void GetHeroPos(Vector2Int pos)
    {
        bt.blackboard.heroPosition = pos;
    }    

    void OnTick()
    {
        if (!bt) return;
        bt.getOrigin().Evaluate(bt.getOrigin());
    }
}
