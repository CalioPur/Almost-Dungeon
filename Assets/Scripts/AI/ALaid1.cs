using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ALaid1
{
    public static Aggressivity aggressivity;
    public static List<Objectives> objectivesList;
    private static int numberOfExits;
    private static int numberOfUnvisitedTiles;
    private static TileData[,] map;
    
    public static int distanceToExit = int.MaxValue;
    
    private static void UpdateNumberOfExitsAndUnvisitedTiles(TileData[,] mapDatas)
    {
        numberOfExits = 0;
        numberOfUnvisitedTiles = 0;
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < MapManager.Instance.mapArray.GetLength(1); j++)
            {
                if (map[i, j].isExit)
                {
                    numberOfExits++;
                }
                if (!map[i, j].IsVisited && map[i, j].isConnectedToPath)
                {
                    numberOfUnvisitedTiles++;
                }
            }
        }
    }

    #region Bigleux

    public static DirectionToMove Bigleux(Vector2Int startPos, TileData[,] mapDatas)
    {
        DirectionToMove nothing = PathFinding.BFSFindPath(startPos, mapDatas, Personnalities.Nothing);
        distanceToExit = PathFinding.distToClosestExit;
        map = mapDatas;
        UpdateNumberOfExitsAndUnvisitedTiles(map);
        if(numberOfExits == 0 && numberOfUnvisitedTiles == 0)
        {
            return PathFinding.BreakFreeFromNoExit(startPos, map);
        }
        
        Vector2Int[] neighboursWithEnemiesAndOpenDoors = GetNeighboursWithEnemiesAndOpenDoors(startPos, map);
        
        if (aggressivity == Aggressivity.COURAGEUX && neighboursWithEnemiesAndOpenDoors.Length != 0)
        {
            return GoToEnemies(startPos, neighboursWithEnemiesAndOpenDoors);
        }

        if (aggressivity == Aggressivity.PEUREUX && neighboursWithEnemiesAndOpenDoors.Length != 0)
        {
            //run away from enemies
            DirectionToMove dir = DirectionWithNoEnemies(startPos, map);
            if (dir != DirectionToMove.None)
            {
                return dir;
            }
        }

        return ChooseRandomPossibleDirection(startPos, map);
    }

    private static DirectionToMove GoToEnemies(Vector2Int startPos, Vector2Int[] neighboursWithEnemiesAndOpenDoors)
    {
        Vector2Int chosenNeighbour = neighboursWithEnemiesAndOpenDoors[Random.Range(0, neighboursWithEnemiesAndOpenDoors.Length)];
        switch (chosenNeighbour)
        {
            case var n when n.y > startPos.y:
                return DirectionToMove.Up;
            case var n when n.y < startPos.y:
                return DirectionToMove.Down;
            case var n when n.x > startPos.x:
                return DirectionToMove.Right;
            case var n when n.x < startPos.x:
                return DirectionToMove.Left;
            default:
                Debug.LogError("Unknown direction");
                return DirectionToMove.None;
        }
    }

    private static Vector2Int[] GetNeighboursWithEnemiesAndOpenDoors(Vector2Int startPos, TileData[,] tileDatas)
    {
        map = tileDatas;
        List<Vector2Int> neighboursWithEnemiesAndOpenDoors = new List<Vector2Int>();
        if (map[startPos.x, startPos.y + 1].hasDoorDown && map[startPos.x, startPos.y + 1].enemies.Count != 0)
        {
            neighboursWithEnemiesAndOpenDoors.Add(new Vector2Int(startPos.x, startPos.y + 1));
        }
        if (map[startPos.x, startPos.y - 1].hasDoorUp && map[startPos.x, startPos.y - 1].enemies.Count != 0)
        {
            neighboursWithEnemiesAndOpenDoors.Add(new Vector2Int(startPos.x, startPos.y - 1));
        }
        if (map[startPos.x - 1, startPos.y].hasDoorRight && map[startPos.x - 1, startPos.y].enemies.Count != 0)
        {
            neighboursWithEnemiesAndOpenDoors.Add(new Vector2Int(startPos.x - 1, startPos.y));
        }
        if (map[startPos.x + 1, startPos.y].hasDoorLeft && map[startPos.x + 1, startPos.y].enemies.Count != 0)
        {
            neighboursWithEnemiesAndOpenDoors.Add(new Vector2Int(startPos.x + 1, startPos.y));
        }

        return neighboursWithEnemiesAndOpenDoors.ToArray();
    }

    private static DirectionToMove ChooseRandomPossibleDirection(Vector2Int startPos, TileData[,] mapDatas)
    {
        map = mapDatas;
        List<DirectionToMove> possibleDirections = new List<DirectionToMove>();
        if (map[startPos.x, startPos.y + 1].hasDoorDown)
        {
            possibleDirections.Add(DirectionToMove.Up);
        }
        if (map[startPos.x, startPos.y - 1].hasDoorUp)
        {
            possibleDirections.Add(DirectionToMove.Down);
        }
        if (map[startPos.x - 1, startPos.y].hasDoorRight)
        {
            possibleDirections.Add(DirectionToMove.Left);
        }
        if (map[startPos.x + 1, startPos.y].hasDoorLeft)
        {
            Debug.Log("Right");
            possibleDirections.Add(DirectionToMove.Right);
        }

        switch (possibleDirections.Count)
        {
            case 0:
                Debug.LogError("No possible direction");
                return DirectionToMove.None;
            case 1:
                Debug.Log("Only one possible direction");
                return possibleDirections[0];
            default:
                Debug.Log("Multiple possible direction");
                foreach (var VARIABLE in possibleDirections)
                {
                    switch (VARIABLE)
                    {
                        case DirectionToMove.Up:
                            MapManager.Instance.mapArray[startPos.x, startPos.y + 1].IsVisited = true;
                            break;
                        case DirectionToMove.Down:
                            MapManager.Instance.mapArray[startPos.x, startPos.y - 1].IsVisited = true;
                            break;
                        case DirectionToMove.Left:
                            MapManager.Instance.mapArray[startPos.x - 1, startPos.y].IsVisited = true;
                            break;
                        case DirectionToMove.Right:
                            MapManager.Instance.mapArray[startPos.x + 1, startPos.y].IsVisited = true;
                            break;
                        case DirectionToMove.None:
                            Debug.LogError("Direction to move is none");
                            break;
                        default:
                            Debug.LogError("Direction to move is Unknown");
                            break;
                    }
                }
                return possibleDirections[Random.Range(0, possibleDirections.Count)];
        }
    }

    #endregion

    #region Line
    
    public static DirectionToMove Line(Vector2Int startPos, TileData[,] mapData)
    {
        DirectionToMove nothing = PathFinding.BFSFindPath(startPos, mapData, Personnalities.Nothing);
        distanceToExit = PathFinding.distToClosestExit;
        List<TileData> tilesInLineOfSight = new List<TileData>();
        map = mapData;
        
        tilesInLineOfSight = GetTilesInLineOfSight(startPos, map);
        
        
        UpdateNumberOfExitsAndUnvisitedTiles(map);
        if(numberOfExits == 0 && numberOfUnvisitedTiles == 0)
        {
            return PathFinding.BreakFreeFromNoExit(startPos, map);
        }
        
        
        
        int numberOfVisibleEnemies = tilesInLineOfSight.Sum(tileData => tileData.enemies.Count);

        if (aggressivity == Aggressivity.COURAGEUX && numberOfVisibleEnemies > 0)
        {
            return PathFinding.BFSFindPath(startPos, map, Personnalities.TheKiller);
        }

        if (aggressivity == Aggressivity.PEUREUX && numberOfVisibleEnemies > 0)
        {
            //run away from enemies
            DirectionToMove dir = DirectionWithNoEnemies(startPos, map);
            if (dir != DirectionToMove.None)
            {
                return dir;
            }
        }

        return PathFinding.BFSFindPath(startPos, map, numberOfUnvisitedTiles == 0 ? Personnalities.HurryForTheExit : Personnalities.TheExplorer);
    }

    private static DirectionToMove DirectionWithNoEnemies(Vector2Int startPos, TileData[,] tileDatas)
    {
        map = tileDatas;
        List<DirectionToMove> possibleDirections = new List<DirectionToMove>();
        if (startPos.y <=  map.GetLength(1) - 2 && map[startPos.x, startPos.y + 1].hasDoorDown && map[startPos.x, startPos.y + 1].enemies.Count == 0)
        {
            possibleDirections.Add(DirectionToMove.Up);
        }
        if (startPos.y > 0 && map[startPos.x, startPos.y - 1].hasDoorUp && map[startPos.x, startPos.y - 1].enemies.Count == 0)
        {
            possibleDirections.Add(DirectionToMove.Down);
        }
        if (startPos.x > 0 && map[startPos.x - 1, startPos.y].hasDoorRight && map[startPos.x - 1, startPos.y].enemies.Count == 0)
        {
            possibleDirections.Add(DirectionToMove.Left);
        }
        if (startPos.x <= map.GetLength(0) - 2 && map[startPos.x + 1, startPos.y].hasDoorLeft && map[startPos.x + 1, startPos.y].enemies.Count == 0)
        {
            possibleDirections.Add(DirectionToMove.Right);
        }

        switch (possibleDirections.Count)
        {
            case 0:
                return DirectionToMove.None;
            case 1:
                return possibleDirections[0];
            default:
                foreach (var VARIABLE in possibleDirections)
                {
                    switch (VARIABLE)
                    {
                        case DirectionToMove.Up:
                            MapManager.Instance.mapArray[startPos.x, startPos.y + 1].IsVisited = true;
                            break;
                        case DirectionToMove.Down:
                            MapManager.Instance.mapArray[startPos.x, startPos.y - 1].IsVisited = true;
                            break;
                        case DirectionToMove.Left:
                            MapManager.Instance.mapArray[startPos.x - 1, startPos.y].IsVisited = true;
                            break;
                        case DirectionToMove.Right:
                            MapManager.Instance.mapArray[startPos.x + 1, startPos.y].IsVisited = true;
                            break;
                    }
                }
                return possibleDirections[Random.Range(0, possibleDirections.Count)];
        }
    }

    private static List<TileData> GetTilesInLineOfSight(Vector2Int startPos, TileData[,] mapDatas)
    {
        map = mapDatas;
        List<TileData> visibleTiles = new List<TileData>();
        Vector2Int simulatedPos = startPos;
        
        int debug = 0;

        while (simulatedPos.y > 0 && map[simulatedPos.x, simulatedPos.y].hasDoorDown)
        {
            simulatedPos.y -= 1;
            visibleTiles.Add(map[simulatedPos.x, simulatedPos.y]);
            MapManager.Instance.mapArray[simulatedPos.x, simulatedPos.y].IsVisited = true;
            debug++;
        }

        simulatedPos = startPos;
        while (simulatedPos.y <= map.GetLength(1) - 2 && map[simulatedPos.x, simulatedPos.y].hasDoorUp)
        {
            simulatedPos.y += 1;
            visibleTiles.Add(map[simulatedPos.x, simulatedPos.y]);
            MapManager.Instance.mapArray[simulatedPos.x, simulatedPos.y].IsVisited = true;
            debug++;
        }

        simulatedPos = startPos;
        while (simulatedPos.x > 0 && map[simulatedPos.x, simulatedPos.y].hasDoorLeft)
        {
            simulatedPos.x -= 1;
            visibleTiles.Add(map[simulatedPos.x, simulatedPos.y]);
            MapManager.Instance.mapArray[simulatedPos.x, simulatedPos.y].IsVisited = true;
            debug++;
        }

        simulatedPos = startPos;
        while (simulatedPos.x <= map.GetLength(0) - 2 && map[simulatedPos.x, simulatedPos.y].hasDoorRight)
        {
            simulatedPos.x += 1;
            visibleTiles.Add(map[simulatedPos.x, simulatedPos.y]);
            MapManager.Instance.mapArray[simulatedPos.x, simulatedPos.y].IsVisited = true;
            debug++;
        }
        Debug.Log("Debug : " + debug);
        return visibleTiles;
    }

    #endregion

    #region Clairvoyant

    private static int numberOfEnemies;
    
    public static DirectionToMove Clairvoyant(Vector2Int startPos, TileData[,] mapDatas)
    {
        DirectionToMove nothing = PathFinding.BFSFindPath(startPos, mapDatas, Personnalities.Nothing);
        distanceToExit = PathFinding.distToClosestExit;
        map = mapDatas;
        MapManager.Instance.SetAllTilesAsVisited();
        numberOfEnemies = CheckNumberOfEnemiesOnMap(map);
        if (aggressivity == Aggressivity.COURAGEUX && numberOfEnemies > 0)
        {
            return PathFinding.BFSFindPath(startPos, map, Personnalities.TheKiller);
        }

        if (aggressivity == Aggressivity.PEUREUX && numberOfEnemies > 0)
        {
            //TODO JSP ALAID
        }
        return PathFinding.BFSFindPath(startPos, map, Personnalities.TheExplorer);
    }

    private static int CheckNumberOfEnemiesOnMap(TileData[,] mapDatas)
    {
        int numberOfEnemies = 0;
        for (int i = 0; i < mapDatas.GetLength(0); i++)
        {
            for (int j = 0; j < mapDatas.GetLength(1); j++)
            {
                if (mapDatas[i, j].enemies.Count != 0 && mapDatas[i, j].isConnectedToPath)
                {
                    numberOfEnemies++;
                }
            }
        }
        return numberOfEnemies;
    }

    #endregion
        
}