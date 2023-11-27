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
        for (int i = 0; i < blackboard.ChosenTarget.Count; i++)
        {
            if(blackboard.ChosenTarget[i] == null || blackboard.ChosenTarget[i].isDead)
            {
                blackboard.ChosenTarget.Remove(blackboard.ChosenTarget[i]);
                i--;
            }
        }
        
        
        if (blackboard.ChosenTarget.Count == 0) return NodeState.Success;
        blackboard.ChosenTarget = null;
        return NodeState.Success;

    }
}
