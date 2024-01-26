using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class CheckIfReadyToUndig : Node
{
    private MinionBlackboard minionBlackboard;
    
    public CheckIfReadyToUndig(MinionBlackboard minionBlackboard)
    {
        this.minionBlackboard = minionBlackboard;
    }

    public override NodeState Evaluate(Node root)
    {
        minionSkeleton minionSkeleton = minionBlackboard.minionData as minionSkeleton;
        if (minionSkeleton == null) return NodeState.Failure;
        return minionSkeleton.isReadyToUndig ? NodeState.Success : NodeState.Failure;
    }
}
