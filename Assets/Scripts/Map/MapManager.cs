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
    [SerializeField] private Transform map;
    [SerializeField] private FogPainter fogPainter;
    public CardInfo[] cards;
    public TileData[,] mapArray;
    private readonly MapManagerTools _mapManagerTools;
    private List<TilePreset> _tilePreset = new ();

    public MapManager()
    {
        _mapManagerTools = new MapManagerTools(this);
    }

    private void Awake()
    {
        Instance = this;
        DeckManager.DistributeCardEvent += InitCards;
        
        //open path
        _sprites = Resources.LoadAll<Sprite>(path);
    }

    public Vector2Int GetSizeDungeon()
    {
        return new Vector2Int(width - 2, height - 2);
    }

    private void InitCards(CardInfo[] _cards)
    {
        cards = _cards;
    }

    public void InitMap()
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

        for (int i = 0; i < _tilePreset.Count; i++)
        {
            CardInfoInstance card = _tilePreset[i].cardInfo.CreateInstance();
            fogPainter.dungeonTilesPositions.Add(new Vector2Int(_tilePreset[i].position.x, _tilePreset[i].position.y));
            for (int j = 0; j < _tilePreset[i].rotation; j++)
            {
                card.AddRotation(false);
            }
            Debug.Log("top: " + card.DoorOnTop + " bottom: " + card.DoorOnBottom + " left: " + card.DoorOnLeft + " right: " + card.DoorOnRight);
            SetTileAtPosition(card, _tilePreset[i].position.x, _tilePreset[i].position.y);
        }
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
            card.GetImage().transform.position = card.transform.position;
            card.removeSelection();
        }
        OnCardTryToPlaceEvent?.Invoke(data, card, canBePlaced);
        MapManagerTools.SetConnectedToPath();
        MapManagerTools.SetExits();
    }

    public void InitEnterDungeon(CardInfoInstance card, Vector2Int normsX, Vector2Int normsY, out Vector3 pos, out Vector2Int startPos)
    {
        
        startPos = new Vector2Int(Random.Range(normsX.x, normsX.y), Random.Range(normsY.x, normsY.y));
        
        SetTileAtPosition(card, startPos.x, startPos.y);

        mapArray[startPos.x, startPos.y].isConnectedToPath = true;
        mapArray[startPos.x, startPos.y].isVisited = true;
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

    public void ChangeTileDataAtPosition(int x, int y, TileData data, int doorChanged)
    {
        if (x > width - 2 || y > height - 2 || x < 0 || y < 0) return;
        mapArray[x, y] = data;
        switch (doorChanged)
        {
            case 0:
                if (mapArray[x, y - 1].PiecePlaced) mapArray[x, y - 1].hasDoorUp = true;
                break;
            case 1:
                if (mapArray[x - 1, y].PiecePlaced) mapArray[x - 1, y].hasDoorRight = true;
                break;
            case 2:
                if (mapArray[x, y + 1].PiecePlaced) mapArray[x, y + 1].hasDoorDown = true;
                break;
            case 3:
                if (mapArray[x + 1, y].PiecePlaced) mapArray[x + 1, y].hasDoorLeft = true;
                break;
        }
        
        mapArray[x, y].isRoom = mapArray[x, y].img.sprite.name.Contains("Room");

        MapManagerTools.CheckAllTilesTypeAndRotation();
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

    public void GetMonstersOnPos(Vector2Int pos, out List<TrapData> minions)
    {
        TileData data = GetTileDataAtPosition(pos.x, pos.y);
        minions = new List<TrapData>(data.enemies.OrderBy(x => x.GetSO().targetPriority));
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

    public bool AddMinionOnTile(Vector2Int vector2Int, TrapData minionData, out int index)
    {
        TileData data = GetTileDataAtPosition(vector2Int.x, vector2Int.y);
        GetWorldPosFromTilePos(vector2Int, out Vector3 posToGo);
        if (data.GetFirstAvailabalePosition(out var offset, out index))
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

    public void SpawnPresets(List<TilePreset> tilePreset)
    {
        _tilePreset = tilePreset;
    }
}