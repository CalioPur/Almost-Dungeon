using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionManagerScript : MonoBehaviour
{
    public GameObject minionPrefab;
    public MapManager mapManager;

    private void Start()
    {
        DragAndDropManager.OnTilePosedEvent += SpawnMinionOnTile;
    }

    public void SpawnMinionOnTile(TileData tile, CardInfoInstance card)
    {
        for (int i = 0; i < card.So.nbMinionOnCard; i++)
        {
            Vector3 posToSpawn = tile.transform.position;

            GameObject minion = Instantiate(minionPrefab, posToSpawn, minionPrefab.transform.rotation, transform);
            MinionData minionData = minion.GetComponent<MinionData>();
            mapManager.GetTilePosFromWorldPos(posToSpawn, out minionData.indexMinionX, out minionData.indexMinionY);
            minionData.mapManager = mapManager;
            tile.minions.Add(minionData);
            minionData.StartListenTick();
        }
        
    }
}
