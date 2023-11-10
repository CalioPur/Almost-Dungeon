using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FogGenerator
{
    public static void CreateFog(HashSet<Vector2Int> dungeonTilesPositions,FogPainter fogPainter, int width, int height)
    {
        var basicFogPosition = FindFogInDirection(dungeonTilesPositions, width,height);
        fogPainter.PaintFog(basicFogPosition, width, height);
    }

    private static HashSet<Vector2Int> FindFogInDirection(HashSet<Vector2Int> dungeonTilesPositions, int width, int height)
    {
        Debug.Log(dungeonTilesPositions.Count);
        HashSet<Vector2Int> fogPositions = new HashSet<Vector2Int>();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2Int currentPosition = new Vector2Int(i, j);
                if(!dungeonTilesPositions.Contains(currentPosition))
                {
                    fogPositions.Add(currentPosition);
                }
            }
        }
        return fogPositions;
    }
}
