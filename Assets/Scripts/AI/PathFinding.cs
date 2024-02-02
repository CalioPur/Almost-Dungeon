using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PathFinding
{
    public static event Action OnNoPathFound;
    public static Vector2Int HeroPos { get; set; }
    
    public static int distToClosestExit = 9999;

    public static DirectionToMove BFSFindPathWithLessEnemies(Vector2Int startPos, TileData[,] map, oldPerso personality)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> parentMap = new Dictionary<Vector2Int, Vector2Int>();
        List<Vector2Int> exits = new List<Vector2Int>();
        List<Vector2Int> unvisitedTiles = new List<Vector2Int>();
        List<Vector2Int> tileWithEnemies = new List<Vector2Int>();

        queue.Enqueue(startPos);
        visited.Add(startPos);

        int dbgInt = 0;
        while (queue.Count > 0)
        {
            Vector2Int currentPos = queue.Dequeue();

            if (map[currentPos.x, currentPos.y].isExit)
            {
                exits.Add(currentPos);
                Vector2Int nextPos = currentPos;
                while (parentMap.ContainsKey(nextPos) && parentMap[nextPos] != startPos)
                {
                    nextPos = parentMap[nextPos];
                }
            }

            if (!map[currentPos.x, currentPos.y].IsVisited || map[currentPos.x, currentPos.y].isExit)
            {
                unvisitedTiles.Add(currentPos);
            }

            if (map[currentPos.x, currentPos.y].enemies.Count > 0)
            {
                tileWithEnemies.Add(currentPos);
            }

            Vector2Int[] neighbors = GetNeighbors(currentPos, map);
            foreach (var neighbor in neighbors)
            {
                if (!visited.Contains(neighbor))
                {
                    queue.Enqueue(neighbor);
                    visited.Add(neighbor);
                    parentMap[neighbor] = currentPos;
                }
            }

            dbgInt++;
        }

        distToClosestExit = GetNumberOfTilesToClosestExit(startPos, parentMap, exits);

        switch (personality)
        {
            case oldPerso.HurryForTheExit when exits.Count > 0:
            {
                Vector2Int nextPos = GetNextPosition(startPos, parentMap, exits, true);
                if (GetDirectionToMove(startPos, nextPos) == DirectionToMove.None)
                {
                    return GoThroughDoorWithNoTile(startPos, map);
                }

                return GetDirectionToMove(startPos, nextPos);
            }
            // case oldPerso.HurryForTheExit when exits.Count == 0:
            // {
            //     if (CheckIfNotSurroundedByExits(startPos, map)) return GoThroughRandomOpenDoor(startPos, map);
            //     return BreakFreeFromNoExit(startPos, map);
            // }
            case oldPerso.TheExplorer when unvisitedTiles.Count > 0:
            {
                Vector2Int nextPos = GetNextPosition(startPos, parentMap, unvisitedTiles, true);
                return GetDirectionToMove(startPos, nextPos) == DirectionToMove.None
                    ? GoThroughDoorWithNoTile(startPos, map)
                    : GetDirectionToMove(startPos, nextPos);
            }
            case oldPerso.TheExplorer when exits.Count > 0:
            {
                Vector2Int nextPos = GetNextPosition(startPos, parentMap, exits, true);
                return GetDirectionToMove(startPos, nextPos) == DirectionToMove.None
                    ? GoThroughDoorWithNoTile(startPos, map)
                    : GetDirectionToMove(startPos, nextPos);
            }
            // case oldPerso.TheExplorer when exits.Count == 0:
            // {
            //     if (CheckIfNotSurroundedByExits(startPos, map)) return GoThroughRandomOpenDoor(startPos, map);
            //     return BreakFreeFromNoExit(startPos, map);
            //
            // }
            case oldPerso.TheKiller:
            {
                Vector2Int nextPos = GetNextPosition(startPos, parentMap, tileWithEnemies, true);
                return GetDirectionToMove(startPos, nextPos) == DirectionToMove.None
                    ? GoThroughDoorWithNoTile(startPos, map)
                    : GetDirectionToMove(startPos, nextPos);
            }
            case oldPerso.MoveToHero:
            {
                Vector2Int nextPos = GetNextPosition(startPos, parentMap, new List<Vector2Int>() { HeroPos }, true);
                if (GetDirectionToMove(startPos, nextPos) == DirectionToMove.None)
                {
                    return GoThroughDoorWithNoTile(startPos, map);
                }

                return GetDirectionToMove(startPos, nextPos);
            }
            case oldPerso.Nothing:
            {
                return DirectionToMove.None;
            }
            default:
                Debug.Log("No valid path found because no exit or unvisited tiles found");
                return GoThroughDoorWithNoTile(startPos, map);
        }
    }

    public static DirectionToMove BFSFindPath(Vector2Int startPos, TileData[,] map, oldPerso personality)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> parentMap = new Dictionary<Vector2Int, Vector2Int>();
        List<Vector2Int> exits = new List<Vector2Int>();
        List<Vector2Int> unvisitedTiles = new List<Vector2Int>();
        List<Vector2Int> tileWithEnemies = new List<Vector2Int>();

        queue.Enqueue(startPos);
        visited.Add(startPos);

        int dbgInt = 0;
        while (queue.Count > 0)
        {
            Vector2Int currentPos = queue.Dequeue();

            if (map[currentPos.x, currentPos.y].isExit)
            {
                exits.Add(currentPos);
                Vector2Int nextPos = currentPos;
                while (parentMap.ContainsKey(nextPos) && parentMap[nextPos] != startPos)
                {
                    nextPos = parentMap[nextPos];
                }
            }

            if (!map[currentPos.x, currentPos.y].IsVisited || map[currentPos.x, currentPos.y].isExit)
            {
                unvisitedTiles.Add(currentPos);
            }
            
            if (map[currentPos.x, currentPos.y].enemies.Count > 0)
            {
                tileWithEnemies.Add(currentPos);
            }

            Vector2Int[] neighbors = GetNeighbors(currentPos, map);
            foreach (var neighbor in neighbors)
            {
                if (!visited.Contains(neighbor))
                {
                    queue.Enqueue(neighbor);
                    visited.Add(neighbor);
                    parentMap[neighbor] = currentPos;
                }
            }

            dbgInt++;
        }
        
        distToClosestExit = GetNumberOfTilesToClosestExit(startPos, parentMap, exits);
        
        switch (personality)
        {
            case oldPerso.HurryForTheExit when exits.Count > 0:
            {
                Vector2Int nextPos = GetNextPosition(startPos, parentMap, exits);
                if (GetDirectionToMove(startPos, nextPos) == DirectionToMove.None)
                {
                    return GoThroughDoorWithNoTile(startPos, map);
                }
                return GetDirectionToMove(startPos, nextPos);
            }
            // case oldPerso.HurryForTheExit when exits.Count == 0:
            // {
            //     if(CheckIfNotSurroundedByExits(startPos, map)) return GoThroughRandomOpenDoor(startPos, map);
            //     return BreakFreeFromNoExit(startPos, map);
            // }
            case oldPerso.TheExplorer when unvisitedTiles.Count > 0:
            {
                Vector2Int nextPos = GetNextPosition(startPos, parentMap, unvisitedTiles);
                return GetDirectionToMove(startPos, nextPos) == DirectionToMove.None ? GoThroughDoorWithNoTile(startPos, map) : GetDirectionToMove(startPos, nextPos);
            }
            case oldPerso.TheExplorer when exits.Count > 0:
            {
                Vector2Int nextPos = GetNextPosition(startPos, parentMap, exits);
                return GetDirectionToMove(startPos, nextPos) == DirectionToMove.None ? GoThroughDoorWithNoTile(startPos, map) : GetDirectionToMove(startPos, nextPos);
            }
            // case oldPerso.TheExplorer when exits.Count == 0:
            // {
            //     if(CheckIfNotSurroundedByExits(startPos, map)) return GoThroughRandomOpenDoor(startPos, map);
            //     return BreakFreeFromNoExit(startPos, map);
            //
            // }
            case oldPerso.TheKiller:
            {
                Vector2Int nextPos = GetNextPosition(startPos, parentMap, tileWithEnemies);
                return GetDirectionToMove(startPos, nextPos) == DirectionToMove.None ? GoThroughDoorWithNoTile(startPos, map) : GetDirectionToMove(startPos, nextPos);
            }
            case oldPerso.MoveToHero:
            {
                Vector2Int nextPos = GetNextPosition(startPos, parentMap, new List<Vector2Int>(){HeroPos});
                if (GetDirectionToMove(startPos, nextPos) == DirectionToMove.None)
                {
                    return GoThroughDoorWithNoTile(startPos, map);
                }
                return GetDirectionToMove(startPos, nextPos);
            }
            case oldPerso.Nothing:
            {
                return DirectionToMove.None;
            }
            default:
                Debug.Log("No valid path found because no exit or unvisited tiles found");
                return GoThroughDoorWithNoTile(startPos, map);
        }
    }

    private static bool CheckIfNotSurroundedByExits(Vector2Int startPos, TileData[,] map)
    {
        if (startPos.x > 0 && map[startPos.x - 1, startPos.y].isExit)
        {
            return true;
        }

        if (startPos.x < map.GetLength(0) - 1 && map[startPos.x + 1, startPos.y].isExit)
        {
            return true;
        }

        if (startPos.y > 0 && map[startPos.x, startPos.y - 1].isExit)
        {
            return true;
        }

        return startPos.y < map.GetLength(1) - 1 && map[startPos.x, startPos.y + 1].isExit;
    }

    // public static DirectionToMove BreakFreeFromNoExit(Vector2Int startPos, TileData[,] map)
    // {
    //     SoundManagerIngame.Instance.PlaySound(EmoteType.WallBreak);
    //     OnNoPathFound?.Invoke();
    //     TileData tileWallBreaker = map[startPos.x, startPos.y];
    //     int[] possibleDirectionsToBreak = new int[4];
    //     if (!tileWallBreaker.hasDoorDown)
    //     {
    //         possibleDirectionsToBreak[0] = 1;
    //     }
    //     if (!tileWallBreaker.hasDoorUp)
    //     {
    //         possibleDirectionsToBreak[1] = 1;
    //     }
    //     if (!tileWallBreaker.hasDoorLeft)
    //     {
    //         possibleDirectionsToBreak[2] = 1;
    //     }
    //     if (!tileWallBreaker.hasDoorRight)
    //     {
    //         possibleDirectionsToBreak[3] = 1;
    //     }
    //     
    //     int randomIndex = Random.Range(0, possibleDirectionsToBreak.Length);
    //     int security = 0;
    //     while (possibleDirectionsToBreak[randomIndex] == 0 && security < 100)
    //     {
    //         randomIndex = Random.Range(0, possibleDirectionsToBreak.Length);
    //         security++;
    //     }
    //
    //     switch (randomIndex)
    //     {
    //         case 0:
    //             //down
    //             tileWallBreaker.hasDoorDown = true;
    //             MapManager.Instance.ChangeTileDataAtPosition(startPos.x, startPos.y, tileWallBreaker,0);
    //             MapManager.Instance.MapManagerTools.CheckAllTilesTypeAndRotation();
    //             return DirectionToMove.Down;
    //         case 1:
    //             //up
    //             tileWallBreaker.hasDoorUp = true;
    //             MapManager.Instance.ChangeTileDataAtPosition(startPos.x, startPos.y, tileWallBreaker,2);
    //             MapManager.Instance.MapManagerTools.CheckAllTilesTypeAndRotation();
    //             return DirectionToMove.Up;
    //         case 2:
    //             //left
    //             tileWallBreaker.hasDoorLeft = true;
    //             MapManager.Instance.ChangeTileDataAtPosition(startPos.x, startPos.y, tileWallBreaker,1);
    //             MapManager.Instance.MapManagerTools.CheckAllTilesTypeAndRotation();
    //             return DirectionToMove.Left;
    //         case 3:
    //             //right
    //             tileWallBreaker.hasDoorRight = true;
    //             MapManager.Instance.ChangeTileDataAtPosition(startPos.x, startPos.y, tileWallBreaker,3);
    //             MapManager.Instance.MapManagerTools.CheckAllTilesTypeAndRotation();
    //             return DirectionToMove.Right;
    //     }
    //     
    //     MapManager.Instance.MapManagerTools.CheckAllTilesTypeAndRotation();
    //     return DirectionToMove.None;
    // }

    private static Vector2Int GetNextPosition(Vector2Int startPos, Dictionary<Vector2Int, Vector2Int> parentMap,
        List<Vector2Int> goalPositions, bool withLessEnemies = false)
    {
        Vector2Int closestPos = goalPositions[0];
        int minDist = 9999;
        int minEnemies = 9999;
        foreach (var position in goalPositions)
        {
            Dictionary<Vector2Int, Vector2Int> parentMapCopy = new Dictionary<Vector2Int, Vector2Int>(parentMap);
            Vector2Int currentPos = position;
            int distance = 0;
            int nbEnemies = 0;
            while (parentMapCopy.ContainsKey(currentPos) && parentMapCopy[currentPos] != startPos)
            {
                Vector2Int parent = parentMapCopy[currentPos];
                parentMapCopy.Remove(currentPos);
                currentPos = parent;
                distance++;
                
                TileData tile = null;
                MapManager.Instance.GetTile(new Vector2Int(currentPos.x, currentPos.y), out tile);
                
                if (tile != null && tile.enemies.Count > 0)
                {
                   nbEnemies += tile.enemies.Count;
                }
                
            }

            if (minDist > distance && !withLessEnemies)
            {
                minDist = distance;
                closestPos = currentPos;
            }
            else if (minDist > distance && withLessEnemies && nbEnemies < minEnemies)
            {
                minDist = distance;
                minEnemies = nbEnemies;
                closestPos = currentPos;
            }
        }

        return closestPos;
    }

    public static int GetNumberOfTilesToClosestExit(Vector2Int startPos, Dictionary<Vector2Int, Vector2Int> parentMap,
        List<Vector2Int> exits)
    {
        if (exits.Count == 0)
        {
            return 9999;
        }
        Vector2Int closestPos = exits[0];
        int minDist = 9999;
        foreach (var position in exits)
        {
            Dictionary<Vector2Int, Vector2Int> parentMapCopy = new Dictionary<Vector2Int, Vector2Int>(parentMap);
            Vector2Int currentPos = position;
            int distance = 0;
            while (parentMapCopy.ContainsKey(currentPos) && parentMapCopy[currentPos] != startPos)
            {
                Vector2Int parent = parentMapCopy[currentPos];
                parentMapCopy.Remove(currentPos);
                currentPos = parent;
                distance++;
            }

            if (minDist > distance)
            {
                minDist = distance;
                closestPos = currentPos;
            }
        }
        
        return minDist;
    }
    
    public static int GetDistanceBetweenTwoTiles(Vector2Int startPos, Vector2Int endPos, Dictionary<Vector2Int, Vector2Int> parentMap)
    {
        Dictionary<Vector2Int, Vector2Int> parentMapCopy = new Dictionary<Vector2Int, Vector2Int>(parentMap);
        Vector2Int currentPos = endPos;
        int distance = 0;
        while (parentMapCopy.ContainsKey(currentPos) && parentMapCopy[currentPos] != startPos)
        {
            Vector2Int parent = parentMapCopy[currentPos];
            parentMapCopy.Remove(currentPos);
            currentPos = parent;
            distance++;
        }

        return distance;
    }
    
    private static DirectionToMove GetDirectionToMove(Vector2Int startPos, Vector2Int nextPos)
    {
        if (nextPos.x < startPos.x)
        {
            return DirectionToMove.Left;
        }

        if (nextPos.x > startPos.x)
        {
            return DirectionToMove.Right;
        }

        if (nextPos.y < startPos.y)
        {
            return DirectionToMove.Down;
        }

        if (nextPos.y > startPos.y)
        {
            return DirectionToMove.Up;
        }

        return DirectionToMove.None;
    }

    private static Vector2Int[] GetNeighbors(Vector2Int currentPos, TileData[,] map)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        if (currentPos.x > 0 && map[currentPos.x, currentPos.y].hasDoorLeft)
        {
            neighbors.Add(new Vector2Int(currentPos.x - 1, currentPos.y));
        }

        if (currentPos.x < map.GetLength(0) - 1 && map[currentPos.x, currentPos.y].hasDoorRight)
        {
            neighbors.Add(new Vector2Int(currentPos.x + 1, currentPos.y));
        }

        if (currentPos.y > 0 && map[currentPos.x, currentPos.y].hasDoorDown)
        {
            neighbors.Add(new Vector2Int(currentPos.x, currentPos.y - 1));
        }

        if (currentPos.y < map.GetLength(1) - 1 && map[currentPos.x, currentPos.y].hasDoorUp)
        {
            neighbors.Add(new Vector2Int(currentPos.x, currentPos.y + 1));
        }

        return neighbors.ToArray();
    }


    private static DirectionToMove GoThroughDoorWithNoTile(Vector2Int startPos, TileData[,] map)
    {
        // Check if there is a door on the upper border and no tile on the other side
        if (startPos.y == map.GetLength(1) - 1 && map[startPos.x, startPos.y].hasDoorUp)
        {
            Debug.Log("Go through door up");
            return DirectionToMove.Up;
        }

        // Check if there is a door on the lower border and no tile on the other side
        if (startPos.y == 0 && map[startPos.x, startPos.y].hasDoorDown)
        {
            Debug.Log("Go through door down");
            return DirectionToMove.Down;
        }

        // Check if there is a door on the left border and no tile on the other side
        if (startPos.x == 0 && map[startPos.x, startPos.y].hasDoorLeft)
        {
            Debug.Log("Go through door left");
            return DirectionToMove.Left;
        }

        // Check if there is a door on the right border and no tile on the other side
        if (startPos.x == map.GetLength(0) - 1 && map[startPos.x, startPos.y].hasDoorRight)
        {
            Debug.Log("Go through door right");
            return DirectionToMove.Right;
        }
        
        //go through the door that has no tile on the other side
        if (map[startPos.x, startPos.y].hasDoorUp && !map[startPos.x, startPos.y + 1].PiecePlaced)
        {
            Debug.Log("Go through door up");
            return DirectionToMove.Up;
        }

        if (map[startPos.x, startPos.y].hasDoorDown && !map[startPos.x, startPos.y - 1].PiecePlaced)
        {
            Debug.Log("Go through door down");
            return DirectionToMove.Down;
        }

        if (map[startPos.x, startPos.y].hasDoorLeft && !map[startPos.x - 1, startPos.y].PiecePlaced)
        {
            Debug.Log("Go through door left");
            return DirectionToMove.Left;
        }

        if (map[startPos.x, startPos.y].hasDoorRight && !map[startPos.x + 1, startPos.y].PiecePlaced)
        {
            Debug.Log("Go through door right");
            return DirectionToMove.Right;
        }

        return GoThroughRandomOpenDoor(startPos, map);
    }

    private static DirectionToMove GoThroughRandomOpenDoor(Vector2Int startPos, TileData[,] map)
    {
        //go through a random door that is open
        List<DirectionToMove> directions = new List<DirectionToMove>();
        if (map[startPos.x, startPos.y].hasDoorUp) directions.Add(DirectionToMove.Up);
        if (map[startPos.x, startPos.y].hasDoorDown) directions.Add(DirectionToMove.Down);
        if (map[startPos.x, startPos.y].hasDoorLeft) directions.Add(DirectionToMove.Left);
        if (map[startPos.x, startPos.y].hasDoorRight) directions.Add(DirectionToMove.Right);

        if (directions.Count > 0)
        {
            int randomIndex = Random.Range(0, directions.Count);
            Debug.Log("Go through random door : " + directions[randomIndex] + " because no tile on the other side");
            return directions[randomIndex];
        }

        Debug.Log("Something went wrong, no door found");
        return DirectionToMove.None;
    }
}