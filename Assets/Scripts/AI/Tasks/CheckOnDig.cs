using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class CheckOnDig : Node
{
    private MinionBlackboard minionBlackboard;
    
    public CheckOnDig(MinionBlackboard minionBlackboard)
    {
        this.minionBlackboard = minionBlackboard;
    }
    
    public override NodeState Evaluate(Node root)
    {
        minionSkeleton minionSkeleton = minionBlackboard.minionData as minionSkeleton;
        if (minionSkeleton == null) return NodeState.Failure;
        if (minionSkeleton.isDigger) return NodeState.Success;
        return NodeState.Failure;
    }
}
