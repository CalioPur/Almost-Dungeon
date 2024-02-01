using System.Collections.Generic;
using UnityEngine;

public static class Bigleux
{
    // private Aggressivity aggressivity;
    // public static DirectionToMove Bigleux(Vector2Int startPos, TileData[,] mapDatas)
    // {
    //     DirectionToMove nothing = PathFinding.BFSFindPath(startPos, mapDatas, Personnalities.Nothing);
    //     distanceToExit = PathFinding.distToClosestExit;
    //     map = mapDatas;
    //     UpdateNumberOfExitsAndUnvisitedTiles(map);
    //     if (numberOfExits == 0 && numberOfUnvisitedTiles == 0)
    //     {
    //         return PathFinding.BreakFreeFromNoExit(startPos, map);
    //     }
    //
    //     foreach (var personnalities in Hero.Instance.HeroBlackboard.personalities)
    //     {
    //         if (personnalities == PersonnalitiesV2.IMPATIENT)
    //         {
    //             if (PathFinding.distToClosestExit > 5)
    //             {
    //                 return PathFinding.BreakFreeFromNoExit(startPos, map);
    //             }
    //         }
    //     }
    //
    //     Vector2Int[] neighboursWithEnemiesAndOpenDoors = GetNeighboursWithEnemiesAndOpenDoors(startPos, map);
    //
    //     if (aggressivity == Aggressivity.COURAGEUX && neighboursWithEnemiesAndOpenDoors.Length != 0)
    //     {
    //         return GoToEnemies(startPos, neighboursWithEnemiesAndOpenDoors);
    //     }
    //
    //     if (aggressivity == Aggressivity.PEUREUX && neighboursWithEnemiesAndOpenDoors.Length != 0)
    //     {
    //         //run away from enemies
    //         DirectionToMove dir = DirectionWithNoEnemies(startPos, map);
    //         if (dir != DirectionToMove.None)
    //         {
    //             return dir;
    //         }
    //     }
    //
    //     return ChooseRandomPossibleDirection(startPos, map);
    // }
    //
    // private static DirectionToMove GoToEnemies(Vector2Int startPos, Vector2Int[] neighboursWithEnemiesAndOpenDoors)
    // {
    //     Vector2Int chosenNeighbour =
    //         neighboursWithEnemiesAndOpenDoors[Random.Range(0, neighboursWithEnemiesAndOpenDoors.Length)];
    //     switch (chosenNeighbour)
    //     {
    //         case var n when n.y > startPos.y:
    //             return DirectionToMove.Up;
    //         case var n when n.y < startPos.y:
    //             return DirectionToMove.Down;
    //         case var n when n.x > startPos.x:
    //             return DirectionToMove.Right;
    //         case var n when n.x < startPos.x:
    //             return DirectionToMove.Left;
    //         default:
    //             Debug.LogError("Unknown direction");
    //             return DirectionToMove.None;
    //     }
    // }
    //
    // private static Vector2Int[] GetNeighboursWithEnemiesAndOpenDoors(Vector2Int startPos, TileData[,] tileDatas)
    // {
    //     map = tileDatas;
    //     List<Vector2Int> neighboursWithEnemiesAndOpenDoors = new List<Vector2Int>();
    //     if (map[startPos.x, startPos.y + 1].hasDoorDown && map[startPos.x, startPos.y + 1].enemies.Count != 0)
    //     {
    //         neighboursWithEnemiesAndOpenDoors.Add(new Vector2Int(startPos.x, startPos.y + 1));
    //     }
    //
    //     if (map[startPos.x, startPos.y - 1].hasDoorUp && map[startPos.x, startPos.y - 1].enemies.Count != 0)
    //     {
    //         neighboursWithEnemiesAndOpenDoors.Add(new Vector2Int(startPos.x, startPos.y - 1));
    //     }
    //
    //     if (map[startPos.x - 1, startPos.y].hasDoorRight && map[startPos.x - 1, startPos.y].enemies.Count != 0)
    //     {
    //         neighboursWithEnemiesAndOpenDoors.Add(new Vector2Int(startPos.x - 1, startPos.y));
    //     }
    //
    //     if (map[startPos.x + 1, startPos.y].hasDoorLeft && map[startPos.x + 1, startPos.y].enemies.Count != 0)
    //     {
    //         neighboursWithEnemiesAndOpenDoors.Add(new Vector2Int(startPos.x + 1, startPos.y));
    //     }
    //
    //     return neighboursWithEnemiesAndOpenDoors.ToArray();
    // }
    //
    // private static DirectionToMove ChooseRandomPossibleDirection(Vector2Int startPos, TileData[,] mapDatas)
    // {
    //     map = mapDatas;
    //     List<DirectionToMove> possibleDirections = new List<DirectionToMove>();
    //     if (map[startPos.x, startPos.y + 1].hasDoorDown)
    //     {
    //         possibleDirections.Add(DirectionToMove.Up);
    //     }
    //
    //     if (map[startPos.x, startPos.y - 1].hasDoorUp)
    //     {
    //         possibleDirections.Add(DirectionToMove.Down);
    //     }
    //
    //     if (map[startPos.x - 1, startPos.y].hasDoorRight)
    //     {
    //         possibleDirections.Add(DirectionToMove.Left);
    //     }
    //
    //     if (map[startPos.x + 1, startPos.y].hasDoorLeft)
    //     {
    //         Debug.Log("Right");
    //         possibleDirections.Add(DirectionToMove.Right);
    //     }
    //
    //     switch (possibleDirections.Count)
    //     {
    //         case 0:
    //             Debug.LogError("No possible direction");
    //             return DirectionToMove.None;
    //         case 1:
    //             Debug.Log("Only one possible direction");
    //             return possibleDirections[0];
    //         default:
    //             Debug.Log("Multiple possible direction");
    //             foreach (var VARIABLE in possibleDirections)
    //             {
    //                 switch (VARIABLE)
    //                 {
    //                     case DirectionToMove.Up:
    //                         MapManager.Instance.mapArray[startPos.x, startPos.y + 1].IsVisited = true;
    //                         break;
    //                     case DirectionToMove.Down:
    //                         MapManager.Instance.mapArray[startPos.x, startPos.y - 1].IsVisited = true;
    //                         break;
    //                     case DirectionToMove.Left:
    //                         MapManager.Instance.mapArray[startPos.x - 1, startPos.y].IsVisited = true;
    //                         break;
    //                     case DirectionToMove.Right:
    //                         MapManager.Instance.mapArray[startPos.x + 1, startPos.y].IsVisited = true;
    //                         break;
    //                     case DirectionToMove.None:
    //                         Debug.LogError("Direction to move is none");
    //                         break;
    //                     default:
    //                         Debug.LogError("Direction to move is Unknown");
    //                         break;
    //                 }
    //             }
    //
    //             return possibleDirections[Random.Range(0, possibleDirections.Count)];
    //     }
    // }
}