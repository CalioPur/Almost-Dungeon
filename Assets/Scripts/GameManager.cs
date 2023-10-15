using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public bool DoorOnTop;
        public bool DoorOnBottom;
        public bool DoorOnLeft;
        public bool DoorOnRight;

        public MapDataCell(bool _ = false)
        {
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
    }

    public bool IsTileCanBeOccupied(Vector3Int position, List<bool> doorsOpenAndClose)
    {
        if (position.x < 0 || position.x >= SizeX || position.y < 0 || position.y >= SizeY) return false;
        if (map[position.x, position.y].isOccupied) return false;
        
        //Check if your door is open and the door of the tile you want to go is open or it's a void
        if (position.y + 1 < SizeY && map[position.x, position.y + 1].isOccupied &&doorsOpenAndClose[0] != map[position.x, position.y + 1].DoorOnBottom) return false;
        if (position.y - 1 >= 0 && map[position.x, position.y - 1].isOccupied && doorsOpenAndClose[1] != map[position.x, position.y - 1].DoorOnTop) return false;
        if (position.x - 1 >= 0 && map[position.x - 1, position.y].isOccupied && doorsOpenAndClose[2] != map[position.x - 1, position.y].DoorOnRight) return false;
        if (position.x + 1 < SizeX && map[position.x + 1, position.y].isOccupied && doorsOpenAndClose[3] != map[position.x + 1, position.y].DoorOnLeft) return false;
        //
        
        // check if the tile you want to place is not blocking a door
        if (position.y + 1 < SizeY && map[position.x, position.y + 1].isOccupied && map[position.x, position.y + 1].DoorOnBottom && !doorsOpenAndClose[0]) return false;
        if (position.y - 1 >= 0 && map[position.x, position.y - 1].isOccupied && map[position.x, position.y - 1].DoorOnTop && !doorsOpenAndClose[1]) return false;
        if (position.x - 1 >= 0 && map[position.x - 1, position.y].isOccupied && map[position.x - 1, position.y].DoorOnRight && !doorsOpenAndClose[2]) return false;
        if (position.x + 1 < SizeX && map[position.x + 1, position.y].isOccupied && map[position.x + 1, position.y].DoorOnLeft && !doorsOpenAndClose[3]) return false;
        //
        
        return true;
    }
    
    public void OccupiedTile(Vector3Int position, List<bool> doorsOpenAndClose)
    {
        map[position.x, position.y].isOccupied = true;
        map[position.x, position.y].DoorOnTop = doorsOpenAndClose[0];
        map[position.x, position.y].DoorOnBottom = doorsOpenAndClose[1];
        map[position.x, position.y].DoorOnLeft = doorsOpenAndClose[2];
        map[position.x, position.y].DoorOnRight = doorsOpenAndClose[3];
        
    }
    
    public void SpawnTile(Vector3Int position, TileType tileType)
    {
        spawnMap.SetTile(position, tileType);
    }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    public List<bool> GetDoorAtPosition(Vector2Int position)
    {
        if (position.x < 0 || position.x >= SizeX || position.y < 0 || position.y >= SizeY) return null;
        return new List<bool>()
        {
            map[position.x, position.y].DoorOnTop,
            map[position.x, position.y].DoorOnBottom,
            map[position.x, position.y].DoorOnLeft,
            map[position.x, position.y].DoorOnRight
        };
    }
    
    void Start()
    {
        spawnMap.SpawnMapTile(SizeX, SizeY);
        InitializeMap();
        int posY = Random.Range(0, SizeY);
        spawnMap.SetTile(new Vector3Int(0, posY), TileType.Start);
        HeroMovement.Instance.SetPosition(new Vector2Int(0, posY));
        map[0, posY].isOccupied = true;
        map[0, posY].DoorOnRight = true;
        SetHeroPosition(new Vector2Int(0, posY));
    }


    public void SetHeroPosition(Vector2Int position)
    {
        HeroToken.transform.position = new Vector3(position.x, position.y);
        if (position.x < 0 || position.x >= SizeX || position.y < 0 || position.y >= SizeY)
        {
            Debug.Log("Hero is out of the map");
            Time.timeScale = 0;
            return;
        }
        if (!map[position.x, position.y].isOccupied)
        {
            Debug.Log("Hero is not on a tile");
            Time.timeScale = 0;
            return;
        }
    }
}
