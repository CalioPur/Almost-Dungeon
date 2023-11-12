using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogPainter : MonoBehaviour
{
    public GameObject baseFogTile;
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
            GameObject fogTile = Instantiate(baseFogTile, new Vector3((fogPosition.x-(float)width/2)+0.5f, 0, (fogPosition.y-(float)height/2)+0.5f), baseFogTile.transform.rotation);
            fogTile.transform.parent = transform;
        }
    }
    
    private void DestoyChilds()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}