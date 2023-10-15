using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private SpawnMap spawnMap;
    [SerializeField] private int SizeX;
    [SerializeField] private int SizeY;
    [SerializeField] private SpriteRenderer HeroToken;
    
    private MapDataCell[,] map; // true - Tile is occupied, false - Tile is free  (apres, ce sera une struct qui prend si le hero est dessus, si un ennemi est dessus, si a cote il peut marcher etc)

    struct MapDataCell
    {
        public bool isOccupied;
        public bool isWalkable;
        public bool DoorOnTop;
        public bool DoorOnBottom;
        public bool DoorOnLeft;
        public bool DoorOnRight;

        public MapDataCell(bool _ = false)
        {
            isWalkable = false;
            DoorOnTop = false;
            DoorOnBottom = false;
            DoorOnLeft = false;
            DoorOnRight = false;
            isOccupied = false;
        }
    }
    
    private void InitializeMap()
    {
        map = new MapDataCell[SizeX, SizeY];
        for (int i = 0; i < SizeX; i++)
        {
            // for (int j = 0; j < SizeY; j++)
            // {
            //     map[i, j];
            // }
        }
    }

    public bool IsTileOccupied(Vector3Int position)
    {
        return map[position.x, position.y].isOccupied;
    }
    
    public void SetTileOccupied(Vector3Int position)
    {
        map[position.x, position.y].isOccupied = true;
    }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    public void SpawnTile(Vector3Int position, TileType tileType)
    {
        spawnMap.SetTile(position, tileType);
    }

    void Start()
    {
        spawnMap.SpawnMapTile(SizeX, SizeY);
        InitializeMap();
        int posY = Random.Range(0, SizeY);
        spawnMap.SetTile(new Vector3Int(0, posY), TileType.Start);
        HeroToken.transform.position = new Vector3(0, posY);
        map[0, posY].isOccupied = true;
    }
}
