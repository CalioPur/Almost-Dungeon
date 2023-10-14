using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SpawnMap spawnMap;
    [SerializeField] private int SizeX;
    [SerializeField] private int SizeY;
    [SerializeField] private SpriteRenderer HeroToken;
    
    private bool[,] map; // true - Tile is occupied, false - Tile is free  (apres, ce sera une struct qui prend si le hero est dessus, si un ennemi est dessus, si a cote il peut marcher etc)
    
    private void InitializeMap()
    {
        map = new bool[SizeX, SizeY];
        for (int i = 0; i < SizeX; i++)
        {
            for (int j = 0; j < SizeY; j++)
            {
                map[i, j] = false;
            }
        }
    }
    
    
    void Start()
    {
        spawnMap.SpawnMapTile(SizeX, SizeY);
        InitializeMap();
        int posY = Random.Range(0, SizeY);
        spawnMap.SetTile(new Vector3Int(0, posY), TileType.Start);
        HeroToken.transform.position = new Vector3(0, posY);
        map[0, posY] = true;
    }
}
