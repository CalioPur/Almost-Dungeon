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
    Slime,
    None
}

[Serializable]
public struct ListOfTraps
{
    public TrapType type;
    public TrapData prefab;
}

public class SpawnEnemyManager : MonoBehaviour
{
    [SerializeField] private List<ListOfTraps> TrapsPrefab;
    [SerializeField] private MapManager mapManager;

    private void OnEnable()
    {
        DragAndDropManager.OnTilePosedEvent += SpawnMinionOnTile;
    }
    
    private void OnDisable()
    {
        DragAndDropManager.OnTilePosedEvent -= SpawnMinionOnTile;
    }

    private void SpawnMinionOnTile(TileData tile, CardInfoInstance card)
    {
        int nbEnemyBasic = 0;
        int nbEnemyArcher = 0;
        int nbEnemySkeleton = 0;
        int nbEnemyslime = 0;
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
                case TrapType.Slime:
                    nbEnemyslime++;
                    break;
            }
        }
       
        for (int i = 0; i < nbEnemyBasic; i++)
        {
            var angle = i * Mathf.PI * 2 / nbEnemyBasic;
            if (nbEnemyBasic != 1) angle -= Mathf.PI / 4;

            SpawnEnemy(TrapsPrefab.Find(x => x.type == TrapType.BasicCaC).prefab as MinionData, tile, true,
                new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * 0.3f);
        }
        
        for (int i = 0; i < nbEnemyArcher; i++)
        {
            var angle = i * Mathf.PI * 2 / nbEnemyArcher;
            if (nbEnemyArcher != 1) angle -= Mathf.PI / 4;

            SpawnEnemy(TrapsPrefab.Find(x => x.type == TrapType.Archer).prefab as MinionData, tile, true,
                new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * 0.3f);
        }
        
        for (int i = 0; i < nbEnemySkeleton; i++)
        {
            var angle = i * Mathf.PI * 2 / nbEnemySkeleton;
            if (nbEnemySkeleton != 1) angle -= Mathf.PI / 4;

            SpawnEnemy(TrapsPrefab.Find(x => x.type == TrapType.Skeleton).prefab as MinionData, tile, false,
                new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * 0.3f);
        }
        
        for (int i = 0; i < nbEnemyslime; i++)
        {
            var angle = i * Mathf.PI * 2 / nbEnemyslime;
            if (nbEnemyslime != 1) angle -= Mathf.PI / 4;

            SpawnEnemy(TrapsPrefab.Find(x => x.type == TrapType.Slime).prefab as MinionData, tile, false,
                new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * 0.3f);
        }

        if (web)
        {
            SpawnEnemy(TrapsPrefab.Find(x => x.type == TrapType.Web).prefab, tile, true, Vector3.zero);
        }

        if (pyke)
        {
            SpawnEnemy(TrapsPrefab.Find(x => x.type == TrapType.Pyke).prefab, tile, false, Vector3.zero);
        }
    }

    public void SpawnEnemy<T>(T prefab, TileData tile, bool addEnemyOnTile, Vector3 positionOffset)
        where T : TrapData
    {
        Vector3 position = tile.transform.position + positionOffset;
        mapManager.GetTilePosFromWorldPos(position, out int indexesX, out int indexesY);
        SpawnEnemy(prefab, new Vector2Int(indexesX, indexesY), position, mapManager, addEnemyOnTile);
    }
    
    public static void SpawnEnemy<T>(T prefab, Vector2Int indexes, Vector3 position, MapManager _mapManager, bool addEnemyOnTile)
        where T : TrapData
    {
        if(!_mapManager.AvailableForSpawn(indexes.x, indexes.y)) return;
        T script = Instantiate(prefab, position, prefab.transform.rotation);
        script.indexX = indexes.x;
        script.indexY = indexes.y;
        script.mapManager = _mapManager;
        int index = -1;
        if (addEnemyOnTile) _mapManager.AddMinionOnTile(new Vector2Int(script.indexX, script.indexY), script, out index);

        if (script is MinionData minionData)
        {
            minionData.indexOffsetTile = index;
        }
    }
}