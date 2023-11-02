using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class CheckTargetIsAlive : Node
{
    private HeroBlackboard blackboard;
    
    public CheckTargetIsAlive(HeroBlackboard _blackboard)
    {
        blackboard = _blackboard;
    }

    public override NodeState Evaluate(Node root)
    {
        if (blackboard.ChosenTarget == null) return NodeState.Failure;
        if (!blackboard.ChosenTarget.isDead) return NodeState.Success;
        blackboard.ChosenTarget = null;
        return NodeState.Success;

    }
}
