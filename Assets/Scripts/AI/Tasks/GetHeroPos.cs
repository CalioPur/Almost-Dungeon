using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class GetHeroPos : Node
{
    
    private MinionBlackboard blackboard;
    
    
    public GetHeroPos(MinionBlackboard blackboard)
    {
        this.blackboard = blackboard;
    }
    
    public override NodeState Evaluate(Node root)
    {
        blackboard.GetHeroPos();
        return NodeState.Success;
    }

}
