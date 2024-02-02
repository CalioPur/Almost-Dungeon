using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapManager : MonoBehaviour
{
    public static event Action<TileData, CardHand, bool> OnCardTryToPlaceEvent;
    public static MapManager Instance { get; private set; }
    
    public static event Action MakeFogEvent;

    [field: SerializeField] public int width { get; private set; }
    [field: SerializeField] public int height { get; private set; }

    public MapManagerTools MapManagerTools => _mapManagerTools;

    public Sprite[] _sprites;
    
    private string path = "Sprites";

    [SerializeField] private GameObject walls, floor;
    public Sprite floorSpriteForRage;
    [SerializeField] private Transform map;
    [SerializeField] private FogPainter fogPainter;
    public CardInfo[] cards;
    public TileData[,] mapArray;
    private readonly MapManagerTools _mapManagerTools;
    private List<TilePresetStruct> _tilePreset = new ();

    [SerializeField] private Sprite ATTENTION;
    [SerializeField] private Sprite ATTENTIONROUUUGE;

    public MapManager()
    {
        _mapManagerTools = new MapManagerTools(this);
    }

    private void Update()
    {
        if (!Hero.Instance) return;
        for (int i = 0; i < Instance.width - 2; i++)
        {
            for (int j = 0; j < Instance.height - 2; j++)
            {
                if (!mapArray[i, j].isExit) continue;
                if (mapArray[i, j].hasDoorDown && j > 0 && !mapArray[i, j - 1].isConnectedToPath)
                {
                    if (Hero.Instance.GetIndexHeroPos().x == i && Hero.Instance.GetIndexHeroPos().y == j)
                    {
                        mapArray[i, j - 1].img.sprite = ATTENTIONROUUUGE;
                    }
                    else
                    {
                        mapArray[i, j - 1].img.sprite = ATTENTION;
                    }
                }
                if (mapArray[i, j].hasDoorUp && j < height - 3 && !mapArray[i, j + 1].isConnectedToPath)
                {
                    if (Hero.Instance.GetIndexHeroPos().x == i && Hero.Instance.GetIndexHeroPos().y == j)
                    {
                        mapArray[i, j + 1].img.sprite = ATTENTIONROUUUGE;
                    }
                    else
                    {
                        mapArray[i, j + 1].img.sprite = ATTENTION;
                    }
                }
                if (mapArray[i, j].hasDoorLeft && i > 0 && !mapArray[i - 1, j].isConnectedToPath)
                {
                    if (Hero.Instance.GetIndexHeroPos().x == i && Hero.Instance.GetIndexHeroPos().y == j)
                    {
                        mapArray[i - 1, j].img.sprite = ATTENTIONROUUUGE;
                    }
                    else
                    {
                        mapArray[i - 1, j].img.sprite = ATTENTION;
                    }
                }
                if (mapArray[i, j].hasDoorRight && i < width - 3 && !mapArray[i + 1, j].isConnectedToPath)
                {
                    if (Hero.Instance.GetIndexHeroPos().x == i && Hero.Instance.GetIndexHeroPos().y == j)
                    {
                        mapArray[i + 1, j].img.sprite = ATTENTIONROUUUGE;
                    }
                    else
                    {
                        mapArray[i + 1, j].img.sprite = ATTENTION;
                    }
                }
            }
        }
    }

    private void Awake()
    {
        Instance = this;
        DeckManager.DistributeCardEvent += InitCards;
        
        //open path
        _sprites = Resources.LoadAll<Sprite>(path);
        floorSpriteForRage = floor.GetComponent<SpriteRenderer>().sprite;
    }

    public Vector2Int GetSizeDungeon()
    {
        return new Vector2Int(width - 2, height - 2);
    }

    private void InitCards(CardInfo[] _cards)
    {
        cards = _cards;
    }

    public void SpawnTilePresets()
    {
        for (int i = 0; i < _tilePreset.Count; i++)
        {
            CardInfoInstance card = _tilePreset[i].cardInfo.CreateInstance();
            fogPainter.dungeonTilesPositions.Add(new Vector2Int(_tilePreset[i].position.x, _tilePreset[i].position.y));
            for (int j = 0; j < _tilePreset[i].rotation; j++)
            {
                card.AddRotation(false);
            }
            SetTileAtPosition(card, _tilePreset[i].position.x, _tilePreset[i].position.y);
            if (card.TypeOfTrapOrEnemyToSpawnInstance.Length > 0)
            {
                for (int j = 0; j < card.TypeOfTrapOrEnemyToSpawnInstance.Length; j++)
                {
                    EnemiDataOnHand data = card.TypeOfTrapOrEnemyToSpawnInstance[j];
                    GetTile(_tilePreset[i].position, out var tile);
                    Vector3 offset = Vector3.zero;
                    if (!(card.TypeOfTrapOrEnemyToSpawnInstance[j].type == TrapType.Web ||
                          card.TypeOfTrapOrEnemyToSpawnInstance[j].type == TrapType.Pyke))
                    {
                        if (_tilePreset[i].cardInfo.offsetMinionPos.Length > 0)
                        {
                            if (data.indexOffsetTile >= _tilePreset[i].cardInfo.offsetMinionPos.Length ||
                                data.indexOffsetTile < 0)
                            {
                                offset = _tilePreset[i].cardInfo.offsetMinionPos[0];
                            }
                            else
                            {
                                offset = _tilePreset[i].cardInfo.offsetMinionPos[data.indexOffsetTile];
                            }
                        }
                    }
                    

                    SpawnEnemyManager.SpawnEnemyWithoutPrefab(data.type,tile, true, offset, data.indexOffsetTile, this);
                }
            }
        }
        MapManagerTools.SetConnectedToPath();
        MapManagerTools.SetExits();
        
        // SetAllTilesAsVisited();
    }

    public void SetAllTilesAsVisited()
    {
        for (int i = 0; i < width - 2; i++)
        {
            for (int j = 0; j < height - 2; j++)
            {
                if (mapArray[i, j].isConnectedToPath)
                    mapArray[i, j].IsVisited = true;
            }
        }
    }

    public void SpawnMap()
    {
        mapArray = new TileData[width, height];
        width += 2;
        height += 2;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector3 pos;
                GetWorldPosFromTilePos(new Vector2Int(i, j), out pos); //pour centrer le tout
                if (i == 0 || j == 0 || i == width - 1 || j == height - 1)
                {
                    //Instantiate(walls, pos, walls.transform.rotation, map); //verifie si on est sur un bord
                }
                else
                {
                    mapArray[i - 1, j - 1] =
                        Instantiate(floor, pos, walls.transform.rotation, map)
                            .GetComponent<TileData>(); //verifie si on est sur un bord
                }
            }
        }
    }
    
    public void InitMap()
    {
        SpawnMap();
        SpawnTilePresets();
        MapManagerTools.SetConnectedToPath();
        MapManagerTools.SetExits();
        HandsManager.OnCardTryToPlaceEvent += CheckCardPos;
    }

    public void CreateFog(Vector2Int StarterPos)
    {
        // fogPainter.dungeonTilesPositions.Add(new Vector2Int(StarterPos.x, StarterPos.y)); //position de départ du hero, A CHANGER
        // FogGenerator.CreateFog(fogPainter.dungeonTilesPositions, fogPainter, width - 2, height - 2);
        MakeFogEvent?.Invoke();
    }
    
    public void AddRandomCard()
    {
        // for (int i = 0; i < 5; i++)
        // {
        //     CardInfo card = cards[Random.Range(0, cards.Length)];
        //     CardInfoInstance cardInstance = card.CreateInstance();
        //     int nbRot = Random.Range(0, 3);
        //     for (int j = 0; j < nbRot; j++)
        //     {
        //         cardInstance.AddRotation(true);
        //     }
        //
        //     CheckTileToManipulateRandomPosition(cardInstance);
        // }
        MapManagerTools.SetConnectedToPath();
        MapManagerTools.SetExits();
    }

    private void CheckTileToManipulateRandomPosition(CardInfoInstance cardInstance)
    {
        int x = Random.Range(1, width - 3);
        int y = Random.Range(1, height - 3);

        if (!CheckPosWithPosition(x, y, cardInstance)) CheckTileToManipulateRandomPosition(cardInstance);
        else SetTileAtPosition(cardInstance, x, y);
    }

    private void CheckCardPos(TileData data, CardHand card)
    {
        bool canBePlaced = true;
        canBePlaced = CheckPosWithData(data, card);
        if (!canBePlaced)
        {
            return;
            // card.GetImage().transform.position = card.transform.position;
            // card.removeSelection();
        }
        OnCardTryToPlaceEvent?.Invoke(data, card, canBePlaced);
        MapManagerTools.SetConnectedToPath();
        MapManagerTools.SetExits();
    }

    public void InitEnterDungeon(CardInfoInstance card,int rot, out Vector3 pos, Vector2Int startPos)
    {
        for (int i = 0; i < rot; i++)
        {
            card.AddRotation(false);
        }
        SetTileAtPosition(card, startPos.x, startPos.y);

        mapArray[startPos.x, startPos.y].isConnectedToPath = true;
        mapArray[startPos.x, startPos.y].IsVisited = true;
        MapManagerTools.SetConnectedToPath();
        MapManagerTools.SetExits();
        GetWorldPosFromTilePos(startPos, out pos);
    }

    private bool CheckPosWithData(TileData data, CardHand card)
    {
        for (int i = 0; i < width - 2; i++)
        {
            for (int j = 0; j < height - 2; j++)
            {
                if (mapArray[i, j] != data) continue;
                return CheckPosWithPosition(i, j, card.Card);
            }
        }

        return true;
    }


    private bool CheckPosWithPosition(int x, int y, CardInfoInstance card)
    {
        if (mapArray[x, y].PiecePlaced) return false;
        bool West = x > 0 && mapArray[x - 1, y].PiecePlaced;
        bool East = x < width - 3 && mapArray[x + 1, y].PiecePlaced;
        bool South = y > 0 && mapArray[x, y - 1].PiecePlaced;
        bool North = y < height - 3 && mapArray[x, y + 1].PiecePlaced;


        if (West && mapArray[x - 1, y].hasDoorRight != card.DoorOnLeft) return false;
        if (East && mapArray[x + 1, y].hasDoorLeft != card.DoorOnRight) return false;
        if (South && mapArray[x, y - 1].hasDoorUp != card.DoorOnBottom) return false;
        if (North && mapArray[x, y + 1].hasDoorDown != card.DoorOnTop) return false;


        return true;
    }

    public TileData GetTileDataAtPosition(int x, int y)
    {
        if (x >= width - 2 || y >= height - 2 || x < 0 || y < 0) return null;
        return mapArray[x, y];
    }

    public bool DoorIsOpenAtPosition(Vector2Int pos, DirectionToMove dir)
    {
        return dir switch
        {
            DirectionToMove.Left => mapArray[pos.x, pos.y].hasDoorLeft,
            DirectionToMove.Right => mapArray[pos.x, pos.y].hasDoorRight,
            DirectionToMove.Up => mapArray[pos.x, pos.y].hasDoorUp,
            DirectionToMove.Down => mapArray[pos.x, pos.y].hasDoorDown,
            DirectionToMove.None => false,
            DirectionToMove.Error => throw new ArgumentOutOfRangeException(nameof(dir), dir, null),
            _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
        };
    }

    public Vector2Int GetPosFromData(TileData data)
    {
        for (int i = 0; i < width - 2; i++)
        {
            for (int j = 0; j < height - 2; j++)
            {
                if (data.Equals(mapArray[i, j]))
                {
                    return new Vector2Int(i, j);
                }
            }
        }

        Debug.LogError("il n'existe aucune tile correspondant a : " + data.name);
        return new Vector2Int(-1, -1);
    }

    public void ChangeTileDataAtPosition(int x, int y)
    {
        ResetMapArrayCell(x, y);
        // if (y != 0) ResetMapArrayCell(x, y - 1);
        // if (x != 0) ResetMapArrayCell(x - 1, y);
        // if (y != height - 3) ResetMapArrayCell(x, y + 1);
        // if (x != width - 3) ResetMapArrayCell(x + 1, y);
        
        // switch (doorChanged)
        // {
        //     case 0:
        //         if (y == 0) return;
        //         if (mapArray[x, y - 1].PiecePlaced) mapArray[x, y - 1].hasDoorUp = true;
        //         break;
        //     case 1:
        //         if (x == 0) return;
        //         if (mapArray[x - 1, y].PiecePlaced) mapArray[x - 1, y].hasDoorRight = true;
        //         break;
        //     case 2:
        //         if (y == height - 3) return;
        //         if (mapArray[x, y + 1].PiecePlaced) mapArray[x, y + 1].hasDoorDown = true;
        //         break;
        //     case 3:
        //         if (x == width - 3) return;
        //         if (mapArray[x + 1, y].PiecePlaced) mapArray[x + 1, y].hasDoorLeft = true;
        //         break;
        // }
        
        mapArray[x, y].isRoom = mapArray[x, y].img.sprite.name.Contains("Room");

        MapManagerTools.CheckAllTilesTypeAndRotation();
        MapManagerTools.SetConnectedToPath();
        MapManagerTools.SetExits();
    }
    
    void ResetMapArrayCell(int xCoord, int yCoord)
    {
        if (mapArray[xCoord, yCoord].PiecePlaced)
        {
            mapArray[xCoord, yCoord].hasDoorUp = true;
            mapArray[xCoord, yCoord].hasDoorRight = true;
            mapArray[xCoord, yCoord].hasDoorDown = true;
            mapArray[xCoord, yCoord].hasDoorLeft = true;
            mapArray[xCoord, yCoord].gameObject.GetComponent<SpriteRenderer>().sprite = floorSpriteForRage;
            mapArray[xCoord, yCoord].img.color = Color.white;
            mapArray[xCoord, yCoord].isVisited = false;
            mapArray[xCoord, yCoord].isConnectedToPath = false;
            mapArray[xCoord, yCoord].isExit = false;
            mapArray[xCoord, yCoord].PiecePlaced = false;
        }
    }

    void SetTileAtPosition(CardInfoInstance card, int posX, int posY)
    {
        mapArray[posX, posY].SetInstance(card);
    }

    public void GetWorldPosFromTilePos(Vector2Int oldPos, out Vector3 pos, bool isStatic = false)
    {
        pos = new Vector3(oldPos.x - ((float)(width - 1) / 2), 0, oldPos.y - (float)(height - 1) / 2);
        if (isStatic)
        {
            pos = mapArray[oldPos.x, oldPos.y].transform.position;
        }
    }

    public void GetTilePosFromWorldPos(Vector3 pos, out int x, out int y)
    {
        x = Mathf.RoundToInt(pos.x + ((float)(width - 1) / 2)) - 1;
        y = Mathf.RoundToInt(pos.z + ((float)(height - 1) / 2)) - 1;
    }

    public bool CheckIfTileIsFree(Vector2Int pos)
    {
        if (pos.x >= width - 2 || pos.y >= height - 2 || pos.x < 0 || pos.y < 0) return false;
        return mapArray[pos.x, pos.y].PiecePlaced;
    }

    public bool GetMonstersOnPos(Vector2Int pos, out List<TrapData> minions)
    {
        if (pos.x >= width - 2 || pos.y >= height - 2 || pos.x < 0 || pos.y < 0) { minions = null; return false;}
        TileData data = GetTileDataAtPosition(pos.x, pos.y);
        minions = new List<TrapData>();
        if (data.enemies.Count == 0) return false;
        foreach (var enemy in data.enemies.OrderBy(x => x.GetSO().targetPriority))
        {
            minions.Add(enemy);
        }
        //minions = new List<TrapData>(data.enemies.OrderBy(x => x.GetSO().targetPriority));
        return true;
    }
    
    public int GetNbMonstersOnPos(Vector2Int pos)
    {
        TileData data = GetTileDataAtPosition(pos.x, pos.y);
        return data.enemies.Count;
    }

    public void RemoveEnemyOnTile(Vector2Int vector2Int, TrapData minionData, Vector3 WoldPos)
    {
        TileData data = GetTileDataAtPosition(vector2Int.x, vector2Int.y);
        data.enemies.Remove(minionData);
        if (minionData is MinionData minion)
            data.freePosition(minion.indexOffsetTile);
    }
    
    public bool AvailableForSpawn(int x, int y)
    {
        return mapArray[x, y].AvailableForSpawn();
    }

    public bool AddMinionOnTile(Vector2Int vector2Int, TrapData minionData, ref int index)
    {
        TileData data = GetTileDataAtPosition(vector2Int.x, vector2Int.y);
        GetWorldPosFromTilePos(vector2Int, out Vector3 posToGo);
        if (data.GetFirstAvailabalePosition(out var offset, ref index))
        {
            data.enemies.Add(minionData);
            return true;
        }

        return false;
    }

    public TileData[,] getMapArray()
    {
        return mapArray;
    }

    public bool GetTile(Vector2Int indexes, out TileData tile)
    {
        tile = GetTileDataAtPosition(indexes.x, indexes.y);
        return tile != null;
    }

    public void AddTrapOnTile(Vector2Int vector2Int, TrapData trapData)
    {
        TileData data = GetTileDataAtPosition(vector2Int.x, vector2Int.y);
        data.enemies.Add(trapData);
    }
    
    private void OnDisable()
    {
        DeckManager.DistributeCardEvent -= InitCards;
        HandsManager.OnCardTryToPlaceEvent -= CheckCardPos;
        OnCardTryToPlaceEvent = null;
    }

    public void SpawnPresets(List<TilePresetStruct> tilePreset)
    {
        _tilePreset = tilePreset;
    }

    public void Revive()
    {
        foreach (var tile in mapArray)
        {
            if (tile.PiecePlaced)
            {
                if (tile._instance.TypeOfTrapOrEnemyToSpawnInstance.Length > 0)
                {
                    foreach (var enemy in tile._instance.TypeOfTrapOrEnemyToSpawnInstance)
                    {
                        if (enemy.canBeRevive)
                        {
                            SpawnEnemyManager.SpawnEnemyWithType(enemy.type, tile, Vector3.zero, enemy.indexOffsetTile, this);
                        }
                    }
                }
            }
        }
    }

    public bool HasDoorOpen(Vector2Int oldPos, Vector2Int newPos)
    {
        if (newPos.x >= width - 2 || newPos.y >= height - 2 || newPos.x < 0 || newPos.y < 0) return false;
        if (oldPos.x == newPos.x) return oldPos.y > newPos.y ? mapArray[oldPos.x, oldPos.y].hasDoorDown : mapArray[oldPos.x, oldPos.y].hasDoorUp;
        return oldPos.x > newPos.x ? mapArray[oldPos.x, oldPos.y].hasDoorLeft : mapArray[oldPos.x, oldPos.y].hasDoorRight;
    }
    public Vector2Int[] GetTilesInLineOfSight(Vector2Int startPos)
    {
        return MapManagerTools.GetTilesInLineOfSight(startPos);
    }

    public void GetIndexFromTile(TileData tile, out Vector2Int vector2Int)
    {
        vector2Int = new Vector2Int(-1, -1);
        for (int i = 0; i < width - 2; i++)
        {
            if (vector2Int.x != -1) break;
            for (int j = 0; j < height - 2; j++)
            {
                if (mapArray[i, j] == tile)
                {
                    vector2Int = new Vector2Int(i, j);
                    return;
                }
            }
        }
    }
}