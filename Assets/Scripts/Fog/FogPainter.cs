using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogPainter : MonoBehaviour
{
    public GameObject baseFogTile, FogTop, FogTopRight, FogRight, FogBottomRight,
        FogBottom, FogBottomLeft, FogLeft, FogTopLeft,
        FogCornerTopRight, FogCornerBottomRight, FogCornerBottomLeft, FogCornerTopLeft,
        FogSlash, FogAntiSlash,
        FogEncapsulated,
        FogTopEncap, FogRightEncap, FogBottomEncap, FogLeftEncap,
        FogTopBottom, FogRightLeft,
        FogDoubleCornerTop, FogDoubleCornerRight, FogDoubleCornerBottom, FogDoubleCornerLeft,
        FogTripleCornerTopLeft, FogTripleCornerTopRight, FogTripleCornerBottomRight, FogTripleCornerBottomLeft,
        FogQuad,
        FogTurnTopLeft, FogTurnTopRight, FogTurnBottomRight, FogTurnBottomLeft,

        CornerLeftBottom, CornerLeftLeft, CornerLeftTop, CornerLeftRight,
        CornerRightBottom, CornerRightLeft, CornerRightTop, CornerRightRight,
        DoubleCornerBottom, DoubleCornerLeft, DoubleCornerTop, DoubleCornerRight;
        
    public HashSet<Vector2Int> dungeonTilesPositions = new HashSet<Vector2Int>();

    

    public void OnEnable()
    {
        PlayerCardController.OnTilePosedEvent += ActualizeFog;
    }
    
    public void OnDisable()
    {
        PlayerCardController.OnTilePosedEvent -= ActualizeFog;
    }

    private void ActualizeFog(TileData data, CardInfoInstance instance)
    {
        //get the tile position on the array
        Vector2Int tilePos = MapManager.Instance.GetPosFromData(data);
        dungeonTilesPositions.Add(tilePos);
        FogGenerator.CreateFog(dungeonTilesPositions, this,MapManager.Instance.width,MapManager.Instance.height);
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
        if(!dungeonTilesPositions.Contains(fogPosition)) fogTileType = baseFogTile;
        // si on est pas sur une tile de dongeon, pas besoin d'un fog particulier
        else if (FogByteType.fogEncapsulated.Contains(typeAsInt)) fogTileType = FogEncapsulated;
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

        else if (FogByteType.fogTopEncapsulated.Contains(typeAsInt)) fogTileType = FogTopEncap;
        else if (FogByteType.fogRightEncapsulated.Contains(typeAsInt)) fogTileType = FogRightEncap;
        else if (FogByteType.fogBottomEncapsulated.Contains(typeAsInt)) fogTileType = FogBottomEncap;
        else if (FogByteType.fogLeftEncapsulated.Contains(typeAsInt)) fogTileType = FogLeftEncap;

        else if (FogByteType.fogTopBottom.Contains(typeAsInt)) fogTileType = FogTopBottom;
        else if (FogByteType.fogRightLeft.Contains(typeAsInt)) fogTileType = FogRightLeft;

        else if (FogByteType.fogDoubleCornerBottom.Contains(typeAsInt)) fogTileType = FogDoubleCornerBottom;
        else if (FogByteType.fogDoubleCornerLeft.Contains(typeAsInt)) fogTileType = FogDoubleCornerLeft;
        else if (FogByteType.fogDoubleCornerTop.Contains(typeAsInt)) fogTileType = FogDoubleCornerTop;
        else if (FogByteType.fogDoubleCornerRight.Contains(typeAsInt)) fogTileType = FogDoubleCornerRight;

        else if (FogByteType.fogTripleCornerBottomLeft.Contains(typeAsInt)) fogTileType = FogTripleCornerBottomLeft;
        else if (FogByteType.fogTripleCornerTopLeft.Contains(typeAsInt)) fogTileType = FogTripleCornerTopLeft;
        else if (FogByteType.fogTripleCornerTopRight.Contains(typeAsInt)) fogTileType = FogTripleCornerTopRight;
        else if (FogByteType.fogTripleCornerBottomRight.Contains(typeAsInt)) fogTileType = FogTripleCornerBottomRight;

        else if (FogByteType.fogQuadCorner.Contains(typeAsInt)) fogTileType = FogQuad;

        else if (FogByteType.fogTurnTopLeft.Contains(typeAsInt)) fogTileType = FogTurnTopLeft;
        else if (FogByteType.fogTurnTopRight.Contains(typeAsInt)) fogTileType = FogTurnTopRight;
        else if (FogByteType.fogTurnBottomRight.Contains(typeAsInt)) fogTileType = FogTurnBottomRight;
        else if (FogByteType.fogTurnBottomLeft.Contains(typeAsInt)) fogTileType = FogTurnBottomLeft;

        else if (FogByteType.cornerLeftBottom.Contains(typeAsInt)) fogTileType = CornerLeftBottom;
        else if (FogByteType.cornerLeftLeft.Contains(typeAsInt)) fogTileType = CornerLeftLeft;
        else if (FogByteType.cornerLeftTop.Contains(typeAsInt)) fogTileType = CornerLeftTop;
        else if (FogByteType.cornerLeftRight.Contains(typeAsInt)) fogTileType = CornerLeftRight;

        else if (FogByteType.cornerRightBottom.Contains(typeAsInt)) fogTileType = CornerRightBottom;
        else if (FogByteType.cornerRightLeft.Contains(typeAsInt)) fogTileType = CornerRightLeft;
        else if (FogByteType.cornerRightTop.Contains(typeAsInt)) fogTileType = CornerRightTop;
        else if (FogByteType.cornerRightRight.Contains(typeAsInt)) fogTileType = CornerRightRight;

        else if (FogByteType.doubleCornerBottom.Contains(typeAsInt)) fogTileType = DoubleCornerBottom;
        else if (FogByteType.doubleCornerLeft.Contains(typeAsInt)) fogTileType = DoubleCornerLeft;
        else if (FogByteType.doubleCornerTop.Contains(typeAsInt)) fogTileType = DoubleCornerTop;
        else if (FogByteType.doubleCornerRight.Contains(typeAsInt)) fogTileType = DoubleCornerRight;


        else return;


        GameObject fogTile = Instantiate(fogTileType, new Vector3((fogPosition.x - (float)width / 2) + 0.5f, 0.01f, (fogPosition.y - (float)height / 2) + 0.5f), baseFogTile.transform.rotation);
        fogTile.transform.parent = transform;
    }

    private void DestoyChilds()
    {
        if (transform)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            } 
        }
        else
        {
            Destroy(gameObject);
        }
    }
}