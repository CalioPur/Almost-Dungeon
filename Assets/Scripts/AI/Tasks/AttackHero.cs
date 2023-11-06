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
        this.blackboard=blackboard;
    }

    public override NodeState Evaluate(Node root)
    {
        if(blackboard.heroPosition.x == blackboard.minionData.indexX && blackboard.heroPosition.y == blackboard.minionData.indexY)
        {

            Vector3 heroPosVec3 = Vector3.zero;
            blackboard.minionData.mapManager.GetWorldPosFromTilePos(blackboard.heroPosition.x, blackboard.heroPosition.y, out heroPosVec3, true);
            
            Debug.Log("actual pos: " + blackboard.heroPosition.x + " " + blackboard.heroPosition.y);
            Debug.Log(heroPosVec3);
            
            blackboard.minionData.animQueue.Enqueue(new AnimToQueue(blackboard.minionData.transform, heroPosVec3, 0.3f, Ease.InBack, 2));
            blackboard.minionData.StartCoroutine(blackboard.minionData.doAnim());
            blackboard.minionData.Attack(blackboard.minionData.minionInstance.So.damage);
            return NodeState.Success;
        }
        return NodeState.Failure;
    }
}


