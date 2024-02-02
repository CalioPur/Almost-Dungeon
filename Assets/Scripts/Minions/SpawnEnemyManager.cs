using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyManager : MonoBehaviour
{
    [SerializeField] private List<ListOfTraps> TrapsPrefab;

    private static List<ListOfTraps> _trapsPrefab;

    private void OnEnable()
    {
        MovementManager.OnTilePosedEvent += SpawnMinionOnTile;
        _trapsPrefab = TrapsPrefab;
        FireCamp.ClearMinions();
    }

    private void OnDisable()
    {
        MovementManager.OnTilePosedEvent -= SpawnMinionOnTile;
        FireCamp.ClearMinions();
    }

    public static void SpawnEnemyWithType(TrapType type, TileData tile, Vector3 offset, int index,
        MapManager mapManager)
    {
        Vector3 offsetCoin = Vector3.zero;
        switch (type)
        {
            case TrapType.BasicCaC:
                SpawnTrapData(_trapsPrefab.Find(x => x.type == TrapType.BasicCaC).prefab as MinionData, tile, true,
                    offsetCoin, index, mapManager);
                break;
            case TrapType.Archer:
                SpawnTrapData(_trapsPrefab.Find(x => x.type == TrapType.Archer).prefab as MinionData, tile, true,
                    offsetCoin, index, mapManager);
                break;
            case TrapType.Skeleton:
                SpawnTrapData(_trapsPrefab.Find(x => x.type == TrapType.Skeleton).prefab as MinionData, tile, false,
                    offsetCoin, index, mapManager);
                break;
            case TrapType.Slime:
                SpawnTrapData(_trapsPrefab.Find(x => x.type == TrapType.Slime).prefab as MinionData, tile, true, offsetCoin,
                    index, mapManager);
                break;
            case TrapType.Laden:
                SpawnTrapData(_trapsPrefab.Find(x => x.type == TrapType.Laden).prefab as MinionData, tile, true, offsetCoin,
                    index, mapManager);
                break;
            case TrapType.Wolf:
                SpawnTrapData(_trapsPrefab.Find(x => x.type == TrapType.Wolf).prefab, tile, true, offsetCoin, index,
                    mapManager);
                break;
            case TrapType.Web:
                SpawnTrapData(_trapsPrefab.Find(x => x.type == TrapType.Web).prefab, tile, false, offsetCoin, -1,
                    mapManager);
                break;
            case TrapType.Pyke:
                SpawnTrapData(_trapsPrefab.Find(x => x.type == TrapType.Pyke).prefab, tile, false, offsetCoin, -1,
                    mapManager);
                break;
            case TrapType.FireCamp:
                SpawnTrapData(_trapsPrefab.Find(x => x.type == TrapType.FireCamp).prefab, tile, false, offsetCoin, -1,
                    mapManager);
                break;

            case TrapType.None:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SpawnMinionOnTile(TileData tile, CardInfoInstance card)
    {
        MapManager mapManager = MapManager.Instance;
        for (int i = 0; i < card.So.TypeOfTrapOrEnemyToSpawn.Length; i++)
        {
            int index = card.TypeOfTrapOrEnemyToSpawnInstance[i].indexOffsetTile;
            SpawnEnemyWithType(card.So.TypeOfTrapOrEnemyToSpawn[i].type, tile, card.So.offsetMinionPos[0], index,
                mapManager);
        }
    }

    public static void SpawnTrapData<T>(T prefab, TileData tile, bool addEnemyOnTile, Vector3 positionOffset,
        int indexOnTile, MapManager _mapManager)
        where T : TrapData
    {
        _mapManager.GetIndexFromTile(tile, out Vector2Int indexes);
        SpawnEnemy(prefab, indexes, tile.transform.position, _mapManager, addEnemyOnTile);
    }

    public static void SpawnEnemy<T>(T prefab, Vector2Int indexes, Vector3 position, MapManager _mapManager,
        bool addEnemyOnTile, int indexOnTile)
        where T : TrapData
    {
        if (!_mapManager.AvailableForSpawn(indexes.x, indexes.y)) return;
        T script = Instantiate(prefab, position, prefab.transform.rotation);
        script.indexX = indexes.x;
        script.indexY = indexes.y;
        script.mapManager = _mapManager;
        int index = indexOnTile;
        if (addEnemyOnTile)
            _mapManager.AddMinionOnTile(new Vector2Int(script.indexX, script.indexY), script, ref index);

        if (script is MinionData minionData)
        {
            minionData.indexOffsetTile = index;
            FireCamp.StockMinions(minionData);
        }
    }

    public static void SpawnEnemyWithoutPrefab(TrapType typeOfEnemy, TileData tile, bool addEnemyOnTile,
        Vector3 positionOffset, int indexOnTile, MapManager _mapManager)
    {
        Vector3 position = tile.transform.position + positionOffset;
        _mapManager.GetTilePosFromWorldPos(position, out int indexesX, out int indexesY);
        SpawnEnemyWithType(typeOfEnemy, tile, positionOffset, indexOnTile, _mapManager);
    }
}