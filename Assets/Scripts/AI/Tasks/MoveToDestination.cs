using System;
using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class MoveToDestination : Node
{
    private HeroBlackboard blackboard;
    
    public MoveToDestination(HeroBlackboard _blackboard)
    {
        blackboard = _blackboard;
    }
    
    public override NodeState Evaluate(Node root)
    {
        if (blackboard.directionToMove == DirectionToMove.None)
            return NodeState.Failure;
        
        blackboard.hero.Move(blackboard.directionToMove);
        return NodeState.Success;
    }
}
