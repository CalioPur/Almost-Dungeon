using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RageScript
{
    public static void Rage(Vector2Int getIndexHeroPos)
    {
        int radius = 3;
        for (int i = 0; i < MapManager.Instance.mapArray.GetLength(0); i++)
        {
            for (int j = 0; j < MapManager.Instance.mapArray.GetLength(1); j++)
            {
                if (Vector2Int.Distance(new Vector2Int(i, j), getIndexHeroPos) <= radius)
                {
                    MapManager.Instance.mapArray[i, j].ResetData();
                }
            }
        }
        
    }
}
