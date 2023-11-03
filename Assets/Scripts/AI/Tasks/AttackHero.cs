using System;
using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
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
            blackboard.minionData.Attack(blackboard.minionData.minionInstance.So.damage);
            return NodeState.Success;
        }
        return NodeState.Failure;
    }
}


