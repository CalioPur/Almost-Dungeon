using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class ResetDigSystem : Node
{
    private MinionBlackboard minionBlackboard;
    
    public ResetDigSystem(MinionBlackboard minionBlackboard)
    {
        this.minionBlackboard = minionBlackboard;
    }
    public override NodeState Evaluate(Node root)
    {
        minionSkeleton minionSkeleton = minionBlackboard.minionData as minionSkeleton;
        if (minionSkeleton == null) return NodeState.Failure;
        minionSkeleton.isReadyToUndig = false;
        minionSkeleton.isDigger = false;
        return NodeState.Success;
    }
}
