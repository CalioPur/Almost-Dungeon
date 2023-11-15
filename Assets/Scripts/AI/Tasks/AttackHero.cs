using System;
using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using DG.Tweening;
using UnityEngine;


public class AttackHero : Node
{
    MinionBlackboard blackboard;

    public AttackHero(MinionBlackboard blackboard)
    {
        this.blackboard = blackboard;
    }

    public override NodeState Evaluate(Node root)
    {
            Vector3 heroPosVec3 = Vector3.zero;
            blackboard.minionData.mapManager.GetWorldPosFromTilePos(
                new Vector2Int(blackboard.heroPosition.x + 1, blackboard.heroPosition.y + 1), out heroPosVec3);
            blackboard.minionData.addAnim(new AnimToQueue(blackboard.minionData.transform, heroPosVec3, 0.3f,
                Ease.InBack, 2));
            blackboard.minionData.Attack(blackboard.minionData.minionInstance.So.damage);
            return NodeState.Success;
    }
}