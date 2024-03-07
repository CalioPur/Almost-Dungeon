using System;
using System.Collections.Generic;
using System.Linq;
using BehaviourTree;
using Unity.VisualScripting;
using UnityEngine;

public class CheckDirectionToMove : Node
{
    private HeroBlackboard blackboard;
    private static TileData target = null;

    public CheckDirectionToMove(HeroBlackboard _blackboard)
    {
        blackboard = _blackboard;
    }

    public override NodeState Evaluate(Node root)
    {

        SetVisibleTiles();
        
        //observe ???
        
        HashSet<TileData> listOfExits = new HashSet<TileData>();
        //listOfExits.AddRange(blackboard.visibleTiles.Where(VARIABLE => VARIABLE.isExit));
        var currentMap = MapManager.Instance.mapArray;
        foreach (var tile in currentMap)
        {
            if (tile.isConnectedToPath && tile.IsVisited)
            {
                int[,] directions = { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };

                for (int i = 0; i < directions.GetLength(0); i++)
                {
                    var i1 = tile.IndexInMapArray.x;
                    var i2 = tile.IndexInMapArray.y;
                    int newX = i1 + directions[i, 0];
                    int newY = i2 + directions[i, 1];
                    
                    if(i==0 && tile.hasDoorUp && (!currentMap[newX,newY].isVisited || !currentMap[newX,newY].isConnectedToPath)) listOfExits.Add(tile);
                    if(i==1 && tile.hasDoorDown && (!currentMap[newX,newY].isVisited || !currentMap[newX,newY].isConnectedToPath)) listOfExits.Add(tile);
                    if(i==2 && tile.hasDoorRight && (!currentMap[newX,newY].isVisited || !currentMap[newX,newY].isConnectedToPath)) listOfExits.Add(tile);
                    if(i==3 && tile.hasDoorLeft && (!currentMap[newX,newY].isVisited || !currentMap[newX,newY].isConnectedToPath)) listOfExits.Add(tile);
                    
                }
            }
        }

        
        // foreach (var VARIABLE in blackboard.visibleTiles)
        // {
        //     // if (!listOfExits.Contains(VARIABLE))
        //     // {
        //     //     if (blackboard.memory.Contains(VARIABLE))
        //     //     {
        //     //         blackboard.memory.Remove(VARIABLE);
        //     //         if (target == VARIABLE)
        //     //         {
        //     //             target = null;
        //     //             
        //     //         }
        //     //     }
        //     // }
        //     if (blackboard.hero.GetIndexHeroPos() == target?.IndexInMapArray)
        //     {
        //         target = null;
        //     }
        //     if (!blackboard.memory.Contains(VARIABLE))
        //     {
        //         blackboard.memory.Add(VARIABLE);
        //     }
        //     blackboard.options.Add(VARIABLE);
        // }
        // blackboard.options = GetRidOfOptionsThatHaveNoDoorsToUnvisitedTiles(blackboard.options);
        // blackboard.memory = GetRidOfOptionsThatHaveNoDoorsToUnvisitedTiles(blackboard.memory);
        if (blackboard.hero.GetIndexHeroPos() == target?.IndexInMapArray)
        {
            target = null;
        }
        //
        blackboard.options.Clear();
        foreach (var VARIABLE in listOfExits)
        {
            blackboard.options.Add(VARIABLE);
        }
        //
        
        // CHOOSE
        if (target == null)
        {
            // if (blackboard.options.Count == 0)
            // {
            //     if (blackboard.memory.Count > 0)
            //     {
            //         foreach (var VARIABLE in blackboard.memory)
            //         {
            //             blackboard.options.Add(VARIABLE);
            //         }
            //     }
            // }
            
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
        
        
        
        //ACT

        foreach (var exit in listOfExits)
        {
            Debug.Log(exit.IndexInMapArray);
            Debug.DrawRay(exit.transform.position, Vector3.up*2, Color.blue, 1);
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
            if ((blackboard.personalities.Contains(Personnalities.IMPATIENT) &&
                BFSScript.DistanceFromExit(blackboard.hero.GetIndexHeroPos(),
                    blackboard.hero.mapManager.getMapArray()) >
                5) || target == null)
            {
                RageScript.Rage(blackboard.hero.GetIndexHeroPos());
                blackboard.directionToMove = DirectionToMove.None;
            }
            else if (!EnemiesOnRange(listOfEnemiesPos) && blackboard.aggressivity == Aggressivity.COURAGEUX && listOfEnemiesPos.Count > 0)
            {
                blackboard.directionToMove = BFSScript.BFSGoInDirection(blackboard.hero.GetIndexHeroPos(),
                    listOfEnemiesPos,
                    blackboard.hero.mapManager.getMapArray());
            }
            else if (EnemiesOnRange(listOfEnemiesPos) && listOfEnemiesPos.Count > 0 &&
                                               (blackboard.aggressivity ==
                                                  Aggressivity.COURAGEUX || MonsterOnPathToTarget(listOfEnemiesPos)))
            {
               //ca sert a rien pour l'instant mais la il faut attaquer l'ennemi le plus proche #refacto le BT
               return NodeState.Failure;
            }
            else
            {
                blackboard.directionToMove = BFSScript.BFSGoInDirection(blackboard.hero.GetIndexHeroPos(),
                    new List<TileData> {target},
                    blackboard.hero.mapManager.getMapArray());
            }
        }
        blackboard.options.Clear();
        return NodeState.Success;
        // foreach (var VARIABLE in blackboard.memory)
        // {
        //     Debug.DrawRay(VARIABLE.gameObject.transform.position, Vector3.up, Color.red, 1);
        // }
        //
        // foreach (var VARIABLE in blackboard.options)
        // {
        //     Debug.DrawRay(VARIABLE.gameObject.transform.position, Vector3.up, Color.green, 1);
        // }
        // blackboard.options.Clear();
        // if (target != null && target.isVisited)
        // {
        //     target = null;
        // }
        //
        // if (blackboard.directionToMove == DirectionToMove.Error)
        // {
        //     Debug.LogError("MARCHE PO");
        //     // blackboard.directionToMove = RandomDirection();
        // }
        // Vector2Int simulatedPos = blackboard.hero.GetIndexHeroPos();
        // switch (blackboard.directionToMove)
        // {
        //     case DirectionToMove.Up:
        //         simulatedPos.y += 1;
        //         break;
        //     case DirectionToMove.Down:
        //         simulatedPos.y -= 1;
        //         break;
        //     case DirectionToMove.Left:
        //         simulatedPos.x -= 1;
        //         break;
        //     case DirectionToMove.Right:
        //         simulatedPos.x += 1;
        //         break;
        //     case DirectionToMove.None:
        //         
        //         break;
        //     default:
        //         return NodeState.Failure;
        // }
        // if (blackboard.hero.mapManager.CheckIfTileIsFree(simulatedPos)) return NodeState.Success;
        // blackboard.hero.OutOfMap(blackboard.directionToMove);
        // return NodeState.Failure;
    }
    
    private bool EnemiesOnRange(List<TileData> listOfEnemiesPos)
    {
        foreach (var VARIABLE in listOfEnemiesPos)
        {
            if (VARIABLE.enemies.Count > 0)
            {
                if ((VARIABLE.IndexInMapArray == blackboard.hero.GetIndexHeroPos() - Vector2Int.up && VARIABLE.hasDoorUp) ||
                    (VARIABLE.IndexInMapArray == blackboard.hero.GetIndexHeroPos() - Vector2Int.down && VARIABLE.hasDoorDown) ||
                    (VARIABLE.IndexInMapArray == blackboard.hero.GetIndexHeroPos() - Vector2Int.left && VARIABLE.hasDoorLeft) ||
                    (VARIABLE.IndexInMapArray == blackboard.hero.GetIndexHeroPos() - Vector2Int.right && VARIABLE.hasDoorRight))
                {
                    return true;
                }
            }
        }
        return false;
    }
    
    private bool MonsterOnPathToTarget(List<TileData> listOfEnemiesPos)
    {
        List<TileData> path = new List<TileData>();
        TileData currentTile = blackboard.hero.mapManager.GetTileDataAtPosition(blackboard.hero.GetIndexHeroPos().x, blackboard.hero.GetIndexHeroPos().y);
        while (currentTile != target)
        {
            List<TileData> targetList = new List<TileData>() { target };
            DirectionToMove nextTileDir = BFSScript.BFSGoInDirection(currentTile.IndexInMapArray, targetList, blackboard.hero.mapManager.getMapArray());
            switch (nextTileDir)
            {
                case DirectionToMove.Up:
                    currentTile = blackboard.hero.mapManager.GetTileDataAtPosition(currentTile.IndexInMapArray.x, currentTile.IndexInMapArray.y + 1);
                    break;
                case DirectionToMove.Down:
                    currentTile = blackboard.hero.mapManager.GetTileDataAtPosition(currentTile.IndexInMapArray.x, currentTile.IndexInMapArray.y - 1);
                    break;
                case DirectionToMove.Left:
                    currentTile = blackboard.hero.mapManager.GetTileDataAtPosition(currentTile.IndexInMapArray.x - 1, currentTile.IndexInMapArray.y);
                    break;
                case DirectionToMove.Right:
                    currentTile = blackboard.hero.mapManager.GetTileDataAtPosition(currentTile.IndexInMapArray.x + 1, currentTile.IndexInMapArray.y);
                    break;
                case DirectionToMove.None:
                    return false;
                default:
                    return false;
            }
            path.Add(currentTile);
        }
        
        foreach (var tile in path)
        {
            foreach (var VARIABLE in listOfEnemiesPos)
            {
                if (tile.IndexInMapArray == VARIABLE.IndexInMapArray)
                {
                    return true;
                }
            }
        }
        return false;
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