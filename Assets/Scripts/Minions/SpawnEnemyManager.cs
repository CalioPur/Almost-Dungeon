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
    
    private static List<ListOfTraps> _trapsPrefab;

    private void OnEnable()
    {
        MovementManager.OnTilePosedEvent += SpawnMinionOnTile;
        _trapsPrefab = TrapsPrefab;
    }
    
    private void OnDisable()
    {
        MovementManager.OnTilePosedEvent -= SpawnMinionOnTile;
    }

    private void SpawnMinionOnTile(TileData tile, CardInfoInstance card)
    {
        for (int i = 0; i < card.So.TypeOfTrapOrEnemyToSpawn.Length; i++)
        {
            int index = card.TypeOfTrapOrEnemyToSpawnInstance[i].indexOffsetTile;
            switch (card.So.TypeOfTrapOrEnemyToSpawn[i].type)
            {
                case TrapType.BasicCaC:
                    SpawnTrapData(TrapsPrefab.Find(x => x.type == TrapType.BasicCaC).prefab as MinionData, tile, true, card.So.offsetMinionPos[index], index);
                    break;
                case TrapType.Archer:
                    SpawnTrapData(TrapsPrefab.Find(x => x.type == TrapType.Archer).prefab as MinionData, tile, true, card.So.offsetMinionPos[index], index);
                    break;
                case TrapType.Skeleton:
                    SpawnTrapData(TrapsPrefab.Find(x => x.type == TrapType.Skeleton).prefab as MinionData, tile, true, card.So.offsetMinionPos[index], index);
                    break;
                case TrapType.Slime:
                    SpawnTrapData(TrapsPrefab.Find(x => x.type == TrapType.Slime).prefab as MinionData, tile, true, card.So.offsetMinionPos[index], index);
                    break;
                case TrapType.Web:
                    SpawnTrapData(TrapsPrefab.Find(x => x.type == TrapType.Web).prefab, tile, true, Vector3.zero, -1);
                    break;
                case TrapType.Pyke:
                    SpawnTrapData(TrapsPrefab.Find(x => x.type == TrapType.Pyke).prefab, tile, true, Vector3.zero, -1);
                    break;
                case TrapType.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }
    }

    public void SpawnTrapData<T>(T prefab, TileData tile, bool addEnemyOnTile, Vector3 positionOffset, int indexOnTile)
        where T : TrapData
    {
        Vector3 position = tile.transform.position + positionOffset;
        mapManager.GetTilePosFromWorldPos(position, out int indexesX, out int indexesY);
        SpawnEnemy(prefab, new Vector2Int(indexesX, indexesY), position, mapManager, addEnemyOnTile, indexOnTile);
    }
    
    public static void SpawnEnemy<T>(T prefab, Vector2Int indexes, Vector3 position, MapManager _mapManager, bool addEnemyOnTile, int indexOnTile)
        where T : TrapData
    {
        if(!_mapManager.AvailableForSpawn(indexes.x, indexes.y)) return;
        T script = Instantiate(prefab, position, prefab.transform.rotation);
        script.indexX = indexes.x;
        script.indexY = indexes.y;
        script.mapManager = _mapManager;
        int index = indexOnTile;
        if (addEnemyOnTile) _mapManager.AddMinionOnTile(new Vector2Int(script.indexX, script.indexY), script, ref index);

        if (script is MinionData minionData)
        {
            minionData.indexOffsetTile = index;
        }
    }
    
    public static void SpawnEnemyWithoutPrefab(TrapType typeOfEnemy, TileData tile, bool addEnemyOnTile, Vector3 positionOffset, int indexOnTile, MapManager _mapManager)
    {
        Vector3 position = tile.transform.position + positionOffset;
        _mapManager.GetTilePosFromWorldPos(position, out int indexesX, out int indexesY);
        SpawnEnemy(_trapsPrefab.Find(x => x.type == typeOfEnemy).prefab, new Vector2Int(indexesX, indexesY), position, _mapManager, addEnemyOnTile, indexOnTile);
    }
}