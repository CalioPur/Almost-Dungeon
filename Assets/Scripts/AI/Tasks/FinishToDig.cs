using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class FinishToDig : Node
{
    private MinionBlackboard minionBlackboard;
    
    public FinishToDig(MinionBlackboard minionBlackboard)
    {
        this.minionBlackboard = minionBlackboard;
    }

    public override NodeState Evaluate(Node root)
    {
        minionSkeleton minionSkeleton = minionBlackboard.minionData as minionSkeleton;
        if (minionSkeleton == null) return NodeState.Failure;
        minionSkeleton.FinishToDig();
        return NodeState.Success;
    }
}
