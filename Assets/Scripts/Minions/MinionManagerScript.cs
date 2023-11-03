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
            Vector3 posToSpawn = tile.transform.position;
            
            GameObject minionPrefab = TrapsPrefab.Find(x => x.name == "basicMinion").prefab;

            GameObject minion = Instantiate(minionPrefab, posToSpawn, minionPrefab.transform.rotation, transform);
            MinionData minionData = minion.GetComponent<MinionData>();
            mapManager.GetTilePosFromWorldPos(posToSpawn, out minionData.indexMinionX, out minionData.indexMinionY);
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
            mapManager.GetTilePosFromWorldPos(posToSpawn, out trapData.indexMinionX, out trapData.indexMinionY);
            trapData.mapManager = mapManager;
            tile.enemies.Add(trapData);
            trapData.StartListenTick(MovementType.Trap);
        }
        
    }
}
