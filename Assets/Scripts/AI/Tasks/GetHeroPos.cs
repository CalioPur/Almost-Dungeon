using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class GetHeroPos : Node
{
    
    private MinionBlackboard blackboard;
    
    
    private Vector2 heroPosition;
    
    public GetHeroPos(MinionBlackboard blackboard)
    {
        this.blackboard = blackboard;
    }
    
    public override NodeState Evaluate(Node root)
    {
        return NodeState.Success;
    }

}
