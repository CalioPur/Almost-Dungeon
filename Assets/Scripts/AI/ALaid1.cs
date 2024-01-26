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
        
        foreach(var personnalities in Hero.Instance.HeroBlackboard.personalities)
        {
            if (personnalities == PersonnalitiesV2.IMPATIENT)
            {
                if (PathFinding.distToClosestExit > 5)
                {
                    return PathFinding.BreakFreeFromNoExit(startPos, map);
                }
            }
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
        return PathFinding.BFSFindPathWithLessEnemies(startPos, tileDatas, numberOfUnvisitedTiles == 0 ? Personnalities.HurryForTheExit : Personnalities.TheExplorer);
    }

    public static List<TileData> GetTilesInLineOfSight(Vector2Int startPos, TileData[,] mapDatas)
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
        while (simulatedPos.y <= map.GetLength(1) && map[simulatedPos.x, simulatedPos.y].hasDoorUp)
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
        while (simulatedPos.x <= map.GetLength(0) && map[simulatedPos.x, simulatedPos.y].hasDoorRight)
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
        
        foreach(var personnalities in Hero.Instance.HeroBlackboard.personalities)
        {
            if (personnalities == PersonnalitiesV2.IMPATIENT)
            {
                if (PathFinding.distToClosestExit > 5)
                {
                    return PathFinding.BreakFreeFromNoExit(startPos, map);
                }
            }
        }
        
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