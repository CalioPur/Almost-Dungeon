using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class BFSScript
{
    public static TileData BSFGoToTile(Vector2Int startPos, List<TileData> tilesObjectif, TileData[,] map, bool withLessEnnemies = false)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> parentMap = new Dictionary<Vector2Int, Vector2Int>();
        List<Vector2Int> exits = new List<Vector2Int>();

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
        
        TileData targetTile = GetNextPosition(startPos, parentMap, tilesObjectif, withLessEnnemies);
        return targetTile;
        // if (GoAttackDragon(startPos, map, out var bsfGoToTile)) return bsfGoToTile;
        //
        // if (GetDirectionToMove(startPos, nextPosition) == DirectionToMove.None)
        //     return GoThroughDoorWithNoTile(startPos, map);
        //
        //
        // return GetDirectionToMove(startPos, nextPosition);
    }

    public static DirectionToMove BFSGoInDirection(Vector2Int startPos, List<TileData> tilesObjectif, TileData[,] map,
        bool withLessEnnemies = false)
    {
        TileData targetTile = BSFGoToTile(startPos, tilesObjectif, map, withLessEnnemies);
        Vector2Int nextPosition = targetTile.IndexInMapArray;
        if (GoAttackDragon(startPos, map, out var bsfGoToTile)) return bsfGoToTile;
        if (GetDirectionToMove(startPos, nextPosition) == DirectionToMove.None)
            return GoThroughDoorWithNoTile(startPos, map);
        return GetDirectionToMove(startPos, nextPosition);
    }

    public static bool GoAttackDragon(Vector2Int startPos, TileData[,] map, out DirectionToMove bsfGoToTile)
    {
        if (map[startPos.x, startPos.y].hasDoorUp && !map[startPos.x, startPos.y + 1].PiecePlaced)
        {
            {
                bsfGoToTile = DirectionToMove.Up;
                return true;
            }
        }
        if (map[startPos.x, startPos.y].hasDoorDown && !map[startPos.x, startPos.y - 1].PiecePlaced)
        {
            {
                bsfGoToTile = DirectionToMove.Down;
                return true;
            }
        }
        if (map[startPos.x, startPos.y].hasDoorLeft && !map[startPos.x - 1, startPos.y].PiecePlaced)
        {
            {
                bsfGoToTile = DirectionToMove.Left;
                return true;
            }
        }
        if (map[startPos.x, startPos.y].hasDoorRight && !map[startPos.x + 1, startPos.y].PiecePlaced)
        {
            {
                bsfGoToTile = DirectionToMove.Right;
                return true;
            }
        }

        bsfGoToTile = DirectionToMove.None;
        return false;
    }

    private static DirectionToMove GoThroughDoorWithNoTile(Vector2Int startPos, TileData[,] map)
    {
        if (startPos.y == map.GetLength(1) - 1 && map[startPos.x, startPos.y].hasDoorUp)
        {
            return DirectionToMove.Up;
        }

        if (startPos.y == 0 && map[startPos.x, startPos.y].hasDoorDown)
        {
            return DirectionToMove.Down;
        }

        if (startPos.x == 0 && map[startPos.x, startPos.y].hasDoorLeft)
        {
            return DirectionToMove.Left;
        }

        if (startPos.x == map.GetLength(0) - 1 && map[startPos.x, startPos.y].hasDoorRight)
        {
            return DirectionToMove.Right;
        }
        
        if (map[startPos.x, startPos.y].hasDoorUp && !map[startPos.x, startPos.y + 1].PiecePlaced)
        {
            return DirectionToMove.Up;
        }

        if (map[startPos.x, startPos.y].hasDoorDown && !map[startPos.x, startPos.y - 1].PiecePlaced)
        {
            return DirectionToMove.Down;
        }

        if (map[startPos.x, startPos.y].hasDoorLeft && !map[startPos.x - 1, startPos.y].PiecePlaced)
        {
            return DirectionToMove.Left;
        }

        if (map[startPos.x, startPos.y].hasDoorRight && !map[startPos.x + 1, startPos.y].PiecePlaced)
        {
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
    
    public static List<TileData> GetNeighborsTiles(Vector2Int currentPos, TileData[,] map)
    {
        List<TileData> neighbors = new List<TileData>();

        if (currentPos.x > 0 && map[currentPos.x, currentPos.y].hasDoorLeft)
        {
            neighbors.Add(map[currentPos.x - 1, currentPos.y]);
        }

        if (currentPos.x < map.GetLength(0) - 1 && map[currentPos.x, currentPos.y].hasDoorRight)
        {
            neighbors.Add(map[currentPos.x + 1, currentPos.y]);
        }

        if (currentPos.y > 0 && map[currentPos.x, currentPos.y].hasDoorDown)
        {
            neighbors.Add(map[currentPos.x, currentPos.y - 1]);
        }

        if (currentPos.y < map.GetLength(1) - 1 && map[currentPos.x, currentPos.y].hasDoorUp)
        {
            neighbors.Add(map[currentPos.x, currentPos.y + 1]);
        }

        return neighbors;
    }
    
    private static TileData GetNextPosition(Vector2Int startPos, Dictionary<Vector2Int, Vector2Int> parentMap,
        List<TileData> goalTilesDatas, bool withLessEnemies = false)
    {
        List<Vector2Int> goalPositions = new List<Vector2Int>();
        foreach (var tileData in goalTilesDatas)
        {
            goalPositions.Add(tileData.IndexInMapArray);
        }
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

        return MapManager.Instance.GetTileDataAtPosition(closestPos.x, closestPos.y);
    }

    public static int DistanceFromExit(Vector2Int startPos, TileData[,] exits)
    {
        if (exits == null)
        {
            return 9999;
        }
        int minDist = 9999;
        foreach (var exit in exits)
        {
            Dictionary<Vector2Int, Vector2Int> parentMapCopy = new Dictionary<Vector2Int, Vector2Int>();
            Vector2Int currentPos = exit.IndexInMapArray;
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
            }
        }
        
        return minDist;
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
}
