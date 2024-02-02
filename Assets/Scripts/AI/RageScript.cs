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
        breakedTile.hasDoorDown = true;
        breakedTile.hasDoorLeft = true;
        breakedTile.hasDoorRight = true;
        breakedTile.hasDoorUp = true;
        for (int i = 0; i < MapManager.Instance.mapArray.GetLength(0); i++)
        {
            for (int j = 0; j < MapManager.Instance.mapArray.GetLength(1); j++)
            {
                if (i <= getIndexHeroPos.x + radius && i >= getIndexHeroPos.x - radius &&
                    j <= getIndexHeroPos.y + radius && j >= getIndexHeroPos.y - radius && 
                    i != getIndexHeroPos.x && j != getIndexHeroPos.y)
                {
                    MapManager.Instance.ChangeTileDataAtPosition(getIndexHeroPos.x, getIndexHeroPos.y, breakedTile);
                }
            }
        }
        
    }
}
