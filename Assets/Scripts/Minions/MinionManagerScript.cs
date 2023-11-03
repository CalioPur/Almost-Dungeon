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
            //display in circle around the tile
            
            var angle = i * Mathf.PI * 2 / card.So.nbMinionOnCard;
            if (card.So.nbMinionOnCard != 1)
            {
                angle -= Mathf.PI / 4;
            }

            
            var pos = new Vector3 (Mathf.Cos(angle), 0, Mathf.Sin(angle)) * 0.3f;
            
            Vector3 posToSpawn = tile.transform.position+ pos;

            GameObject minion = Instantiate(minionPrefab, posToSpawn, minionPrefab.transform.rotation, transform);
            MinionData minionData = minion.GetComponent<MinionData>();
            mapManager.GetTilePosFromWorldPos(posToSpawn, out minionData.indexMinionX, out minionData.indexMinionY);
            minionData.mapManager = mapManager;
            tile.minions.Add(minionData);
            minionData.StartListenTick();
        }
        
    }
}
