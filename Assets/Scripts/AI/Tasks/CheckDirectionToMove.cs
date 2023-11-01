using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;
using UnityEngine.EventSystems;

public class CheckDirectionToMove : Node
{
    private HeroBlackboard blackboard;

    public CheckDirectionToMove(HeroBlackboard _blackboard)
    {
        blackboard = _blackboard;
    }

    public override NodeState Evaluate(Node root)
    {
        blackboard.directionToMove = PathFinding.BFSGoToClosestExit(new Vector2Int(blackboard.hero.indexHeroX, blackboard.hero.indexHeroY), blackboard.hero.mapManager.getMapArray());
        
        return NodeState.Success;
    }
}
