using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum TrapType
{
    Web,
    Pyke,
    BasicCaC,
    Archer,
    Skeleton,
    None
}

[Serializable]
public struct ListOfTraps
{
    public TrapType type;
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
        int nbEnemyBasic = 0;
        int nbEnemyArcher = 0;
        int nbEnemySkeleton = 0;
        bool web = false;
        bool pyke = false;
        
        foreach (var typeOfTrapOrEnemy in card.So.TypeOfTrapOrEnemyToSpawn)
        {
            switch (typeOfTrapOrEnemy)
            {
                case TrapType.BasicCaC:
                    nbEnemyBasic++;
                    break;
                case TrapType.Archer:
                    nbEnemyArcher++;
                    break;
                case TrapType.Skeleton:
                    nbEnemySkeleton++;
                    break;
                case TrapType.Web:
                    web = true;
                    break;
                case TrapType.Pyke:
                    pyke = true;
                    break;
            }
        }
       
        for (int i = 0; i < nbEnemyBasic; i++)
        {
            var angle = i * Mathf.PI * 2 / nbEnemyBasic;
            if (nbEnemyBasic != 1) angle -= Mathf.PI / 4;

            SpawnEnemy<MinionData>(TrapsPrefab.Find(x => x.type == TrapType.BasicCaC).prefab, tile, true,
                new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * 0.3f);
        }
        
        for (int i = 0; i < nbEnemyArcher; i++)
        {
            var angle = i * Mathf.PI * 2 / nbEnemyArcher;
            if (nbEnemyArcher != 1) angle -= Mathf.PI / 4;

            SpawnEnemy<MinionData>(TrapsPrefab.Find(x => x.type == TrapType.Archer).prefab, tile, true,
                new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * 0.3f);
        }
        
        for (int i = 0; i < nbEnemySkeleton; i++)
        {
            var angle = i * Mathf.PI * 2 / nbEnemySkeleton;
            if (nbEnemySkeleton != 1) angle -= Mathf.PI / 4;

            SpawnEnemy<MinionData>(TrapsPrefab.Find(x => x.type == TrapType.Skeleton).prefab, tile, false,
                new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * 0.3f);
        }

        if (web)
        {
            SpawnEnemy<TrapData>(TrapsPrefab.Find(x => x.type == TrapType.Web).prefab, tile, true, Vector3.zero);
        }

        if (pyke)
        {
            SpawnEnemy<TrapData>(TrapsPrefab.Find(x => x.type == TrapType.Pyke).prefab, tile, false, Vector3.zero);
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
        int index = -1;
        if (addEnemyOnTile)
            mapManager.AddMinionOnTile(new Vector2Int(script.indexX, script.indexY), script, out index);

        if (script is MinionData minionData)
        {
            minionData.indexPos = index;
        }
    }
}