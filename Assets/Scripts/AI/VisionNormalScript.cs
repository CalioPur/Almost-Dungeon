using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class VisionNormalScript
{
    public static List<TileData> GetVisibleTiles(Vector2Int startPos)
    {
        List<TileData> visibleTiles = new List<TileData>();
        TileData[,] map = MapManager.Instance.mapArray;
        Vector2Int simulatedPos = startPos;
        
        int debug = 0;
        
        visibleTiles.Add(map[simulatedPos.x, simulatedPos.y]);

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
        while (simulatedPos.x <= map.GetLength(0)  && map[simulatedPos.x, simulatedPos.y].hasDoorRight)
        {
            simulatedPos.x += 1;
            visibleTiles.Add(map[simulatedPos.x, simulatedPos.y]);
            MapManager.Instance.mapArray[simulatedPos.x, simulatedPos.y].IsVisited = true;
            debug++;
        }

        visibleTiles.AddRange(MapManager.Instance.mapArray.Cast<TileData>().Where(VARIABLE => VARIABLE.isConnectedToPath && !VARIABLE.IsVisited));
        return visibleTiles;
    }
}
