using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
    public static DirectionToMove BFSGoToClosestExit(Vector2Int startPos, TileData[,] map)
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
                    DrawPathLine(nextPos, parentMap[nextPos]); // Draw path as it is reconstructed
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

        Debug.Log("BFS iterations: " + dbgInt);
        if (exits.Count > 0)
        {
            int randomIndex = Random.Range(0, exits.Count);
            Vector2Int exitPos = exits[randomIndex];
            Vector2Int nextPos = exitPos;
            while (parentMap.ContainsKey(nextPos) && parentMap[nextPos] != startPos)
            {
                nextPos = parentMap[nextPos];
            }

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
            
        }

        Debug.Log("No valid path found because no exit found");
        return GoThroughDoorWithNoTile(startPos, map);
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


    // private static Vector2Int[] GetNeighbors(Vector2Int pos, TileData[,] map)
    // {
    //     int width = map.GetLength(0);
    //     int height = map.GetLength(1);
    //
    //     List<Vector2Int> neighbors = new List<Vector2Int>();
    //
    //     if (map[pos.x, pos.y].isExit)
    //     {
    //         // Connect exit tile with all adjacent tiles
    //         if (pos.x > 0) neighbors.Add(new Vector2Int(pos.x - 1, pos.y)); // Left
    //         if (pos.x < width - 1) neighbors.Add(new Vector2Int(pos.x + 1, pos.y)); // Right
    //         if (pos.y > 0) neighbors.Add(new Vector2Int(pos.x, pos.y - 1)); // Down
    //         if (pos.y < height - 1) neighbors.Add(new Vector2Int(pos.x, pos.y + 1)); // Up
    //     }
    //     else
    //     {
    //         // Connect tiles with doors
    //         if (pos.x > 0 && map[pos.x - 1, pos.y].hasDoorRight || map[pos.x, pos.y].isExit)
    //         {
    //             neighbors.Add(new Vector2Int(pos.x - 1, pos.y));
    //             //draw a blue line when there is a door towards the left
    //             Debug.DrawLine(new Vector3(pos.x - 3.5f, 0, pos.y - 2.5f),
    //                 new Vector3(pos.x - 3.5f, 0, pos.y - 2.5f) + Vector3.left * 0.4f, Color.blue, 3f); // Left
    //         } // Left
    //         else if (pos.x > 0 && !map[pos.x - 1, pos.y].hasDoorRight && !map[pos.x, pos.y].isExit)
    //         {
    //             //draw a red line when there is no door
    //             Debug.DrawLine(new Vector3(pos.x - 3.5f, 0, pos.y - 2.5f),
    //                 new Vector3(pos.x - 3.5f, 0, pos.y - 2.5f) + Vector3.left * 0.4f, Color.red, 3f); // Left
    //         }
    //
    //         if (pos.x < width - 1 && map[pos.x + 1, pos.y].hasDoorLeft || map[pos.x, pos.y].isExit)
    //         {
    //             neighbors.Add(new Vector2Int(pos.x + 1, pos.y));
    //             //draw a blue line when there is a door towards the right
    //             Debug.DrawLine(new Vector3(pos.x - 3.5f, 0, pos.y - 2.5f),
    //                 new Vector3(pos.x - 3.5f, 0, pos.y - 2.5f) + Vector3.right * 0.4f, Color.blue, 3f); // Right
    //         } // Right
    //         else if (pos.x < width - 1 && !map[pos.x + 1, pos.y].hasDoorLeft && !map[pos.x, pos.y].isExit)
    //         {
    //             //draw a red line when there is no door
    //             Debug.DrawLine(new Vector3(pos.x - 3.5f, 0, pos.y - 2.5f),
    //                 new Vector3(pos.x - 3.5f, 0, pos.y - 2.5f) + Vector3.right * 0.4f, Color.red, 3f); // Right
    //         }
    //
    //         if (pos.y > 0 && map[pos.x, pos.y - 1].hasDoorUp || map[pos.x, pos.y].isExit)
    //         {
    //             neighbors.Add(new Vector2Int(pos.x, pos.y - 1));
    //             //draw a blue line when there is a door towards the bottom
    //             Debug.DrawLine(new Vector3(pos.x - 3.5f, 0, pos.y - 2.5f),
    //                 new Vector3(pos.x - 3.5f, 0, pos.y - 2.5f) + Vector3.back * 0.4f, Color.blue, 3f); // Down
    //         } // Down
    //         else if (pos.y > 0 && !map[pos.x, pos.y - 1].hasDoorUp && !map[pos.x, pos.y].isExit)
    //         {
    //             //draw a red line when there is no door
    //             Debug.DrawLine(new Vector3(pos.x - 3.5f, 0, pos.y - 2.5f),
    //                 new Vector3(pos.x - 3.5f, 0, pos.y - 2.5f) + Vector3.back * 0.4f, Color.red, 3f); // Down
    //         }
    //
    //         if (pos.y < height - 1 && map[pos.x, pos.y + 1].hasDoorDown || map[pos.x, pos.y].isExit)
    //         {
    //             neighbors.Add(new Vector2Int(pos.x, pos.y + 1));
    //             //draw a blue line when there is a door towards the top
    //             Debug.DrawLine(new Vector3(pos.x - 3.5f, 0, pos.y - 2.5f),
    //                 new Vector3(pos.x - 3.5f, 0, pos.y - 2.5f) + Vector3.forward * 0.4f, Color.blue, 3f); // Up
    //         } // Up
    //         else if (pos.y < height - 1 && !map[pos.x, pos.y + 1].hasDoorDown && !map[pos.x, pos.y].isExit)
    //         {
    //             //draw a red line when there is no door
    //             Debug.DrawLine(new Vector3(pos.x - 3.5f, 0, pos.y - 2.5f),
    //                 new Vector3(pos.x - 3.5f, 0, pos.y - 2.5f) + Vector3.forward * 0.4f, Color.red, 3f); // Up
    //         }
    //
    //         // else
    //         // {
    //         //     //draw a red line when there is no door
    //         //     if (pos.x > 0)
    //         //     {
    //         //         Debug.DrawLine(new Vector3(pos.x - 3.5f, 0, pos.y - 2.5f),
    //         //             new Vector3(pos.x - 3.5f, 0, pos.y - 2.5f) + Vector3.left * 0.4f, Color.red, 1f);
    //         //     } // Left
    //         //
    //         //     if (pos.x < width - 1)
    //         //     {
    //         //         Debug.DrawLine(new Vector3(pos.x - 3.5f, 0, pos.y - 2.5f),
    //         //             new Vector3(pos.x - 3.5f, 0, pos.y - 2.5f) + Vector3.right * 0.4f, Color.red, 1f);
    //         //     } // Right
    //         //
    //         //     if (pos.y > 0)
    //         //     {
    //         //         Debug.DrawLine(new Vector3(pos.x - 3.5f, 0, pos.y - 2.5f),
    //         //             new Vector3(pos.x - 3.5f, 0, pos.y - 2.5f) + Vector3.back * 0.4f, Color.red, 1f);
    //         //     } // Down
    //         //
    //         //     if (pos.y < height - 1)
    //         //     {
    //         //         Debug.DrawLine(new Vector3(pos.x - 3.5f, 0, pos.y - 2.5f),
    //         //             new Vector3(pos.x - 3.5f, 0, pos.y - 2.5f) + Vector3.forward * 0.4f, Color.red, 1f);
    //         //     } // Up
    //         // }
    //     }
    //
    //     return neighbors.ToArray();
    // }


    private static void DrawPathLine(Vector2Int start, Vector2Int end)
    {
        Vector3 startPos = new Vector3(start.x - 3.5f, 0, start.y - 2.5f);
        Vector3 endPos = new Vector3(end.x - 3.5f, 0, end.y - 2.5f);
        Debug.DrawLine(startPos, endPos, Color.green, 1f);
    }
}