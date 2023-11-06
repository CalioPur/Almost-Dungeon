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

    private void SpawnMinionOnTile(TileData tile, CardInfoInstance card)
    {
        for (int i = 0; i < card.So.nbMinionOnCard; i++)
        {
            var angle = i * Mathf.PI * 2 / card.So.nbMinionOnCard;
            if (card.So.nbMinionOnCard != 1) angle -= Mathf.PI / 4;

            SpawnEnemy<MinionData>(TrapsPrefab.Find(x => x.name == "basicMinion").prefab, tile, true,
                new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * 0.3f);
            
            
        }

        if (card.So.Web)
        {
            SpawnEnemy<TrapData>(TrapsPrefab.Find(x => x.name == "web").prefab, tile, true, Vector3.zero);
        }

        if (card.So.Pyke)
        {
            SpawnEnemy<TrapData>(TrapsPrefab.Find(x => x.name == "pyke").prefab, tile, false, Vector3.zero);
        }
    }

    private void SpawnEnemy<T>(GameObject prefab, TileData tile, bool addEnemyOnTile, Vector3 positionOffset)
        where T : TrapData
    {
        Vector3 position = tile.transform.position + positionOffset;
        GameObject minion = Instantiate(prefab, position, prefab.transform.rotation);
        T script = minion.GetComponent<T>();
        mapManager.GetTilePosFromWorldPos(position, out script.indexX, out script.indexY);
        script.mapManager = mapManager;
        if (addEnemyOnTile)
            tile.enemies.Add(script);
    }
}