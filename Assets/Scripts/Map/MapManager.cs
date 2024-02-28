using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ink.Parsed;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapManager : MonoBehaviour
{
    public static event Action<TileData, CardHand, bool> OnCardTryToPlaceEvent;
    public static MapManager Instance { get; private set; }

    [field: SerializeField] public int width { get; private set; }
    [field: SerializeField] public int height { get; private set; }

    public MapManagerTools MapManagerTools => _mapManagerTools;

    public Sprite[] _sprites;
    public GameObject[] _models;

    private string path = "Sprites";
    private string path2 = "Models";

    [SerializeField] private TileData floor;
    public Sprite floorSpriteForRage;
    [SerializeField] private Transform map;
    [SerializeField] private FogPainter fogPainter;
    public TileData[,] mapArray;
    private readonly MapManagerTools _mapManagerTools;
    private List<TilePresetStruct> _tilePreset = new();

    [SerializeField] private Sprite ATTENTION;
    [SerializeField] private Sprite ATTENTIONROUUUGE;

    private bool UpScale = false;

    private CardInfo[] cards;

    public MapManager()
    {
        _mapManagerTools = new MapManagerTools(this);
    }

    private IEnumerator UpdateBoard()
    {
        // List<TileData> tiles = new List<TileData>();
        // foreach (var tile in tiles)
        // {
        //     tile.img.transform.localScale = new Vector3(1f, 1f, 1f);
        // }

        if (Hero.Instance)
        {
            for (int i = 0; i < Instance.width; i++)
            {
                for (int j = 0; j < Instance.height; j++)
                {
                    if (!mapArray[i, j].isExit) continue;
                    if (mapArray[i, j].hasDoorDown && j > 0 && !mapArray[i, j - 1].isConnectedToPath)
                    {
                        if (Hero.Instance.GetIndexHeroPos().x == i && Hero.Instance.GetIndexHeroPos().y == j)
                        {
                            mapArray[i, j - 1].img.sprite = ATTENTIONROUUUGE;
                            //tiles.Add(mapArray[i, j - 1]);
                        }
                        else
                        {
                            mapArray[i, j - 1].img.sprite = ATTENTION;
                            //tiles.Add(mapArray[i, j - 1]);
                        }
                    }

                    if (mapArray[i, j].hasDoorUp && j < height - 1 && !mapArray[i, j + 1].isConnectedToPath)
                    {
                        if (Hero.Instance.GetIndexHeroPos().x == i && Hero.Instance.GetIndexHeroPos().y == j)
                        {
                            mapArray[i, j + 1].img.sprite = ATTENTIONROUUUGE;
                            //tiles.Add(mapArray[i, j + 1]);
                        }
                        else
                        {
                            mapArray[i, j + 1].img.sprite = ATTENTION;
                            //tiles.Add(mapArray[i, j + 1]);
                        }
                    }

                    if (mapArray[i, j].hasDoorLeft && i > 0 && !mapArray[i - 1, j].isConnectedToPath)
                    {
                        if (Hero.Instance.GetIndexHeroPos().x == i && Hero.Instance.GetIndexHeroPos().y == j)
                        {
                            mapArray[i - 1, j].img.sprite = ATTENTIONROUUUGE;
                            //tiles.Add(mapArray[i - 1, j]);
                        }
                        else
                        {
                            mapArray[i - 1, j].img.sprite = ATTENTION;
                            //tiles.Add(mapArray[i - 1, j]);
                        }
                    }

                    if (mapArray[i, j].hasDoorRight && i < width - 1 && !mapArray[i + 1, j].isConnectedToPath)
                    {
                        if (Hero.Instance.GetIndexHeroPos().x == i && Hero.Instance.GetIndexHeroPos().y == j)
                        {
                            mapArray[i + 1, j].img.sprite = ATTENTIONROUUUGE;
                            //tiles.Add(mapArray[i + 1, j]);
                        }
                        else
                        {
                            mapArray[i + 1, j].img.sprite = ATTENTION;
                            //tiles.Add(mapArray[i + 1, j]);
                        }
                    }
                }
            }

            // yield return new WaitForSeconds(0.2f);
            // UpScale = !UpScale;
            // for (var index = 0; index < tiles.Count; index++)
            // {
            //     var tile = tiles[index];
            //     if (UpScale)
            //         tile.img.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            // }
        }

        yield return null;
        StartCoroutine(UpdateBoard());
    }

    private void Awake()
    {
        Instance = this;
        DeckManager.DistributeCardEvent += InitCards;

        //open path
        _sprites = Resources.LoadAll<Sprite>(path);
        _models = Resources.LoadAll<GameObject>(path2);
        floorSpriteForRage = floor.GetComponent<SpriteRenderer>().sprite;
    }

    public Vector2Int GetSizeDungeon()
    {
        return new Vector2Int(width, height);
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
                    SpawnEnemyManager.SpawnEnemyWithoutPrefab(data.type, tile, this);
                }
            }
        }

        _mapManagerTools.SetConnectedToPath();
        _mapManagerTools.SetExits();
        _mapManagerTools.CheckAllTilesTypeAndRotation();


        // SetAllTilesAsVisited();
    }

    public void SetAllTilesAsVisited()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (mapArray[i, j].isConnectedToPath)
                    mapArray[i, j].IsVisited = true;
            }
        }
    }

    public void SpawnMap()
    {
        mapArray = new TileData[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector3 pos;
                GetWorldPosFromTilePos(new Vector2Int(i, j), out pos); //pour centrer le tout
                mapArray[i, j] =
                    Instantiate(floor, pos, floor.transform.rotation, map); //verifie si on est sur un bord
            }
        }
    }

    public void InitMap()
    {
        SpawnMap();
        SpawnTilePresets();
        _mapManagerTools.SetConnectedToPath();
        _mapManagerTools.SetExits();
        HandsManager.OnCardTryToPlaceEvent += CheckCardPos;
        StartCoroutine(UpdateBoard());
    }

    public void AddRandomCard()
    {
        _mapManagerTools.SetConnectedToPath();
        _mapManagerTools.SetExits();
    }

    private void CheckTileToManipulateRandomPosition(CardInfoInstance cardInstance)
    {
        int x = Random.Range(1, width - 1);
        int y = Random.Range(1, height - 1);

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
        _mapManagerTools.SetConnectedToPath();
        _mapManagerTools.SetExits();
    }

    public void InitEnterDungeon(CardInfoInstance card, int rot, out Vector3 pos, Vector2Int startPos)
    {
        for (int i = 0; i < rot; i++)
        {
            card.AddRotation(false);
        }

        SetTileAtPosition(card, startPos.x, startPos.y);

        mapArray[startPos.x, startPos.y].isConnectedToPath = true;
        mapArray[startPos.x, startPos.y].IsVisited = true;
        _mapManagerTools.SetConnectedToPath();
        _mapManagerTools.SetExits();
        GetWorldPosFromTilePos(startPos, out pos);
    }

    private bool CheckPosWithData(TileData data, CardHand card)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
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
        bool East = x < width - 1 && mapArray[x + 1, y].PiecePlaced;
        bool South = y > 0 && mapArray[x, y - 1].PiecePlaced;
        bool North = y < height - 1 && mapArray[x, y + 1].PiecePlaced;


        if (West && mapArray[x - 1, y].hasDoorRight != card.DoorOnLeft) return false;
        if (East && mapArray[x + 1, y].hasDoorLeft != card.DoorOnRight) return false;
        if (South && mapArray[x, y - 1].hasDoorUp != card.DoorOnBottom) return false;
        if (North && mapArray[x, y + 1].hasDoorDown != card.DoorOnTop) return false;

        if (x == 0 && card.DoorOnLeft) return false;
        if (x == width - 1 && card.DoorOnRight) return false;
        if (y == 0 && card.DoorOnBottom) return false;
        if (y == height - 1 && card.DoorOnTop) return false;

        return true;
    }

    public TileData GetTileDataAtPosition(int x, int y)
    {
        if (x >= width || y >= height || x < 0 || y < 0) return null;
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
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
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

        mapArray[x, y].isRoom = mapArray[x, y].img.sprite.name.Contains("Room");

        _mapManagerTools.CheckAllTilesTypeAndRotation();
        _mapManagerTools.SetConnectedToPath();
        _mapManagerTools.SetExits();
    }

    void ResetMapArrayCell(int xCoord, int yCoord)
    {
        if (mapArray[xCoord, yCoord].PiecePlaced)
        {
            mapArray[xCoord, yCoord].hasDoorUp = true;
            mapArray[xCoord, yCoord].hasDoorRight = true;
            mapArray[xCoord, yCoord].hasDoorDown = true;
            mapArray[xCoord, yCoord].hasDoorLeft = true;
            SpriteRenderer renderer = mapArray[xCoord, yCoord].gameObject.GetComponent<SpriteRenderer>();
            renderer.sprite = floorSpriteForRage;
            renderer.enabled = false;
            mapArray[xCoord, yCoord].img.color = Color.white;
            mapArray[xCoord, yCoord].isVisited = false;
            mapArray[xCoord, yCoord].isConnectedToPath = false;
            mapArray[xCoord, yCoord].isExit = false;
            mapArray[xCoord, yCoord].PiecePlaced = false;
            int childCount = mapArray[xCoord, yCoord].transform.childCount;
            List<GameObject> children = new List<GameObject>();
            for (int i = 0; i < childCount; i++)
            {
                children.Add(mapArray[xCoord, yCoord].transform.GetChild(i).gameObject);
            }

            mapArray[xCoord, yCoord].transform.DetachChildren();
            
            foreach (var child in children)
            {
                Destroy(child);
            }

            if (mapArray[xCoord, yCoord]._instance != null)
                mapArray[xCoord, yCoord]._instance.Rotation = 0;
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
        x = Mathf.RoundToInt(pos.x + ((float)(width - 1) / 2));
        y = Mathf.RoundToInt(pos.z + ((float)(height - 1) / 2));
    }

    public bool CheckIfTileIsFree(Vector2Int pos)
    {
        if (pos.x >= width || pos.y >= height || pos.x < 0 || pos.y < 0) return false;
        return mapArray[pos.x, pos.y].PiecePlaced;
    }

    public bool GetMonstersOnPos(Vector2Int pos, out List<TrapData> minions)
    {
        if (pos.x >= width || pos.y >= height || pos.x < 0 || pos.y < 0)
        {
            minions = null;
            return false;
        }

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
            data.freePosition();
    }

    public bool AvailableForSpawn(int x, int y)
    {
        return mapArray[x, y].AvailableForSpawn();
    }

    public bool AddMinionOnTile(Vector2Int vector2Int, TrapData minionData)
    {
        TileData data = GetTileDataAtPosition(vector2Int.x, vector2Int.y);
        GetWorldPosFromTilePos(vector2Int, out Vector3 posToGo);
        if (data.GetFirstAvailabalePosition())
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
                            SpawnEnemyManager.SpawnEnemyWithType(enemy.type, tile, this);
                        }
                    }
                }
            }
        }
    }

    public bool HasDoorOpen(Vector2Int oldPos, Vector2Int newPos)
    {
        if (newPos.x >= width || newPos.y >= height || newPos.x < 0 || newPos.y < 0) return false;
        if (oldPos.x == newPos.x)
            return oldPos.y > newPos.y
                ? mapArray[oldPos.x, oldPos.y].hasDoorDown
                : mapArray[oldPos.x, oldPos.y].hasDoorUp;
        return oldPos.x > newPos.x
            ? mapArray[oldPos.x, oldPos.y].hasDoorLeft
            : mapArray[oldPos.x, oldPos.y].hasDoorRight;
    }

    public void GetIndexFromTile(TileData tile, out Vector2Int vector2Int)
    {
        vector2Int = new Vector2Int(-1, -1);
        for (int i = 0; i < width; i++)
        {
            if (vector2Int.x != -1) break;
            for (int j = 0; j < height; j++)
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