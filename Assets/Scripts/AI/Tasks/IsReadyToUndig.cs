using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class IsReadyToUndig : Node
{
    private MinionBlackboard minionBlackboard;

    public IsReadyToUndig(MinionBlackboard minionBlackboard)
    {
        this.minionBlackboard = minionBlackboard;
    }

    public override NodeState Evaluate(Node root)
    {
        minionSkeleton minionSkeleton = minionBlackboard.minionData as minionSkeleton;
        if (minionSkeleton == null) return NodeState.Failure;
        minionSkeleton.isReadyToUndig = true;
        return NodeState.Success;
    }
}