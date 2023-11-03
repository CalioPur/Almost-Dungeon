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

public class SpawnEnemyManager : MonoBehaviour
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
            Vector3 posToSpawn = tile.transform.position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * 0.3f;
            ;
            GameObject minion = Instantiate(minionPrefab, posToSpawn, minionPrefab.transform.rotation, transform);
            MinionData minionData = minion.GetComponent<MinionData>();
            mapManager.GetTilePosFromWorldPos(posToSpawn, out minionData.indexX, out minionData.indexY);
            minionData.mapManager = mapManager;
            tile.enemies.Add(minionData);
            minionData.StartListenTick(MovementType.Monster);
        }

        if (card.So.Web)
        {
            SpawnEnemy<TrapData>(TrapsPrefab.Find(x => x.name == "web").prefab, tile, true);
        }

        if (card.So.Pyke)
        {
            SpawnEnemy<TrapData>(TrapsPrefab.Find(x => x.name == "pyke").prefab, tile, false);

        }
    }

    public void SpawnEnemy<T>(GameObject prefab, TileData tile, bool addEnemyOnTile) where T : TrapData
    {
        Vector3 position = tile.transform.position;
        GameObject minion = Instantiate(prefab, position, prefab.transform.rotation, transform);
        T script = minion.GetComponent<T>();
        mapManager.GetTilePosFromWorldPos(position, out script.indexX, out script.indexY);
        script.mapManager = mapManager;
        if (addEnemyOnTile)
            tile.enemies.Add(script);
    }
}