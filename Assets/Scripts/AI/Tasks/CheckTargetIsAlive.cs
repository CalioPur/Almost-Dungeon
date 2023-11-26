using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        foreach (var VARIABLE in blackboard.ChosenTarget.Where(VARIABLE => VARIABLE.isDead))
        {
            blackboard.ChosenTarget.Remove(VARIABLE);
        }
        
        if (blackboard.ChosenTarget.Count == 0) return NodeState.Success;
        blackboard.ChosenTarget = null;
        return NodeState.Success;

    }
}
