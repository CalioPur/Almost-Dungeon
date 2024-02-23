using System;
using System.Collections.Generic;
using System.Linq;
using BehaviourTree;
using UnityEngine;

public class CheckDirectionToMove : Node
{
    private HeroBlackboard blackboard;
    static TileData target = null;

    public CheckDirectionToMove(HeroBlackboard _blackboard)
    {
        blackboard = _blackboard;
    }

    public override NodeState Evaluate(Node root)
    {

        SetVisibleTiles();
        
        
        List<TileData> listOfExits = new List<TileData>();
        listOfExits.AddRange(blackboard.visibleTiles.Where(VARIABLE => VARIABLE.isExit));
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

        
        foreach (var VARIABLE in blackboard.visibleTiles)
        {
            if (!listOfExits.Contains(VARIABLE))
            {
                if (blackboard.memory.Contains(VARIABLE))
                {
                    blackboard.memory.Remove(VARIABLE);
                    if (target == VARIABLE)
                    {
                        target = null;
                        Debug.Log("Target has been removed");
                    }
                }
            }
            if (!blackboard.memory.Contains(VARIABLE))
            {
                blackboard.memory.Add(VARIABLE);
            }
            blackboard.options.Add(VARIABLE);
        }
        blackboard.options = GetRidOfOptionsThatHaveNoDoorsToUnvisitedTiles(blackboard.options);
        blackboard.memory = GetRidOfOptionsThatHaveNoDoorsToUnvisitedTiles(blackboard.memory);

        if (target == null)
        {
            Debug.Log("Target is null");
            if (blackboard.options.Count == 0)
            {
                if (blackboard.memory.Count > 0)
                {
                    foreach (var VARIABLE in blackboard.memory)
                    {
                        blackboard.options.Add(VARIABLE);
                    }
                }
            }
            if (blackboard.options.Count > 0 || blackboard.memory.Count > 0)
            {
                if (blackboard.aggressivity == Aggressivity.PEUREUX)
                {
                    target = BFSScript.BSFGoToTile(blackboard.hero.GetIndexHeroPos(),
                        blackboard.options, blackboard.hero.mapManager.getMapArray(), true);
                }
                else if (blackboard.personalities.Contains(Personnalities.EXPLORATEUR) || blackboard.memory == blackboard.options)
                {
                    target = BFSScript.BSFGoToTile(blackboard.hero.GetIndexHeroPos(),
                        blackboard.options, blackboard.hero.mapManager.getMapArray());
                }
                else if (blackboard.options.Count > 0)
                {
                    int random = UnityEngine.Random.Range(0, blackboard.options.Count);
                    target = blackboard.options[random];
                }
                else if (blackboard.memory.Count > 0)
                {
                    target = blackboard.memory[UnityEngine.Random.Range(0, blackboard.memory.Count)];
                }
                else
                {
                    Debug.LogError("MARCHE PO options count : " + blackboard.options.Count + " memory count : " + blackboard.memory.Count);
                }
            }
        }
        
        List<TileData> listOfEnemiesPos = new List<TileData>();
        listOfEnemiesPos.AddRange(from VARIABLE1 in blackboard.visibleTiles where VARIABLE1.enemies.Count > 0 select VARIABLE1);
        bool isAtExit = false;
        foreach (var VARIABLE in currentMap)
        {
            if (VARIABLE.isExit && VARIABLE.IndexInMapArray == blackboard.hero.GetIndexHeroPos() &&
                BFSScript.GoAttackDragon(blackboard.hero.GetIndexHeroPos(), blackboard.hero.mapManager.getMapArray(),
                    out var bsfGoToTile))
            {
                blackboard.directionToMove = bsfGoToTile;
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
                if (blackboard.personalities.Contains(Personnalities.IMPATIENT) &&
                    BFSScript.DistanceFromExit(blackboard.hero.GetIndexHeroPos(),blackboard.hero.mapManager.getMapArray()) > 5) Debug.Log("Raged because of distance");
                else
                {
                }
            }
            else if (blackboard.aggressivity == Aggressivity.COURAGEUX)
            {
                blackboard.directionToMove = BFSScript.BFSGoInDirection(blackboard.hero.GetIndexHeroPos(),
                    listOfEnemiesPos,
                    blackboard.hero.mapManager.getMapArray());
            }
            else if (target != null)
            {
                blackboard.directionToMove = BFSScript.BFSGoInDirection(blackboard.hero.GetIndexHeroPos(),
                    new List<TileData> {target},
                    blackboard.hero.mapManager.getMapArray(), true);
            }
        }
        foreach (var VARIABLE in blackboard.memory)
        {
            Debug.DrawRay(VARIABLE.gameObject.transform.position, Vector3.up, Color.red, 1);
        }

        foreach (var VARIABLE in blackboard.options)
        {
            Debug.DrawRay(VARIABLE.gameObject.transform.position, Vector3.up, Color.green, 1);
        }
        blackboard.options.Clear();
        if (target.isVisited)
        {
            target = null;
        }

        if (blackboard.directionToMove == DirectionToMove.Error)
        {
            Debug.LogError("MARCHE PO");
            // blackboard.directionToMove = RandomDirection();
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
        return NodeState.Failure;
    }

    public void SetVisibleTiles()
    {
        blackboard.visibleTiles = blackboard.visionType switch
        {
            VisionType.BIGLEUX => BlindScript.GetAdjacentTiles(blackboard.hero.GetIndexHeroPos()),
            VisionType.LIGNEDROITE => VisionNormalScript.GetVisibleTiles(blackboard.hero.GetIndexHeroPos()),
            _ => SeerScript.GetAllConnectedToPathTiles(blackboard.hero.GetIndexHeroPos())
        };
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
                    tileDatasToReturn.Add(VARIABLE1);
                }
            }
            if(VARIABLE.isConnectedToPath && !VARIABLE.IsVisited)
            {
                tileDatasToReturn.Add(VARIABLE);
            }
        }
        return tileDatasToReturn.Distinct().ToList();
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