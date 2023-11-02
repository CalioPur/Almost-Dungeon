using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class ChooseTargetToHitOnCaC : Node
{
    private HeroBlackboard blackboard;
    
    public ChooseTargetToHitOnCaC(HeroBlackboard _blackboard)
    {
        blackboard = _blackboard;
    }
    
    public override NodeState Evaluate(Node root)
    {
        if (blackboard.Targets.Count <= 0) return NodeState.Failure;
        if (blackboard.ChosenTarget != null)
        {
            if (blackboard.Targets.Contains(blackboard.ChosenTarget)) return NodeState.Success;
            blackboard.ChosenTarget = blackboard.Targets[0];
            return NodeState.Success;
        }
        blackboard.ChosenTarget = blackboard.Targets[0];
        return NodeState.Success;

    }
}
