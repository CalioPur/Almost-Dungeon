using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BehaviourTree;
using Ink.Parsed;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.WSA;

public class CheckDirectionToMove : Node
{
    private HeroBlackboard blackboard;
    List<TileData> memory = new();
    List<TileData> options = new();

    public CheckDirectionToMove(HeroBlackboard _blackboard)
    {
        blackboard = _blackboard;
    }

    public override NodeState Evaluate(Node root)
    {

        var visibleTiles = blackboard.visionType switch
        {
            VisionType.BIGLEUX => BlindScript.GetAdjacentTiles(blackboard.hero.GetIndexHeroPos()),
            VisionType.LIGNEDROITE => VisionNormalScript.GetVisibleTiles(blackboard.hero.GetIndexHeroPos()),
            _ => SeerScript.GetAllConnectedToPathTiles(blackboard.hero.GetIndexHeroPos())
        };
        
        
        List<TileData> listOfExits = new List<TileData>();
        listOfExits.AddRange(visibleTiles.Where(VARIABLE => VARIABLE.isExit));
        var currentMap = MapManager.Instance.mapArray;
        foreach (var VARIABLE in currentMap)
        {
            if (VARIABLE.isConnectedToPath && !VARIABLE.IsVisited)
            {
                int[,] directions = { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };

                for (int i = 0; i < directions.GetLength(0); i++)
                {
                    var i1 = VARIABLE.IndexInMapArray.x;
                    var i2 = VARIABLE.IndexInMapArray.y;
                    int newX = i1 + directions[i, 0];
                    int newY = i2 + directions[i, 1];

                    if (newX >= 0 && newX < currentMap.GetLength(0) && newY >= 0 && newY < currentMap.GetLength(1))
                    {
                        if ((i == 0 && !MapManager.Instance.GetTileDataAtPosition(i1, i2).hasDoorUp) ||
                            (i == 1 && !MapManager.Instance.GetTileDataAtPosition(i1, i2).hasDoorDown) ||
                            (i == 2 && !MapManager.Instance.GetTileDataAtPosition(i1, i2).hasDoorRight) ||
                            (i == 3 && !MapManager.Instance.GetTileDataAtPosition(i1, i2).hasDoorLeft))
                            listOfExits.Add(VARIABLE);
                    }
                }
            }
        }

        TileData target = null;
        
        foreach (var VARIABLE in visibleTiles)
        {
            if (!listOfExits.Contains(VARIABLE))
            {
                if (memory.Contains(VARIABLE))
                {
                    memory.Remove(VARIABLE);
                    if (target == VARIABLE)
                    {
                        target = null;
                    }
                }
            }
            if (!memory.Contains(VARIABLE))
            {
                memory.Add(VARIABLE);
            }
            options.Add(VARIABLE);
        }
        
        foreach (var VARIABLE in options)
        {
            Debug.DrawRay(VARIABLE.transform.position, Vector3.up * 3, Color.green, 1f);
        }

        if (target == null)
        {
            if (options.Count == 0)
            {
                if (memory.Count > 0)
                {
                    foreach (var VARIABLE in memory)
                    {
                        options.Add(VARIABLE);
                    }
                }
            }
            if (options.Count > 0 || memory.Count > 0)
            {
                if (blackboard.aggressivity == Aggressivity.PEUREUX)
                {
                    target = BFSScript.BSFGoToTile(blackboard.hero.GetIndexHeroPos(),
                        options, blackboard.hero.mapManager.getMapArray(), true);
                }
                else if (blackboard.personalities.Contains(Personnalities.EXPLORATEUR) || memory == options)
                {
                    target = BFSScript.BSFGoToTile(blackboard.hero.GetIndexHeroPos(),
                        options, blackboard.hero.mapManager.getMapArray());
                }
                else if (options.Count > 0)
                {
                    options = GetRidOfOptionsThatHaveNoDoorsToUnvisitedTiles(options);
                    int random = UnityEngine.Random.Range(0, options.Count);
                    Debug.Log("Random : " + random);
                    target = options[random];
                }
                else if (memory.Count > 0)
                {
                    var tempMemory = GetRidOfOptionsThatHaveNoDoorsToUnvisitedTiles(memory);
                    target = tempMemory[UnityEngine.Random.Range(0, tempMemory.Count)];
                }
                else
                {
                    Debug.LogError("MARCHE PO👺👺👺👺👺👺");
                }
            }
        }

        List<TileData> listOfEnemiesPos = new List<TileData>();
        listOfEnemiesPos.AddRange(from VARIABLE1 in visibleTiles where VARIABLE1.enemies.Count > 0 select VARIABLE1);
        bool isAtExit = false;
        foreach (var VARIABLE in currentMap)
        {
            if (VARIABLE.isExit && VARIABLE.IndexInMapArray == blackboard.hero.GetIndexHeroPos() &&
                BFSScript.GoAttackDragon(blackboard.hero.GetIndexHeroPos(), blackboard.hero.mapManager.getMapArray(),
                    out var bsfGoToTile))
            {
                blackboard.directionToMove = bsfGoToTile;
                Debug.Log("Attack dragon");
                isAtExit = true;
            }
        }
        if (!isAtExit)
        {
            if (blackboard.personalities.Contains(Personnalities.IMPATIENT) &&
                BFSScript.DistanceFromExit(blackboard.hero.GetIndexHeroPos(),
                    blackboard.hero.mapManager.getMapArray()) >
                5 || target == null)
            {
                RageScript.Rage(blackboard.hero.GetIndexHeroPos());
                blackboard.directionToMove = DirectionToMove.None;
                Debug.Log("Rage");
            }
            else if (blackboard.aggressivity == Aggressivity.COURAGEUX)
            {
                blackboard.directionToMove = BFSScript.BFSGoInDirection(blackboard.hero.GetIndexHeroPos(),
                    listOfEnemiesPos,
                    blackboard.hero.mapManager.getMapArray());
                Debug.Log("Go to enemy");
            }
            else if (target != null)
            {
                Debug.DrawRay(target.transform.position, Vector3.up * 3, Color.red, 1f);
                blackboard.directionToMove = BFSScript.BFSGoInDirection(blackboard.hero.GetIndexHeroPos(),
                    new List<TileData> {target},
                    blackboard.hero.mapManager.getMapArray(), true);
                Debug.Log("Go to target");
            }
        }
        options.Clear();

        if (blackboard.directionToMove == DirectionToMove.Error)
        {
            Debug.LogError("MARCHE PO👺👺👺👺👺👺");
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
        // Debug.Log("Direction to move : " + blackboard.directionToMove);
        return NodeState.Failure;
    }

    private List<TileData> GetRidOfOptionsThatHaveNoDoorsToUnvisitedTiles(List<TileData> tileDatas)
    {
        List<TileData> tileDatasToReturn = new List<TileData>();
        foreach (var VARIABLE in tileDatas)
        {
            if (VARIABLE.isConnectedToPath && VARIABLE.IsVisited)
            {
                var tmp = BFSScript.GetNeighborsTiles(VARIABLE.IndexInMapArray, blackboard.hero.mapManager.getMapArray());
                foreach (var VARIABLE1 in tmp)
                {
                    if (!tileDatasToReturn.Contains(VARIABLE1))
                    {
                        tileDatasToReturn.Add(VARIABLE1);
                    }
                }
            }
        }
        tileDatasToReturn = tileDatas.Distinct().ToList();
        return tileDatasToReturn;
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