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
        return NodeState.Failure;
        blackboard.directionToMove = PathFinding.BFSGoToClosestExit(
            blackboard.hero.GetIndexHeroPos(),
            blackboard.hero.mapManager.getMapArray(), Personnalities.HurryForTheExit);
        Vector2Int simulatedPos = blackboard.hero.GetIndexHeroPos();
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

        if (blackboard.hero.mapManager.CheckIfTileIsFree(simulatedPos)) return NodeState.Success;
        blackboard.hero.OutOfMap();
        return NodeState.Failure;
    }
}