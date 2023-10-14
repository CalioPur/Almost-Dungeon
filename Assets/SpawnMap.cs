using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnMap : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private int SizeX;
    [SerializeField] private int SizeY;

    [SerializeField] private Tile[] tiles;

    // Start is called before the first frame update

    private void SpawnMapTile()
    {
        tilemap.ClearAllTiles();

        for (int i = 0; i < SizeX; i++)
        {
            tilemap.SetTile(new Vector3Int(i, 0, 0), tiles[0]);
            tilemap.SetTile(new Vector3Int(i, SizeY - 1, 0), tiles[0]);
        }

        for (int i = 0; i < SizeY; i++)
        {
            tilemap.SetTile(new Vector3Int(0, i, 0), tiles[1]);
            tilemap.SetTile(new Vector3Int(SizeX - 1, i, 0), tiles[1]);
        }
        
        
        for (int i = 1; i < SizeX - 1; i++)
        {
            for (int j = 1; j < SizeY - 1; j++)
            {
                tilemap.SetTile(new Vector3Int(i, j, 0), tiles[2]);
            }
        }
    }


    void Start()
    {
        SpawnMapTile();
    }
}