using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SeerScript
{
    public static List<TileData> GetAllConnectedToPathTiles(Vector2Int getIndexHeroPos)
    {
        TileData[,] map = MapManager.Instance.mapArray;
        return map.Cast<TileData>().Where(VARIABLE => VARIABLE.isConnectedToPath).ToList();
    }
}
