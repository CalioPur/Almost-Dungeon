using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ALaid1
{
    public static Aggressivity aggressivity;
    public static List<Objectives> objectivesList;
    private static int numberOfExits;
    private static int numberOfUnvisitedTiles;
    private static TileData[,] map;
    
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
        map = mapDatas;
        UpdateNumberOfExitsAndUnvisitedTiles(map);
        if(numberOfExits == 0 && numberOfUnvisitedTiles == 0)
        {
            return PathFinding.BreakFreeFromNoExit(startPos, map);
        }
        
        if (aggressivity == Aggressivity.COURAGEUX)
        {
            //TODO check neighbours if they are enemies
        }
        else if (aggressivity == Aggressivity.PEUREUX)
        {
            //TODO check neighbours if they are enemies
        }
        
        return ChooseRandomPossibleDirection(startPos, map);
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
        List<TileData> tilesInLineOfSight = new List<TileData>();
        map = mapData;
        
        tilesInLineOfSight = GetTilesInLineOfSight(startPos, map);
        
        
        UpdateNumberOfExitsAndUnvisitedTiles(map);
        if(numberOfExits == 0 && numberOfUnvisitedTiles == 0)
        {
            return PathFinding.BreakFreeFromNoExit(startPos, map);
        }
        
        
        
        int numberOfVisibleEnemies = 0;
        
        foreach (var tileData in tilesInLineOfSight)
        {
            numberOfVisibleEnemies += tileData.enemies.Count;
        }
        
        if (aggressivity == Aggressivity.COURAGEUX && numberOfVisibleEnemies > 0)
        {
            PathFinding.BFSFindPath(startPos, map, Personnalities.TheKiller);
        }
        else if (aggressivity == Aggressivity.PEUREUX)
        {
            //TODO check if there are enemies in line of sight
        }

        return PathFinding.BFSFindPath(startPos, map, numberOfUnvisitedTiles == 0 ? Personnalities.HurryForTheExit : Personnalities.TheExplorer);
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
        while (simulatedPos.x < map.GetLength(0) - 2 && map[simulatedPos.x, simulatedPos.y].hasDoorRight)
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