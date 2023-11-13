using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogPainter : MonoBehaviour
{
    public GameObject baseFogTile, FogTop, FogTopRight, FogRight, FogBottomRight, 
        FogBottom, FogBottomLeft, FogLeft, FogTopLeft,
        FogCornerTopRight, FogCornerBottomRight, FogCornerBottomLeft, FogCornerTopLeft,
        FogSlash, FogAntiSlash;
    public HashSet<Vector2Int> dungeonTilesPositions = new HashSet<Vector2Int>();

    

    public void Start()
    {
        DragAndDropManager.OnTilePosedEvent += ActualizeFog;
    }

    private void ActualizeFog(TileData data, CardInfoInstance instance)
    {
        //get the tile position on the array
        Vector2Int tilePos = MapManager.Instance.GetPosFromData(data);
        dungeonTilesPositions.Add(tilePos);
        FogGenerator.CreateFog(dungeonTilesPositions, this,MapManager.Instance.width-2,MapManager.Instance.height-2);
    }

    public void PaintFog(HashSet<Vector2Int> fogPositions, int width, int height)
    {
        DestoyChilds();
        foreach (var fogPosition in fogPositions)
        {
            string neighbourBinaryType = "";
            foreach (var direction in Direction2D.cardDirList)
            {
                var neighborPos = fogPosition + direction;
                neighbourBinaryType += dungeonTilesPositions.Contains(neighborPos) ? "0" : "1";
                
            }
            
            PaintSingleFog(fogPosition, neighbourBinaryType, width, height);
        }
    }

    private void PaintSingleFog(Vector2Int fogPosition, string neighbourBinaryType, int width, int height)
    {
        int typeAsInt = Convert.ToInt32(neighbourBinaryType,2);
        GameObject fogTileType = null;
        if (FogByteType.fogFull.Contains(typeAsInt)) fogTileType = baseFogTile;
        else if (FogByteType.fogTop.Contains(typeAsInt)) fogTileType = FogTop;
        else if (FogByteType.fogTopRight.Contains(typeAsInt)) fogTileType = FogTopRight;
        else if (FogByteType.fogRight.Contains(typeAsInt)) fogTileType = FogRight;
        else if (FogByteType.fogBottomRight.Contains(typeAsInt)) fogTileType = FogBottomRight;
        else if (FogByteType.fogBottom.Contains(typeAsInt)) fogTileType = FogBottom;
        else if (FogByteType.fogBottomLeft.Contains(typeAsInt)) fogTileType = FogBottomLeft;
        else if (FogByteType.fogLeft.Contains(typeAsInt)) fogTileType = FogLeft;
        else if (FogByteType.fogTopLeft.Contains(typeAsInt)) fogTileType = FogTopLeft;

        else if (FogByteType.fogCornerTopRight.Contains(typeAsInt)) fogTileType = FogCornerTopRight;
        else if (FogByteType.fogCornerBottomRight.Contains(typeAsInt)) fogTileType = FogCornerBottomRight;
        else if (FogByteType.fogCornerBottomLeft.Contains(typeAsInt)) fogTileType = FogCornerBottomLeft;
        else if (FogByteType.fogCornerTopLeft.Contains(typeAsInt)) fogTileType = FogCornerTopLeft;

        else if (FogByteType.fogSlash.Contains(typeAsInt)) fogTileType = FogSlash;
        else if (FogByteType.fogBackSlash.Contains(typeAsInt)) fogTileType = FogAntiSlash;


        else return;


        GameObject fogTile = Instantiate(fogTileType, new Vector3((fogPosition.x - (float)width / 2) + 0.5f, 0, (fogPosition.y - (float)height / 2) + 0.5f), baseFogTile.transform.rotation);
        fogTile.transform.parent = transform;
    }

    private void DestoyChilds()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}