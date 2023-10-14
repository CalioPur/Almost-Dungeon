using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType
{
    HorizontalWall = 0,
    VerticalWall,
    Hole,
    Start,
    End,
    SimpleLine,
    LLine,
    TLine,
    XLine,
    FaceDeck,
    ERROR = -1,
}



public class SpawnMap : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private MapStruct[] maps;
    
    [Serializable]
    private struct MapStruct
    {
        public TileType type;
        public Tile tile;
    }
    
    private Dictionary<TileType, Tile> tileDictionary = new ();

    private void Awake()
    {
        foreach (var map in maps)
        {
            tileDictionary.Add(map.type, map.tile);
        }
    }

    public void SpawnMapTile(int SizeX, int SizeY)
    {
        //tilemap.ClearAllTiles();
        Tile[] tiles = new Tile[tileDictionary.Count];
        tileDictionary.Values.CopyTo(tiles, 0);

        for (int i = -1; i <= SizeX; i++)
        {
            tilemap.SetTile(new Vector3Int(i, -1, 0), tiles[(int)TileType.HorizontalWall]);
            tilemap.SetTile(new Vector3Int(i, SizeY, 0), tiles[(int)TileType.HorizontalWall]);
        }

        for (int i = -1; i <= SizeY; i++)
        {
            tilemap.SetTile(new Vector3Int(-1, i, 0), tiles[(int)TileType.VerticalWall]);
            tilemap.SetTile(new Vector3Int(SizeX, i, 0), tiles[(int)TileType.VerticalWall]);
        }
        
        
        for (int i = 0; i < SizeX; i++)
        {
            for (int j = 0; j < SizeY; j++)
            {
                tilemap.SetTile(new Vector3Int(i, j, 0), tiles[(int)TileType.Hole]);
            }
        }
    }
    
    public void SetTile(Vector3Int position, TileType type)
    {
        tilemap.SetTile(position, tileDictionary[type]);
    }
    
    public void SetTile(Vector3Int position, Tile type)
    {
        tilemap.SetTile(position, type);
    }
}