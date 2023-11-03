using System;
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
        blackboard.directionToMove = PathFinding.BFSGoToClosestExit(new Vector2Int(blackboard.hero.indexHeroX, blackboard.hero.indexHeroY), blackboard.hero.mapManager.getMapArray(),Personnalities.HurryForTheExit);
        Vector2Int simulatedPos = new Vector2Int(blackboard.hero.indexHeroX, blackboard.hero.indexHeroY);
        switch (blackboard.directionToMove)
        {
            case DirectionToMove.Up:
                simulatedPos.y += 1;
                break;
            case DirectionToMove.Down:
                simulatedPos.y -= 1;
                break;
            case DirectionToMove.Left:
                simulatedPos.x -= 1;
                break;
            case DirectionToMove.Right:
                simulatedPos.x += 1;
                break;
            case DirectionToMove.None:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        if (blackboard.hero.mapManager.CheckIfTileIsFree(simulatedPos.x, simulatedPos.y)) return NodeState.Success;
        return blackboard.hero.mapManager.CheckDragonHP(blackboard.hero) ? NodeState.Failure : NodeState.Success;
    }
}
