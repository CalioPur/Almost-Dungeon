using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class ChooseTargetToHit : Node
{
    private HeroBlackboard blackboard;

    public ChooseTargetToHit(HeroBlackboard _blackboard)
    {
        blackboard = _blackboard;
    }

    public override NodeState Evaluate(Node root)
    {
        if (blackboard.Targets.Count <= 0) return NodeState.Failure;
        if (blackboard.ChosenTarget != null && blackboard.Targets.Contains(blackboard.ChosenTarget))
            return NodeState.Success;
        blackboard.ChosenTarget = blackboard.Targets[Random.Range(0, blackboard.Targets.Count)];
        return NodeState.Success;
    }
}