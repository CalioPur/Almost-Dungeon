using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class minionFighter : MinionData
{
    [SerializeField] private minionSOScript minionSO;

    void Start()
    {
        GameManager.OnBeginToMoveEvent += StartListenTick;
        Hero.OnGivePosBackEvent += GetHeroPos;
    }

    private void GetHeroPos(Vector2Int pos)
    {
        bt.blackboard.heroPosition = pos;
    }

    void StartListenTick()
    {
        TickManager.SubscribeToMovementEvent(MovementType.Monster, OnTick, entityId);
    }

    void OnTick()
    {
        if (!bt) return;
        bt.getOrigin().Evaluate(bt.getOrigin());
    }
}
