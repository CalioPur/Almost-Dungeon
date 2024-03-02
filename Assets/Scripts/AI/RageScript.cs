using System;
using UnityEngine;

public static class RageScript
{
    public static event Action OnNoPathFound;
    public static void Rage(Vector2Int getIndexHeroPos)
    {
        SoundManagerIngame.Instance.PlaySound(EmoteType.WallBreak);
        OnNoPathFound?.Invoke();
        int radius = 3;
        TileData breakedTile = MapManager.Instance.GetTileDataAtPosition(getIndexHeroPos.x, getIndexHeroPos.y);
        // if(getIndexHeroPos.x != 0) breakedTile.hasDoorLeft = true;
        // if(getIndexHeroPos.x != MapManager.Instance.mapArray.GetLength(0) - 1) breakedTile.hasDoorRight = true;
        // if(getIndexHeroPos.y != 0) breakedTile.hasDoorDown = true;
        // if(getIndexHeroPos.y != MapManager.Instance.mapArray.GetLength(1) - 1) breakedTile.hasDoorUp = true;
        int startX = getIndexHeroPos.x - radius;
        int startY = getIndexHeroPos.y - radius;
        int endX = getIndexHeroPos.x + radius;
        int endY = getIndexHeroPos.y + radius;
        
        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                if (x >= 0 && x < MapManager.Instance.mapArray.GetLength(0) && y >= 0 && y < MapManager.Instance.mapArray.GetLength(1))
                {
                    if (x != getIndexHeroPos.x || y != getIndexHeroPos.y) MapManager.Instance.ChangeTileDataAtPosition(x, y);
                }
            }
        }
        
        MapManager.Instance.MapManagerTools.ResetConnectedToPath(new Vector2Int(getIndexHeroPos.x, getIndexHeroPos.y));
    }
}
