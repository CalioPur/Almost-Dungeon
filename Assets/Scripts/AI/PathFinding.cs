using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PathFinding
{
    public static event Action OnNoPathFound;
    public static Vector2Int HeroPos { get; set; }
    
    public static DirectionToMove BFSFindPath(Vector2Int startPos, TileData[,] map, Personnalities personality)
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

            if (!map[currentPos.x, currentPos.y].IsVisited)
            {
                unvisitedTiles.Add(currentPos);
            }
            
            if (map[currentPos.x, currentPos.y].enemies.Count > 0)
            {
                unvisitedTiles.Add(currentPos);
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
                if(CheckIfNotSurroundedByExits(startPos, map)) return GoThroughRandomOpenDoor(startPos, map);
                return BreakFreeFromNoExit(startPos, map);
            }
            case Personnalities.TheExplorer when unvisitedTiles.Count > 0:
            {
                Vector2Int nextPos = GetNextPosition(startPos, parentMap, unvisitedTiles);
                return GetDirectionToMove(startPos, nextPos) == DirectionToMove.None ? GoThroughDoorWithNoTile(startPos, map) : GetDirectionToMove(startPos, nextPos);
            }
            case Personnalities.TheExplorer when exits.Count > 0:
            {
                Vector2Int nextPos = GetNextPosition(startPos, parentMap, exits);
                return GetDirectionToMove(startPos, nextPos) == DirectionToMove.None ? GoThroughDoorWithNoTile(startPos, map) : GetDirectionToMove(startPos, nextPos);
            }
            case Personnalities.TheExplorer when exits.Count == 0:
            {
                if(CheckIfNotSurroundedByExits(startPos, map)) return GoThroughRandomOpenDoor(startPos, map);
                return BreakFreeFromNoExit(startPos, map);

            }
            case Personnalities.TheKiller:
            {
                Vector2Int nextPos = GetNextPosition(startPos, parentMap, tileWithEnemies);
                return GetDirectionToMove(startPos, nextPos) == DirectionToMove.None ? GoThroughDoorWithNoTile(startPos, map) : GetDirectionToMove(startPos, nextPos);
            }
            case Personnalities.MoveToHero:
            {
                Vector2Int nextPos = GetNextPosition(startPos, parentMap, new List<Vector2Int>(){HeroPos});
                if (GetDirectionToMove(startPos, nextPos) == DirectionToMove.None)
                {
                    return GoThroughDoorWithNoTile(startPos, map);
                }
                return GetDirectionToMove(startPos, nextPos);
            }
            default:
                Debug.Log("No valid path found because no exit or unvisited tiles found");
                return GoThroughDoorWithNoTile(startPos, map);
        }
    }

    public static DirectionToMove BFSFindPathV2(Vector2Int startPos, TileData[,] map,
        List<PersonnalitiesV2> personalities,
        VisionType visionType, Aggressivity aggressivity, Objectives[] objectives)
    {
        // if (personalities.Count > 0)
        // {
        //     //Break a wall and go through it
        //     if (personalities.Contains(PersonnalitiesV2.IMPATIENT))
        //     {
        //         if (CheckIfNotSurroundedByExits(startPos, map))
        //         {
        //             return GoThroughRandomOpenDoor(startPos, map);
        //         }
        //         return BreakFreeFromNoExit(startPos, map);
        //     }
        // }
        
        if (visionType != VisionType.CLAIRVOYANT)
        {
            // check the number of unvisited tiles if it is 0 and there is no exit, break a wall and go through it
            int unvisitedTiles = 0;
            for (int i = 0; i < map.GetLength(0) - 2; i++)
            {
                for (int j = 0; j < map.GetLength(1) - 2; j++)
                {
                    if (!map[i, j].IsVisited && map[i, j].isConnectedToPath)
                    {
                        unvisitedTiles++;
                    }
                }
            }
            
            int exits = 0;
            for (int i = 0; i < map.GetLength(0) - 2; i++)
            {
                for (int j = 0; j < map.GetLength(1) - 2; j++)
                {
                    if (map[i, j].isExit)
                    {
                        exits++;
                    }
                }
            }
            
            if (unvisitedTiles == 0 && exits == 0)
            {
                if (CheckIfNotSurroundedByExits(startPos, map))
                {
                    return GoThroughRandomOpenDoor(startPos, map);
                }

                return BreakFreeFromNoExit(startPos, map);
            }
        }

        switch (visionType)
        {
            case VisionType.RECTILIGNE:
                return LineVision(startPos, map,personalities, aggressivity, objectives);
            case VisionType.BIGLEUX:
                return CheckSuroundingTiles(startPos, map, personalities, aggressivity, objectives);
            case VisionType.CLAIRVOYANT:
                
                SetAllTilesAsVisited(map);
                
                if (aggressivity == Aggressivity.COURAGEUX)
                {
                    // check if there are enemies on the map
                    List<Vector2Int> enemies = new List<Vector2Int>();
                    for (int i = 0; i < map.GetLength(0) - 2; i++)
                    {
                        for (int j = 0; j < map.GetLength(1) - 2; j++)
                        {
                            if (map[i, j].enemies.Count > 0)
                            {
                                enemies.Add(new Vector2Int(i, j));
                            }
                        }
                    }
                    
                    if (enemies.Count > 0)
                    {
                        BFSFindPath(startPos, map, Personnalities.TheKiller);
                    }
                }
                return BFSFindPath(startPos, map, Personnalities.HurryForTheExit);
            default:
                throw new ArgumentOutOfRangeException(nameof(visionType), visionType, null);
        }
    }

    private static DirectionToMove LineVision(Vector2Int startPos, TileData[,] map, List<PersonnalitiesV2> personalities, Aggressivity aggressivity, Objectives[] objectives)
    {
        for (int i = 0; i < 4; i++)
        {
            int x = startPos.x;
            int y = startPos.y;
            bool hasNoDoor = false;
            while (x >= 0 && x < MapManager.Instance.width - 2 && y >= 0 && y < MapManager.Instance.height - 2 && !hasNoDoor)
            {
                switch (i)
                {
                    case 0:
                        if (map[x, y].hasDoorRight)
                        {
                            x += 1;
                        }
                        else
                        {
                            hasNoDoor = true;
                        }
                        break;
                    case 1:
                        if (map[x, y].hasDoorLeft)
                        {
                            x -= 1;
                        }
                        else
                        {
                            hasNoDoor = true;
                        }
                        break;
                    case 2:
                        if (map[x, y].hasDoorUp)
                        {
                            y += 1;
                        }
                        else
                        {
                            hasNoDoor = true;
                        }
                        break;
                    case 3:
                        if (map[x, y].hasDoorDown)
                        {
                            y -= 1;
                        }
                        else
                        {
                            hasNoDoor = true;
                        }
                        break;
                }
                
                //Debug the number of lines set to visited
                Debug.Log("Line " + i + " : x" + (startPos.x - x) + " y" + (startPos.y - y));
            }
        }

        if (aggressivity == Aggressivity.COURAGEUX)
        {
            return BFSFindPath(startPos, map, Personnalities.TheKiller);
        }

        foreach (var VARIABLE in objectives)
        {
            switch (VARIABLE)
            {
                case Objectives.EXPLORATION:
                    return BFSFindPath(startPos, map, Personnalities.TheExplorer);
                case Objectives.SORTIE:
                    return BFSFindPath(startPos, map, Personnalities.HurryForTheExit);
            }
        }
        
        return BFSFindPath(startPos, map, Personnalities.HurryForTheExit);
    }

    private static void SetAllTilesAsVisited(TileData[,] map)
    {
        for (int i = 0; i < MapManager.Instance.width - 2; i++)
        {
            for (int j = 0; j < MapManager.Instance.height - 2; j++)
            {
                if (MapManager.Instance.mapArray[i, j].isConnectedToPath)
                {
                    MapManager.Instance.mapArray[i, j].isVisited = true;
                    map[i, j].isVisited = true;
                }
            }
        }
    }

    private static DirectionToMove CheckSuroundingTiles(Vector2Int startPos, TileData[,] map, List<PersonnalitiesV2> personalities, Aggressivity aggressivity, Objectives[] objectives)
    {
        
        // check the 4 tiles around the hero, if they have a door
        List<DirectionToMove> directions = new List<DirectionToMove>();
        if (startPos.x > 0 && map[startPos.x, startPos.y].hasDoorLeft)
        {
            directions.Add(DirectionToMove.Left);
            MapManager.Instance.mapArray[startPos.x - 1, startPos.y].isVisited = true;
        }
        if (startPos.x < map.GetLength(0) - 1 && map[startPos.x, startPos.y].hasDoorRight)
        {
            directions.Add(DirectionToMove.Right);
            MapManager.Instance.mapArray[startPos.x + 1, startPos.y].isVisited = true;
        }
        if (startPos.y > 0 && map[startPos.x, startPos.y].hasDoorDown)
        {
            directions.Add(DirectionToMove.Down);
            MapManager.Instance.mapArray[startPos.x, startPos.y - 1].isVisited = true;
        }
        if (startPos.y < map.GetLength(1) - 1 && map[startPos.x, startPos.y].hasDoorUp)
        {
            directions.Add(DirectionToMove.Up);
            MapManager.Instance.mapArray[startPos.x, startPos.y + 1].isVisited = true;
        }
        
        if (directions.Count > 0)
        {
            if (aggressivity == Aggressivity.COURAGEUX)
            {
                List<DirectionToMove> directionsWithEnemies = new List<DirectionToMove>();
                foreach (var direction in directions)
                {
                    Vector2Int simulatedPos = startPos;
                    switch (direction)
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

                    if (map[simulatedPos.x, simulatedPos.y].enemies.Count > 0)
                    {
                        directionsWithEnemies.Add(direction);
                    }
                }
                if (directionsWithEnemies.Count > 0)
                {
                    int randomIndex = Random.Range(0, directionsWithEnemies.Count);
                    return directionsWithEnemies[randomIndex];
                }
            }

            if (aggressivity == Aggressivity.PEUREUX)
            {
                List<DirectionToMove> directionsWithNoEnemies = new List<DirectionToMove>();
                foreach (var direction in directions)
                {
                    Vector2Int simulatedPos = startPos;
                    switch (direction)
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

                    if (map[simulatedPos.x, simulatedPos.y].enemies.Count == 0)
                    {
                        directionsWithNoEnemies.Add(direction);
                    }
                }
                if (directionsWithNoEnemies.Count > 0)
                {
                    List<DirectionToMove> possibleDirections = new List<DirectionToMove>();
                    foreach (var objective in objectives)
                    {
                        if (objective == Objectives.EXPLORATION)
                        {
                            foreach (var direction in directionsWithNoEnemies)
                            {
                                Vector2Int simulatedPos = startPos;
                                switch (direction)
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

                                if (!map[simulatedPos.x, simulatedPos.y].IsVisited)
                                {
                                    possibleDirections.Add(direction);
                                }
                            }
                        }

                        if (objective == Objectives.SORTIE)
                        {
                            foreach (var direction in directionsWithNoEnemies)
                            {
                                Vector2Int simulatedPos = startPos;
                                switch (direction)
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

                                if (map[simulatedPos.x, simulatedPos.y].isExit)
                                {
                                    possibleDirections.Add(direction);
                                }
                            }
                        }
                    }
                    
                    if (possibleDirections.Count > 0)
                    {
                        int randomIndex = Random.Range(0, possibleDirections.Count);
                        return possibleDirections[randomIndex];
                    }
                    
                    return directionsWithNoEnemies[Random.Range(0, directionsWithNoEnemies.Count)];
                }
            }
        }
        
        return GoThroughRandomOpenDoor(startPos, map);
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

    public static DirectionToMove BreakFreeFromNoExit(Vector2Int startPos, TileData[,] map)
    {
        SoundManagerIngame.Instance.PlaySound(EmoteType.WallBreak);
        OnNoPathFound?.Invoke();
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
                MapManager.Instance.MapManagerTools.CheckAllTilesTypeAndRotation();
                return DirectionToMove.Down;
            case 1:
                //up
                tileWallBreaker.hasDoorUp = true;
                MapManager.Instance.ChangeTileDataAtPosition(startPos.x, startPos.y, tileWallBreaker,2);
                MapManager.Instance.MapManagerTools.CheckAllTilesTypeAndRotation();
                return DirectionToMove.Up;
            case 2:
                //left
                tileWallBreaker.hasDoorLeft = true;
                MapManager.Instance.ChangeTileDataAtPosition(startPos.x, startPos.y, tileWallBreaker,1);
                MapManager.Instance.MapManagerTools.CheckAllTilesTypeAndRotation();
                return DirectionToMove.Left;
            case 3:
                //right
                tileWallBreaker.hasDoorRight = true;
                MapManager.Instance.ChangeTileDataAtPosition(startPos.x, startPos.y, tileWallBreaker,3);
                MapManager.Instance.MapManagerTools.CheckAllTilesTypeAndRotation();
                return DirectionToMove.Right;
        }
        
        MapManager.Instance.MapManagerTools.CheckAllTilesTypeAndRotation();
        return DirectionToMove.None;
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