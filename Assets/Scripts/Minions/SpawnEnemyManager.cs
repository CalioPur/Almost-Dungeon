using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ListOfTraps
{
    public string name;
    public GameObject prefab;
}

public class MinionManagerScript : MonoBehaviour
{
    [SerializeField] private List<ListOfTraps> TrapsPrefab;
    [SerializeField] private MapManager mapManager;

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
            
            
            GameObject minionPrefab = TrapsPrefab.Find(x => x.name == "basicMinion").prefab;

            
            var pos = new Vector3 (Mathf.Cos(angle), 0, Mathf.Sin(angle)) * 0.3f;
            
            Vector3 posToSpawn = tile.transform.position+ pos;

            GameObject minion = Instantiate(minionPrefab, posToSpawn, minionPrefab.transform.rotation, transform);
            MinionData minionData = minion.GetComponent<MinionData>();
            mapManager.GetTilePosFromWorldPos(posToSpawn, out minionData.indexX, out minionData.indexY);
            minionData.mapManager = mapManager;
            tile.enemies.Add(minionData);
            minionData.StartListenTick(MovementType.Monster);
        }

        if (card.So.Web)
        {
            Vector3 posToSpawn = tile.transform.position;
            GameObject webPrefab = TrapsPrefab.Find(x => x.name == "web").prefab;
            GameObject minion = Instantiate(webPrefab, posToSpawn, webPrefab.transform.rotation, transform);
            TrapData trapData = minion.GetComponent<TrapData>();
            mapManager.GetTilePosFromWorldPos(posToSpawn, out trapData.indexX, out trapData.indexY);
            trapData.mapManager = mapManager;
            tile.enemies.Add(trapData);
            trapData.StartListenTick(MovementType.Trap);
        }
        
    }
}
