using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class MoveToHero : Node
{
    private MinionBlackboard blackboard;
    public MoveToHero(MinionBlackboard blackboard)
    {
        this.blackboard = blackboard;
    }
    
    public override NodeState Evaluate(Node root)
    {
        if (blackboard.dir == DirectionToMove.None)
            return NodeState.Success;
        
        blackboard.minionData.Move(blackboard.dir);
        return NodeState.Success;
    }
}
