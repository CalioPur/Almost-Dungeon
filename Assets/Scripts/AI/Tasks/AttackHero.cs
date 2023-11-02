using System;
using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class AttackHero : Node
{
    MinionBlackboard blackboard;
    public static event Action<int> OnMonsterAttackEvent;
    public AttackHero(MinionBlackboard blackboard)
    {
        this.blackboard=blackboard;
    }

    public override NodeState Evaluate(Node root)
    {
        if(blackboard.heroPosition.x == blackboard.minionData.indexMinionX && blackboard.heroPosition.y == blackboard.minionData.indexMinionY)
        {
            OnMonsterAttackEvent?.Invoke(blackboard.minionData.minionInstance.damagePoint);
            return NodeState.Success;
        }
        return NodeState.Failure;
    }
}


