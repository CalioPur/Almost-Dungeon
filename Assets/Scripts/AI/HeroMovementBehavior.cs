using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class HeroMovementBehavior : Node
{
    HeroBlackboard BB;

    public HeroMovementBehavior(HeroBlackboard blackboard)
    {
        BB = blackboard;
    }

    public override NodeState Evaluate(Node root)
    {
        new Sequence
        (
            //new CheckIfDoorToRemember(BB),
            new Selector
            (
                new Sequence
                (
                    new CheckDirectionToMove(BB),
                    new MoveToDestination(BB),
                    // new DisplayNewDirection(BB),
                    new CheckPlayerOutOfMap(BB)
                )
            )
            
        ).Evaluate(root);
        
        return NodeState.Success;
    }
}