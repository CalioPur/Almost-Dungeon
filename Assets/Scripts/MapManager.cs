using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private SpawnMap spawnMap;
    [SerializeField] private int SizeX;
    [SerializeField] private int SizeY;
    
    
    // Start is called before the first frame update
    void Start()
    {
        spawnMap.SpawnMapTile(SizeX, SizeY);
        spawnMap.SetTile(new Vector3Int(0, Random.Range(0, SizeY)), TileType.Start);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
