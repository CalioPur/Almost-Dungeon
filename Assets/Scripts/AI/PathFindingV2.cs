using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PersonnalitiesV2
{
    EXPLORATEUR, // Lorsqu'il y a plusieurs tuiles menant à une zone inexplorée dans son champ de vision, se déplace vers la plus proche.
    IMPATIENT // Lorsqu'il se trouve à plus de 5 tuiles de la sortie, entre en RAGE.
}

public enum VisionType
{
    RECTILIGNE, // Le héros voit en ligne droite dans les 4 directions
    BIGLEUX, // Le hero se déplace de manière aléatoire sur les tile non visité adjacentes à sa position sinon complètement aléatoire
    CLAIRVOYANT, // Le hero va au chemin le plus court vers son objectif
}

public enum Aggressivity
{
    PEUREUX, // L'aventurier n'attaque pas et s'éloigne des minions s'il peut poursuivre l'exploration du donjon (c'est-à-dire s'il peut encore découvrir une zone inexplorée)
    COURAGEUX, // L'aventurier va attaquer coute que coute l'ennemi le plus proche dans son champ de vision, meme s'il n'entrave pas son exploration du donjon.
}

public enum Objectives
{
    EXPLORATION, // L'aventurier cherche à explorer le donjon
    SORTIE, // L'aventurier cherche à atteindre la sortie
}

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
            foreach (PersonnalitiesV2 personality in personalities)
            {
                if (personality != PersonnalitiesV2.IMPATIENT) continue;
                FindExits(map);
                FindUnvisitedTiles(map);
                if (exits.Count <= 0 || unvisitedTiles.Count <= 0) continue;
                int minDistance = exits.Aggregate(int.MaxValue,
                    (current, exit) => unvisitedTiles.Select(unvisitedTile =>
                            Mathf.Abs(exit.x - unvisitedTile.x) + Mathf.Abs(exit.y - unvisitedTile.y)).Prepend(current)
                        .Min());
                if (minDistance < 5)
                {
                    Debug.Log("RAGE");
                }
            }
        }
        
        return visionType switch
        {
            VisionType.RECTILIGNE =>
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

    private static DirectionToMove Clairvoyant(Vector2Int startPos, TileData[,] map, List<PersonnalitiesV2> personalities, Aggressivity aggressivity, Objectives[] objectives)
    {
        if (aggressivity == Aggressivity.COURAGEUX)
        {
            FindTilesWithEnemies(map);
            return BFSToObjective(startPos, map, tilesWithEnemies);
        }
        
        switch (objectives.Length)
        {
            case 0:
                return GoThroughRandomOpenDoor(startPos, map);
            case 1:
                switch (objectives[0])
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
                        return BFSToObjective(startPos, map, unvisitedTiles);
                    case Objectives.SORTIE:
                        FindExits(map);
                        return BFSToObjective(startPos, map, exits);
                    default:
                        throw new System.Exception("Unknown objective");
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
                foreach (Vector2Int neighbor in neighborsWithEnemies)
                {
                    Vector2Int[] neighborsOfNeighbor = GetNeighbors(neighbor, map);
                    foreach (Vector2Int neighborOfNeighbor in neighborsOfNeighbor)
                    {
                        if (map[neighborOfNeighbor.x, neighborOfNeighbor.y].enemies.Count <= 0)
                        {
                            neighborsWithoutEnemies.Add(neighborOfNeighbor);
                        }
                    }
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
            if (enemiesInLineOfSight.Count <= 0) return DirectionToMove.None;
            int randomIndex = Random.Range(0, enemiesInLineOfSight.Count);
            return DirectionFromTo(startPos, enemiesInLineOfSight[randomIndex]);
        }
        else
        {
            FindTilesWithEnemies(map);
            List<Vector2Int> enemiesInLineOfSight = new List<Vector2Int>();
            enemiesInLineOfSight = CheckIfEnemyIsInLineOfSight(startPos, tilesInLineOfSight, tilesWithEnemies);
            if (enemiesInLineOfSight.Count > 0)
            {
                int randomIndex = Random.Range(0, enemiesInLineOfSight.Count);
                return DirectionFromTo(startPos, enemiesInLineOfSight[randomIndex]);
            }
            
            List<Vector2Int> unvisitedTilesInLineOfSight = new List<Vector2Int>();
            unvisitedTilesInLineOfSight = CheckIfEnemyIsInLineOfSight(startPos, tilesInLineOfSight, unvisitedTiles);
            if (unvisitedTilesInLineOfSight.Count > 0)
            {
                int randomIndex = Random.Range(0, unvisitedTilesInLineOfSight.Count);
                return DirectionFromTo(startPos, unvisitedTilesInLineOfSight[randomIndex]);
            }
            else
            {
                int randomIndex = Random.Range(0, enemiesInLineOfSight.Count);
                return DirectionFromTo(startPos, enemiesInLineOfSight[randomIndex]);
            }
        }
        
        // Check if there is an unexplored tile in the hero's vision
        // If there is, return the direction to move to reach the closest one
        // If there is not, return DirectionToMove.None

        return DirectionToMove.None; // Placeholder, replace with actual result
    }

    private static List<Vector2Int> CheckIfEnemyIsInLineOfSight(Vector2Int startPos, Vector2Int[] tilesInLineOfSight,
        List<Vector2Int> vector2Ints)
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
            Debug.Log("Go through random door : " + directions[randomIndex] + " because no tile on the other side");
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
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); i++)
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
        return DirectionToMove.None;
    }
    
    private static void FindExits(TileData[,] map)
    {
        exits.Clear();
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); i++)
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

    #endregion
}
