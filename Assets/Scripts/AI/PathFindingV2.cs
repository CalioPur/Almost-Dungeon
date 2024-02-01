using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class PathFindingV2
{
    // Le héros cartographie son évolution dans le donjon, c'est-à-dire qu'il enregistre le chemin vers toutes les tuiles qui sont passées dans son champ de vision rectiligne : 
    // Si le héros a en mémoire une tuile menant à une zone non-cartographiée , il sort de RAGE et se déplace :
    // au hasard vers une de celles présentes son champ de vision
    //     la plus proche de sa tuile s'il y en a aucune dans son champ de vision
    // Sinon, le héros entre en RAGE : il se déplace en direction de la tuile adjacente à une zone vide la plus proche de sa position et casse le mur séparant les deux tuiles, s’infligeant 1 PV de dégâts.
    //
    //     Dans le cas où un MONSTRE ou un OBSTACLE se trouve sur son chemin, il l'attaque pour libérer la voie avant d’avancer.
    //     Dans le cas où le héros s’aventure vers une case non construite, il attaque directement le dragon tapi dans la pénombre.
    
    static List<Vector2Int> exits = new();
    static List<Vector2Int> unvisitedTiles = new();
    static List<Vector2Int> tilesWithEnemies = new();
    
    public static DirectionToMove FindNextMove(Vector2Int startPos, TileData[,] map, List<PersonnalitiesV2> personalities,
        VisionType visionType, Aggressivity aggressivity, Objectives[] objectives)
    {
        
        if (personalities.Count > 0)
        {
            foreach (var personality in personalities.Where(personality => personality == PersonnalitiesV2.IMPATIENT))
            {
                FindExits(map);
                FindUnvisitedTiles(map);
                if (exits.Count <= 0 || unvisitedTiles.Count <= 0) continue;
                int minDistance = exits.Aggregate(int.MaxValue,
                    (current, exit) => unvisitedTiles.Select(unvisitedTile =>
                            Mathf.Abs(exit.x - unvisitedTile.x) + Mathf.Abs(exit.y - unvisitedTile.y)).Prepend(current)
                        .Min());
                if (minDistance < 5)
                {
                    return BreakWallClosestToEmptyTile(startPos, map);
                }
            }
        }
        
        return visionType switch
        {
            VisionType.LIGNEDROITE =>
                // Handle RECTILIGNE vision type
                Rectiligne(startPos, map, personalities, aggressivity, objectives),
            VisionType.BIGLEUX =>
                // Handle BIGLEUX vision type
                Bigleux(startPos, map, personalities, aggressivity, objectives),
            VisionType.CLAIRVOYANT =>
                // Handle CLAIRVOYANT vision type
                Clairvoyant(startPos, map, personalities, aggressivity, objectives),
            _ => throw new System.Exception("Unknown vision type")
        };
    }

    private static DirectionToMove BreakWallClosestToEmptyTile(Vector2Int startPos, TileData[,] map)
    {
        SoundManagerIngame.Instance.PlaySound(EmoteType.WallBreak);
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
        
        Vector2Int closestEmptyTile = FindClosestEmptyTile(startPos, map);
        
        // break the wall closest to the empty tile
        int minDistance = int.MaxValue;
        int directionToBreak = 0;
        for (int i = 0; i < possibleDirectionsToBreak.Length; i++)
        {
            if (possibleDirectionsToBreak[i] == 0) continue;
            switch (i)
            {
                case 0:
                    //down
                    if (Mathf.Abs(startPos.x - closestEmptyTile.x) + Mathf.Abs(startPos.y - 1 - closestEmptyTile.y) < minDistance)
                    {
                        minDistance = Mathf.Abs(startPos.x - closestEmptyTile.x) + Mathf.Abs(startPos.y - 1 - closestEmptyTile.y);
                        directionToBreak = i;
                    }
                    break;
                case 1:
                    //up
                    if (Mathf.Abs(startPos.x - closestEmptyTile.x) + Mathf.Abs(startPos.y + 1 - closestEmptyTile.y) < minDistance)
                    {
                        minDistance = Mathf.Abs(startPos.x - closestEmptyTile.x) + Mathf.Abs(startPos.y + 1 - closestEmptyTile.y);
                        directionToBreak = i;
                    }
                    break;
                case 2:
                    //left
                    if (Mathf.Abs(startPos.x - 1 - closestEmptyTile.x) + Mathf.Abs(startPos.y - closestEmptyTile.y) < minDistance)
                    {
                        minDistance = Mathf.Abs(startPos.x - 1 - closestEmptyTile.x) + Mathf.Abs(startPos.y - closestEmptyTile.y);
                        directionToBreak = i;
                    }
                    break;
                case 3:
                    //right
                    if (Mathf.Abs(startPos.x + 1 - closestEmptyTile.x) + Mathf.Abs(startPos.y - closestEmptyTile.y) < minDistance)
                    {
                        minDistance = Mathf.Abs(startPos.x + 1 - closestEmptyTile.x) + Mathf.Abs(startPos.y - closestEmptyTile.y);
                        directionToBreak = i;
                    }
                    break;
                default:
                    throw new System.Exception("Unknown direction");
            }
        }
        
        switch (directionToBreak)
        {
            case 0:
                //down
                tileWallBreaker.hasDoorDown = true;
                MapManager.Instance.ChangeTileDataAtPosition(startPos.x, startPos.y, tileWallBreaker,0);
                return DirectionToMove.Down;
            case 1:
                //up
                tileWallBreaker.hasDoorUp = true;
                MapManager.Instance.ChangeTileDataAtPosition(startPos.x, startPos.y, tileWallBreaker,0);
                return DirectionToMove.Up;
            case 2:
                //left
                tileWallBreaker.hasDoorLeft = true;
                MapManager.Instance.ChangeTileDataAtPosition(startPos.x, startPos.y, tileWallBreaker,0);
                return DirectionToMove.Left;
            case 3:
                //right
                tileWallBreaker.hasDoorRight = true;
                MapManager.Instance.ChangeTileDataAtPosition(startPos.x, startPos.y, tileWallBreaker,0);
                return DirectionToMove.Right;
            default:
                throw new System.Exception("Unknown direction");
        }
        
    }

    private static Vector2Int FindClosestEmptyTile(Vector2Int startPos, TileData[,] map)
    {
        // PiecePlaced == null
        List<Vector2Int> emptyTiles = new List<Vector2Int>();
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (!map[i, j].PiecePlaced)
                {
                    emptyTiles.Add(new Vector2Int(i, j));
                }
            }
        }
        int minDistance = emptyTiles.Aggregate(int.MaxValue,
            (current, emptyTile) => Mathf.Abs(emptyTile.x - startPos.x) + Mathf.Abs(emptyTile.y - startPos.y) < current
                ? Mathf.Abs(emptyTile.x - startPos.x) + Mathf.Abs(emptyTile.y - startPos.y)
                : current);
        return emptyTiles.First(emptyTile => Mathf.Abs(emptyTile.x - startPos.x) + Mathf.Abs(emptyTile.y - startPos.y) == minDistance);
    }

    private static DirectionToMove Clairvoyant(Vector2Int startPos, TileData[,] map, List<PersonnalitiesV2> personalities, Aggressivity aggressivity, Objectives[] objectives)
    {
        for (int i = 0; i < MapManager.Instance.width; i++)
        {
            for (int j = 0; j < MapManager.Instance.height; j++)
            {
                if (MapManager.Instance.mapArray[i, j].isConnectedToPath)
                {
                    MapManager.Instance.mapArray[i, j].isVisited = true;
                    map[i, j].isVisited = true;
                }
            }
        }
        if (aggressivity == Aggressivity.COURAGEUX)
        {
            FindTilesWithEnemies(map);
            if (tilesWithEnemies.Count > 0)
            {
                return BFSToObjective(startPos, map, tilesWithEnemies);
            }
        }
        
        switch (objectives.Length)
        {
            case 0:
                FindExits(map);
                return BFSToObjective(startPos, map, exits);
            case 1:
                switch (objectives[0])
                {
                    case Objectives.EXPLORATION:
                        FindExits(map);
                        if (aggressivity == Aggressivity.PEUREUX)
                        {
                            FindTilesWithEnemies(map);
                            DirectionToMove dir = BFSToObjectiveWithAvoidingEnemies(startPos, map, exits, tilesWithEnemies);
                            if (dir != DirectionToMove.None)
                            {
                                return dir;
                            }
                        }
                        return BFSToObjective(startPos, map, exits);
                        
                    case Objectives.SORTIE:
                        FindExits(map);
                        if (aggressivity == Aggressivity.PEUREUX)
                        {
                            FindTilesWithEnemies(map);
                            DirectionToMove dir = BFSToObjectiveWithAvoidingEnemies(startPos, map, exits, tilesWithEnemies);
                            if (dir != DirectionToMove.None)
                            {
                                return dir;
                            }
                        }
                        return BFSToObjective(startPos, map, exits);
                    default:
                        throw new Exception("Unknown objective");
                }
            default:
            {
                List<Vector2Int> objectivesPositions = new List<Vector2Int>();
                foreach (Objectives objective in objectives)
                {
                    switch (objective)
                    {
                        case Objectives.EXPLORATION:
                            FindUnvisitedTiles(map);
                            if (aggressivity == Aggressivity.PEUREUX)
                            {
                                foreach (Vector2Int tileWithEnemies in tilesWithEnemies)
                                {
                                    unvisitedTiles.Remove(tileWithEnemies);
                                }
                            }
                            objectivesPositions.AddRange(unvisitedTiles);
                            break;
                        case Objectives.SORTIE:
                            FindExits(map);
                            objectivesPositions.AddRange(exits);
                            break;
                        default:
                            throw new System.Exception("Unknown objective");
                    }
                }
                return BFSToObjective(startPos, map, objectivesPositions);
            }
        }
    }

    private static DirectionToMove BFSToObjectiveWithAvoidingEnemies(Vector2Int startPos, TileData[,] map, List<Vector2Int> vector2Ints, List<Vector2Int> list)
    {
        Queue<Vector2Int> queue = new();
        HashSet<Vector2Int> visited = new();
        Dictionary<Vector2Int, Vector2Int> parentMap = new();
        queue.Enqueue(startPos);
        visited.Add(startPos);
        while (queue.Count > 0)
        {
            Vector2Int currentPos = queue.Dequeue();
            if (vector2Ints.Contains(currentPos))
            {
                while (parentMap[currentPos] != startPos)
                {
                    currentPos = parentMap[currentPos];
                }
                if (currentPos.x == startPos.x)
                {
                    return currentPos.y > startPos.y ? DirectionToMove.Up : DirectionToMove.Down;
                }
                return currentPos.x > startPos.x ? DirectionToMove.Right : DirectionToMove.Left;
            }
            foreach (Vector2Int neighbor in GetNeighbors(currentPos, map))
            {
                if (!visited.Contains(neighbor) && !list.Contains(neighbor))
                {
                    queue.Enqueue(neighbor);
                    visited.Add(neighbor);
                    parentMap.Add(neighbor, currentPos);
                }
            }
        }
        return DirectionToMove.None;
    }

    private static DirectionToMove Bigleux(Vector2Int startPos, TileData[,] map, List<PersonnalitiesV2> personalities, Aggressivity aggressivity, Objectives[] objectives)
    {
        if (aggressivity == Aggressivity.COURAGEUX)
        {
            Vector2Int[] neighbors = GetNeighbors(startPos, map);
            foreach (Vector2Int neighbor in neighbors)
            {
                if (map[neighbor.x, neighbor.y].enemies.Count > 0)
                {
                    return DirectionFromTo(startPos, neighbor);
                }
            }
        }
        else
        {
            Vector2Int[] neighbors = GetNeighbors(startPos, map);
            List<Vector2Int> unvisitedNeighbors = new List<Vector2Int>();
            List<Vector2Int> neighborsWithEnemies = new List<Vector2Int>();
            foreach (Vector2Int neighbor in neighbors)
            {
                if (!map[neighbor.x, neighbor.y].isVisited)
                {
                    unvisitedNeighbors.Add(neighbor);
                }
                if (map[neighbor.x, neighbor.y].enemies.Count > 0)
                {
                    neighborsWithEnemies.Add(neighbor);
                }
            }
            
            if (neighborsWithEnemies.Count > 0)
            {
                List<Vector2Int> neighborsWithoutEnemies = new List<Vector2Int>();
                foreach (var neighborsOfNeighbor in neighborsWithEnemies.Select(neighbor => GetNeighbors(neighbor, map)))
                {
                    neighborsWithoutEnemies.AddRange(neighborsOfNeighbor.Where(neighborOfNeighbor => map[neighborOfNeighbor.x, neighborOfNeighbor.y].enemies.Count <= 0));
                }
                if (neighborsWithoutEnemies.Count > 0)
                {
                    int randomIndex = Random.Range(0, neighborsWithoutEnemies.Count);
                    return DirectionFromTo(startPos, neighborsWithoutEnemies[randomIndex]);
                }
                else
                {
                    int randomIndex = Random.Range(0, neighborsWithEnemies.Count);
                    return DirectionFromTo(startPos, neighborsWithEnemies[randomIndex]);
                }
            }

        }
        switch (objectives.Length)
        {
            case 0:
                return GoThroughRandomOpenDoor(startPos, map);
            case 1:
                switch (objectives[0])
                {
                    case Objectives.EXPLORATION:
                        Vector2Int[] neighbors = GetNeighbors(startPos, map);
                        return neighbors.Length > 0 ? DirectionFromTo(startPos, neighbors[Random.Range(0, neighbors.Length)]) : GoThroughRandomOpenDoor(startPos, map);
                    case Objectives.SORTIE:
                        FindExits(map);
                        CheckIfExitIsNeighbor(startPos, map, out Vector2Int exit);
                        if (exit != Vector2Int.zero)
                        {
                            return DirectionFromTo(startPos, exit);
                        }
                        return GoThroughRandomOpenDoor(startPos, map);
                    default:
                        throw new System.Exception("Unknown objective");
                }
            default:
            {
                int randomIndex = Random.Range(0, objectives.Length);
                switch (objectives[randomIndex])
                {
                    case Objectives.EXPLORATION:
                    case Objectives.SORTIE:
                        return GoThroughRandomOpenDoor(startPos, map);
                    default:
                        throw new System.Exception("Unknown objective");
                }
            }
        }
    }

    private static DirectionToMove Rectiligne(Vector2Int startPos, TileData[,] map, List<PersonnalitiesV2> personalities, Aggressivity aggressivity, Objectives[] objectives)
    {
        //first set all the tiles in straight line of sight to visited
        Vector2Int[] tilesInLineOfSight = MapManager.Instance.GetTilesInLineOfSight(startPos);
        // Handle RECTILIGNE vision type
        if (aggressivity == Aggressivity.COURAGEUX)
        {
            FindTilesWithEnemies(map);
            List<Vector2Int> enemiesInLineOfSight = new List<Vector2Int>();
            enemiesInLineOfSight = CheckIfEnemyIsInLineOfSight(startPos, tilesInLineOfSight, tilesWithEnemies);
            if (enemiesInLineOfSight.Count > 0)
            {
                // go in the direction of the first enemy in line of sight
                return DirectionFromTo(startPos, GetClosestEnemy(startPos, enemiesInLineOfSight));
            }
        }
        else
        {
            FindTilesWithEnemies(map);
            List<Vector2Int> enemiesInLineOfSight = new List<Vector2Int>();
            enemiesInLineOfSight = CheckIfEnemyIsInLineOfSight(startPos, tilesInLineOfSight, tilesWithEnemies);
            if (enemiesInLineOfSight.Count < 0)
            {
                FindUnvisitedTiles(map);
                if (objectives.Any(objective => objective == Objectives.SORTIE))
                {
                    CheckIfExitIsInLineOfSight(startPos, map, out Vector2Int exit);
                    if (exit != Vector2Int.zero)
                    {
                        return DirectionFromTo(startPos, exit);
                    }
                }
                if (unvisitedTiles.Count > 0)
                {
                    int minDistance = unvisitedTiles.Aggregate(int.MaxValue,
                        (current, unvisitedTile) => Mathf.Abs(unvisitedTile.x - startPos.x) + Mathf.Abs(unvisitedTile.y - startPos.y) < current
                            ? Mathf.Abs(unvisitedTile.x - startPos.x) + Mathf.Abs(unvisitedTile.y - startPos.y)
                            : current);
                    Vector2Int closestUnvisitedTile = unvisitedTiles.First(unvisitedTile => Mathf.Abs(unvisitedTile.x - startPos.x) + Mathf.Abs(unvisitedTile.y - startPos.y) == minDistance);
                    return DirectionFromTo(startPos, closestUnvisitedTile);
                }
            }

            FindUnvisitedTiles(map);
            if (objectives.Any(objective => objective == Objectives.SORTIE))
            {
                CheckIfExitIsInLineOfSight(startPos, map, out Vector2Int exit);
                if (exit != Vector2Int.zero)
                {
                    return DirectionFromTo(startPos, exit);
                }
            }
            if (unvisitedTiles.Count > 0)
            {
                // choose a random unvisited tile in line of sight
                int minDistance = unvisitedTiles.Aggregate(int.MaxValue,
                    (current, unvisitedTile) => Mathf.Abs(unvisitedTile.x - startPos.x) + Mathf.Abs(unvisitedTile.y - startPos.y) < current
                        ? Mathf.Abs(unvisitedTile.x - startPos.x) + Mathf.Abs(unvisitedTile.y - startPos.y)
                        : current);
                Vector2Int closestUnvisitedTile = unvisitedTiles.First(unvisitedTile => Mathf.Abs(unvisitedTile.x - startPos.x) + Mathf.Abs(unvisitedTile.y - startPos.y) == minDistance);
                return DirectionFromTo(startPos, closestUnvisitedTile);
            }
        }

        Debug.Log("Something went wrong, no direction found");
        return GoThroughRandomOpenDoor(startPos, map);
    }

    private static DirectionToMove GoToClosestExit(Vector2Int startPos, TileData[,] map)
    {
        FindExits(map);
        int minDistance = exits.Aggregate(int.MaxValue,
            (current, exit) => Mathf.Abs(exit.x - startPos.x) + Mathf.Abs(exit.y - startPos.y) < current
                ? Mathf.Abs(exit.x - startPos.x) + Mathf.Abs(exit.y - startPos.y)
                : current);
        Vector2Int closestExit = exits.First(exit => Mathf.Abs(exit.x - startPos.x) + Mathf.Abs(exit.y - startPos.y) == minDistance);
        return DirectionFromTo(startPos, closestExit);
    }

    private static List<Vector2Int> CheckIfEnemyIsInLineOfSight(Vector2Int startPos, IEnumerable<Vector2Int> tilesInLineOfSight,
        ICollection<Vector2Int> vector2Ints)
    {
        return tilesInLineOfSight.Where(tileInLineOfSight => vector2Ints.Contains(tileInLineOfSight)).ToList();
    }

    #region Tools
    
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
            return directions[randomIndex];
        }

        Debug.Log("Something went wrong, no door found");
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
    
    private static void FindTilesWithEnemies(TileData[,] map)
    {
        tilesWithEnemies.Clear();
        for (int i = 0; i < MapManager.Instance.width; i++)
        {
            for (int j = 0; j < MapManager.Instance.height; j++)
            {
                if (map[i, j].enemies.Count > 0)
                {
                    tilesWithEnemies.Add(new Vector2Int(i, j));
                }
            }
        }
    }
    
    private static DirectionToMove BFSToObjective(Vector2Int startPos, TileData[,] map, List<Vector2Int> listOfObjectives)
    {
        Queue<Vector2Int> queue = new();
        HashSet<Vector2Int> visited = new();
        Dictionary<Vector2Int, Vector2Int> parentMap = new();
        queue.Enqueue(startPos);
        visited.Add(startPos);
        while (queue.Count > 0)
        {
            Vector2Int currentPos = queue.Dequeue();
            if (listOfObjectives.Contains(currentPos))
            {
                while (parentMap[currentPos] != startPos)
                {
                    currentPos = parentMap[currentPos];
                }
                if (currentPos.x == startPos.x)
                {
                    return currentPos.y > startPos.y ? DirectionToMove.Up : DirectionToMove.Down;
                }
                return currentPos.x > startPos.x ? DirectionToMove.Right : DirectionToMove.Left;
            }
            foreach (Vector2Int neighbor in GetNeighbors(currentPos, map))
            {
                if (!visited.Contains(neighbor))
                {
                    queue.Enqueue(neighbor);
                    visited.Add(neighbor);
                    parentMap.Add(neighbor, currentPos);
                }
            }
        }
        Debug.Log("Something went wrong, no direction found");
        return DirectionToMove.None;
    }
    
    private static void FindExits(TileData[,] map)
    {
        exits.Clear();
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j].isExit)
                {
                    exits.Add(new Vector2Int(i, j));
                }
            }
        }
    }

    private static void FindUnvisitedTiles(TileData[,] map)
    {
        unvisitedTiles.Clear();
        exits.Clear();
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); i++)
            {
                if (!map[i, j].isVisited)
                {
                    unvisitedTiles.Add(new Vector2Int(i, j));
                }
                if (map[i, j].isExit)
                {
                    exits.Add(new Vector2Int(i, j));
                }
            }
        }
    }
    
    private static DirectionToMove DirectionFromTo(Vector2Int startPos, Vector2Int neighbor)
    {
        if (neighbor.x == startPos.x)
        {
            return neighbor.y > startPos.y ? DirectionToMove.Up : DirectionToMove.Down;
        }
        return neighbor.x > startPos.x ? DirectionToMove.Right : DirectionToMove.Left;
    }
    
    private static void CheckIfExitIsNeighbor(Vector2Int startPos, TileData[,] map, out Vector2Int vector2Int)
    {
        vector2Int = new Vector2Int();
        Vector2Int[] neighbors = GetNeighbors(startPos, map);
        foreach (Vector2Int neighbor in neighbors)
        {
            if (map[neighbor.x, neighbor.y].isExit)
            {
                vector2Int = neighbor;
                return;
            }
        }
    }
    
    private static Vector2Int GetClosestEnemy(Vector2Int startPos, List<Vector2Int> enemiesInLineOfSight)
    {
        int minDistance = enemiesInLineOfSight.Aggregate(int.MaxValue,
            (current, enemy) => Mathf.Abs(enemy.x - startPos.x) + Mathf.Abs(enemy.y - startPos.y) < current
                ? Mathf.Abs(enemy.x - startPos.x) + Mathf.Abs(enemy.y - startPos.y)
                : current);
        return enemiesInLineOfSight.First(enemy => Mathf.Abs(enemy.x - startPos.x) + Mathf.Abs(enemy.y - startPos.y) == minDistance);
    }
    
    private static void CheckIfExitIsInLineOfSight(Vector2Int startPos, TileData[,] map, out Vector2Int vector2Int)
    {
        vector2Int = new Vector2Int();
        Vector2Int[] tilesInLineOfSight = MapManager.Instance.GetTilesInLineOfSight(startPos);
        foreach (Vector2Int tileInLineOfSight in tilesInLineOfSight)
        {
            if (map[tileInLineOfSight.x, tileInLineOfSight.y].isExit)
            {
                vector2Int = tileInLineOfSight;
                return;
            }
        }
    }
    
    public static event Action OnNoPathFound;

    private static DirectionToMove BreakFreeFromNoExit(Vector2Int startPos, TileData[,] map)
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
                return DirectionToMove.Down;                    
            case 1:
                //up
                tileWallBreaker.hasDoorUp = true;
                MapManager.Instance.ChangeTileDataAtPosition(startPos.x, startPos.y, tileWallBreaker,2);
                return DirectionToMove.Up;
            case 2:
                //left
                tileWallBreaker.hasDoorLeft = true;
                MapManager.Instance.ChangeTileDataAtPosition(startPos.x, startPos.y, tileWallBreaker,1);
                return DirectionToMove.Left;
            case 3:
                //right
                tileWallBreaker.hasDoorRight = true;
                MapManager.Instance.ChangeTileDataAtPosition(startPos.x, startPos.y, tileWallBreaker,3);
                return DirectionToMove.Right;
        }
        return DirectionToMove.None;
    }

    #endregion
}
