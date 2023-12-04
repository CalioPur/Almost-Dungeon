using System.Collections.Generic;
using UnityEngine;

public enum Personnalities
{
    HurryForTheExit,
    TheExplorer,
    TheKiller,
    TheSissy
}

public class PathFinding
{
    public static DirectionToMove BFSGoToClosestExit(Vector2Int startPos, TileData[,] map, Personnalities personality)
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

            if (!map[currentPos.x, currentPos.y].isVisited)
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
        
        switch (personality)
        {
            case Personnalities.HurryForTheExit when exits.Count > 0:
            {
                Vector2Int nextPos = GetNextPosition(startPos, parentMap, exits);
                if (GetDirectionToMove(startPos, nextPos) == DirectionToMove.None)
                {
                    return GoThroughDoorWithNoTile(startPos, map);
                }
                return GetDirectionToMove(startPos, nextPos);
            }
            case Personnalities.HurryForTheExit when exits.Count == 0:
            {
                BreakFreeFromNoExit(startPos, map);
                MapManager.Instance.MapManagerTools.CheckAllTilesTypeAndRotation();
                return DirectionToMove.None;
            }
            case Personnalities.TheExplorer when unvisitedTiles.Count > 0:
            {
                Vector2Int nextPos = GetNextPosition(startPos, parentMap, unvisitedTiles);
                if (GetDirectionToMove(startPos, nextPos) == DirectionToMove.None)
                {
                    return GoThroughDoorWithNoTile(startPos, map);
                }
                return GetDirectionToMove(startPos, nextPos);
            }
            case Personnalities.TheExplorer when exits.Count > 0:
            {
                Vector2Int nextPos = GetNextPosition(startPos, parentMap, exits);
                if (GetDirectionToMove(startPos, nextPos) == DirectionToMove.None)
                {
                    return GoThroughDoorWithNoTile(startPos, map);
                }
                return GetDirectionToMove(startPos, nextPos);
            }
            case Personnalities.TheExplorer when exits.Count == 0:
            {
                BreakFreeFromNoExit(startPos, map);
                MapManager.Instance.MapManagerTools.CheckAllTilesTypeAndRotation();
                return DirectionToMove.None;
            }
            case Personnalities.TheKiller when tileWithEnemies.Count > 0:
            {
                Vector2Int nextPos = GetNextPosition(startPos, parentMap, tileWithEnemies);
                if (GetDirectionToMove(startPos, nextPos) == DirectionToMove.None)
                {
                    return GoThroughDoorWithNoTile(startPos, map);
                }
                return GetDirectionToMove(startPos, nextPos);
            }
            case Personnalities.TheKiller when exits.Count > 0:
            {
                Vector2Int nextPos = GetNextPosition(startPos, parentMap, exits);
                if (GetDirectionToMove(startPos, nextPos) == DirectionToMove.None)
                {
                    return GoThroughDoorWithNoTile(startPos, map);
                }
                return GetDirectionToMove(startPos, nextPos);
            }
            case Personnalities.TheKiller when exits.Count == 0:
            {
                BreakFreeFromNoExit(startPos, map);
                MapManager.Instance.MapManagerTools.CheckAllTilesTypeAndRotation();
                return DirectionToMove.None;
            }
            case Personnalities.TheSissy when tileWithEnemies.Count > 0:
            {
                List<Vector2Int> nextPositionsPossible = new List<Vector2Int>();
                //go to the tile with the the less enemies
                int minEnemies = int.MaxValue;
                foreach (var tile in tileWithEnemies)
                {
                    if (map[tile.x, tile.y].enemies.Count < minEnemies)
                    {
                        minEnemies = map[tile.x, tile.y].enemies.Count;
                        nextPositionsPossible.Clear();
                        nextPositionsPossible.Add(tile);
                    }
                    else if (map[tile.x, tile.y].enemies.Count == minEnemies)
                    {
                        nextPositionsPossible.Add(tile);
                    }
                }
                
                Vector2Int nextPos = GetNextPosition(startPos, parentMap, nextPositionsPossible);
                if (GetDirectionToMove(startPos, nextPos) == DirectionToMove.None)
                {
                    return GoThroughDoorWithNoTile(startPos, map);
                }
                return GetDirectionToMove(startPos, nextPos);
            }
            case Personnalities.TheSissy when exits.Count > 0:
            {
                Vector2Int nextPos = GetNextPosition(startPos, parentMap, exits);
                if (GetDirectionToMove(startPos, nextPos) == DirectionToMove.None)
                {
                    return GoThroughDoorWithNoTile(startPos, map);
                }
                return GetDirectionToMove(startPos, nextPos);
            }
            case Personnalities.TheSissy when exits.Count == 0:
            {
                BreakFreeFromNoExit(startPos, map);
                MapManager.Instance.MapManagerTools.CheckAllTilesTypeAndRotation();
                return DirectionToMove.None;
            }
            default:
                Debug.Log("No valid path found because no exit or unvisited tiles found");
                return GoThroughDoorWithNoTile(startPos, map);
        }
    }

    private static void BreakFreeFromNoExit(Vector2Int startPos, TileData[,] map)
    {
        TileData tileWallBreaker = map[startPos.x, startPos.y];
        int[] possibleDirectionsToBreak = new int[4];
        if (!tileWallBreaker.hasDoorDown)
        {
            possibleDirectionsToBreak[0] = 1;
        }
        if (!tileWallBreaker.hasDoorUp)
        {
            possibleDirectionsToBreak[1] = 1;
        }
        if (!tileWallBreaker.hasDoorLeft)
        {
            possibleDirectionsToBreak[2] = 1;
        }
        if (!tileWallBreaker.hasDoorRight)
        {
            possibleDirectionsToBreak[3] = 1;
        }
        
        int randomIndex = Random.Range(0, possibleDirectionsToBreak.Length);
        int security = 0;
        while (possibleDirectionsToBreak[randomIndex] == 0 && security < 100)
        {
            randomIndex = Random.Range(0, possibleDirectionsToBreak.Length);
            security++;
        }

        switch (randomIndex)
        {
            case 0:
                //down
                tileWallBreaker.hasDoorDown = true;
                MapManager.Instance.ChangeTileDataAtPosition(startPos.x, startPos.y, tileWallBreaker,0);
                break;
            case 1:
                //up
                tileWallBreaker.hasDoorUp = true;
                MapManager.Instance.ChangeTileDataAtPosition(startPos.x, startPos.y, tileWallBreaker,2);
                break;
            case 2:
                //left
                tileWallBreaker.hasDoorLeft = true;
                MapManager.Instance.ChangeTileDataAtPosition(startPos.x, startPos.y, tileWallBreaker,1);
                break;
            case 3:
                //right
                tileWallBreaker.hasDoorRight = true;
                MapManager.Instance.ChangeTileDataAtPosition(startPos.x, startPos.y, tileWallBreaker,3);
                break;
        }
    }

    private static Vector2Int GetNextPosition(Vector2Int startPos, Dictionary<Vector2Int, Vector2Int> parentMap, List<Vector2Int> positions)
    {
        Vector2Int closestPos = positions[0];
        float closestDistance = Vector2Int.Distance(startPos, closestPos);

        foreach (var pos in positions)
        {
            float distance = Vector2Int.Distance(startPos, pos);
            if (distance < closestDistance)
            {
                closestPos = pos;
                closestDistance = distance;
            }
        }

        Vector2Int nextPos = closestPos;
        while (parentMap.ContainsKey(nextPos) && parentMap[nextPos] != startPos)
        {
            nextPos = parentMap[nextPos];
        }

        return nextPos;
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