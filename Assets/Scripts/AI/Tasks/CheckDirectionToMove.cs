using System;
using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using Ink.Parsed;
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
        // blackboard.directionToMove = PathFinding.BFSFindPath(
        //     blackboard.hero.GetIndexHeroPos(),
        //     blackboard.hero.mapManager.getMapArray(), blackboard.personality);
        // blackboard.directionToMove = PathFinding.BFSFindPathV2(
        //     blackboard.hero.GetIndexHeroPos(),
        //     blackboard.hero.mapManager.getMapArray(), new List<PersonnalitiesV2>(), VisionType.RECTILIGNE,
        //     Aggressivity.COURAGEUX, new[]
        //     {
        //         Objectives.SORTIE
        //     });
        blackboard.directionToMove = ALaid1.Line(blackboard.hero.GetIndexHeroPos(), blackboard.hero.mapManager.getMapArray());
        if (blackboard.directionToMove == DirectionToMove.None)
        {
            Debug.Log("Random");
            blackboard.directionToMove = RandomDirection();
        }
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
        blackboard.hero.OutOfMap(blackboard.directionToMove);
        Debug.Log("Direction to move : " + blackboard.directionToMove);
        return NodeState.Failure;
    }
    
    public DirectionToMove RandomDirection()
    {
        int random = UnityEngine.Random.Range(0, 4);
        return random switch
        {
            0 => DirectionToMove.Up,
            1 => DirectionToMove.Down,
            2 => DirectionToMove.Left,
            3 => DirectionToMove.Right,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}