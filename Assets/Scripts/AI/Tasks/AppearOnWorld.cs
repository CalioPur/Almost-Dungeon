using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class AppearOnWorld : Node
{
    private MinionBlackboard blackboard;
    
    public AppearOnWorld(MinionBlackboard _blackboard)
    {
        blackboard = _blackboard;
    }
    
    public override NodeState Evaluate(Node root)
    {
        blackboard.minionData.ShowSprite();
        blackboard.minionData.AddOnTile(blackboard.minionData.indexX, blackboard.minionData.indexY);
        return NodeState.Success;
    }
}
