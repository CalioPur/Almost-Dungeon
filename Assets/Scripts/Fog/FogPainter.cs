using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogPainter : MonoBehaviour
{
    public GameObject baseFogTile;
    

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