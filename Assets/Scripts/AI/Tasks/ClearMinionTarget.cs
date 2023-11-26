using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class ClearMinionTarget : Node
{
    private HeroBlackboard blackboard;

    public ClearMinionTarget(HeroBlackboard _blackboard)
    {
        blackboard = _blackboard;
    }

    public override NodeState Evaluate(Node root)
    {
        blackboard.Targets.Clear();
        blackboard.ChosenTarget = null;
        return NodeState.Success;
    }
}
