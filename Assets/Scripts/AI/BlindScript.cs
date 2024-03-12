using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BlindScript
{
    public static List<TileData> GetAdjacentTiles(Vector2Int pos)
    {
        var currentMap = MapManager.Instance.mapArray;
        List<TileData> adjacentTiles = new List<TileData>();

        int[,] directions = { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };
        adjacentTiles.Add(MapManager.Instance.GetTileDataAtPosition(pos.x, pos.y));

        for (int i = 0; i < directions.GetLength(0); i++)
        {
            int newX = pos.x + directions[i, 0];
            int newY = pos.y + directions[i, 1];

            if (newX >= 0 && newX < currentMap.GetLength(0) && newY >= 0 && newY < currentMap.GetLength(1))
            {
                if ((i == 0 && MapManager.Instance.GetTileDataAtPosition(pos.x, pos.y).hasDoorUp) ||
                    (i == 1 && MapManager.Instance.GetTileDataAtPosition(pos.x, pos.y).hasDoorDown) ||
                    (i == 2 && MapManager.Instance.GetTileDataAtPosition(pos.x, pos.y).hasDoorRight) ||
                    (i == 3 && MapManager.Instance.GetTileDataAtPosition(pos.x, pos.y).hasDoorLeft))
                    adjacentTiles.Add(MapManager.Instance.GetTileDataAtPosition(newX, newY));
            }
        }

        return adjacentTiles;
    }


}
