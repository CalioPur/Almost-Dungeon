using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        
        // if (blackboard.aggressivity == Aggressivity.COURAGEUX)
        //     ALaid1.aggressivity = Aggressivity.COURAGEUX;
        // else if (blackboard.aggressivity == Aggressivity.PEUREUX)
        //     ALaid1.aggressivity = Aggressivity.PEUREUX;
        // // Debug.Log("Vision type = " + blackboard.visionType);
        // if (blackboard.visionType == VisionType.BIGLEUX)
        //     blackboard.directionToMove = ALaid1.Bigleux(blackboard.hero.GetIndexHeroPos(), blackboard.hero.mapManager.getMapArray());
        // if (blackboard.visionType == VisionType.LIGNEDROITE)
        //     blackboard.directionToMove = ALaid1.Line(blackboard.hero.GetIndexHeroPos(), blackboard.hero.mapManager.getMapArray());
        // else if (blackboard.visionType == VisionType.CLAIRVOYANT)
        //     blackboard.directionToMove = ALaid1.Clairvoyant(blackboard.hero.GetIndexHeroPos(), blackboard.hero.mapManager.getMapArray());

        var visibleTiles = blackboard.visionType switch
        {
            VisionType.BIGLEUX => BlindScript.GetAdjacentTiles(blackboard.hero.GetIndexHeroPos()),
            VisionType.LIGNEDROITE => VisionNormalScript.GetVisibleTiles(blackboard.hero.GetIndexHeroPos()),
            _ => SeerScript.GetAllConnectedToPathTiles(blackboard.hero.GetIndexHeroPos())
        };

        foreach (var VARIABLE in visibleTiles)
        {
           Debug.DrawLine(VARIABLE.transform.position, VARIABLE.transform.position + Vector3.up * 3.0f, Color.green, 0.5f);
        }
        
        List<Vector2Int> listOfEnemiesPos = new List<Vector2Int>();
        listOfEnemiesPos.AddRange(from VARIABLE in visibleTiles where VARIABLE.enemies.Count > 0 select VARIABLE.IndexInMapArray);
        if(blackboard.aggressivity == Aggressivity.COURAGEUX)
        {
            blackboard.directionToMove = BFSScript.BSFGoToTile(blackboard.hero.GetIndexHeroPos(), listOfEnemiesPos, blackboard.hero.mapManager.getMapArray());
        }
        
        // List<Vector2Int> listOfConnectedButUnvisitedTiles = new List<Vector2Int>();
        // List<Vector2Int> listOfExits = new List<Vector2Int>();
        // listOfConnectedButUnvisitedTiles.AddRange(from VARIABLE in visibleTiles where VARIABLE.isConnectedToPath && !VARIABLE.IsVisited select VARIABLE.IndexInMapArray);
        // listOfExits.AddRange(from VARIABLE in visibleTiles where VARIABLE.isExit select VARIABLE.IndexInMapArray);
        // if (listOfConnectedButUnvisitedTiles.Count > 0)
        // {
        //     Debug.Log("Go to unvisited");
        //     blackboard.directionToMove = blackboard.aggressivity == Aggressivity.PEUREUX ? BFSScript.BSFGoToTile(blackboard.hero.GetIndexHeroPos(), listOfConnectedButUnvisitedTiles, blackboard.hero.mapManager.getMapArray(),true) : BFSScript.BSFGoToTile(blackboard.hero.GetIndexHeroPos(), listOfConnectedButUnvisitedTiles, blackboard.hero.mapManager.getMapArray());
        // }
        // else if(listOfExits.Count > 0)
        // {
        //     Debug.Log("Go to exit");
        //     blackboard.directionToMove = blackboard.aggressivity == Aggressivity.PEUREUX ? BFSScript.BSFGoToTile(blackboard.hero.GetIndexHeroPos(), listOfExits, blackboard.hero.mapManager.getMapArray(),true) : BFSScript.BSFGoToTile(blackboard.hero.GetIndexHeroPos(), listOfExits, blackboard.hero.mapManager.getMapArray());
        // }
        // else
        // {
        //     Debug.Log("BreakWall");
        //     PathFinding.BreakFreeFromNoExit(blackboard.hero.GetIndexHeroPos(), blackboard.hero.mapManager.getMapArray());
        // } JE GEMIKNFGALKBENFAKJBYHAGKEJUBIULFAH AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA

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
                return NodeState.Failure;
        }

        if (blackboard.hero.mapManager.CheckIfTileIsFree(simulatedPos)) return NodeState.Success;
        blackboard.hero.OutOfMap(blackboard.directionToMove);
        // Debug.Log("Direction to move : " + blackboard.directionToMove);
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