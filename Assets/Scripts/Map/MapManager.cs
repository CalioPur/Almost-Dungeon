using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

public class MapManager : MonoBehaviour
{
    public static event Action<TileData, CardHand, bool> OnCardTryToPlaceEvent;
    public static MapManager Instance { get; private set; }

    [field: SerializeField] public int width { get; private set; }
    [field: SerializeField] public int height { get; private set; }

    [SerializeField] private GameObject walls, floor;
    [SerializeField] private Transform map;
    [SerializeField] private FogPainter fogPainter;
    private CardInfo[] cards;
    private TileData[,] mapArray;

    private void Awake()
    {
        Instance = this;
        CardsManager.DistributeCardEvent += InitCards;
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

        CardsManager.OnCardTryToPlaceEvent += CheckCardPos;
    }

    public void CreateFog(Vector2Int StarterPos)
    {
        fogPainter.dungeonTilesPositions.Add(new Vector2Int(StarterPos.x, StarterPos.y)); //position de départ du hero, A CHANGER
        FogGenerator.CreateFog(fogPainter.dungeonTilesPositions, fogPainter, width - 2, height - 2);
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
        SetConnectedToPath();
        SetExits();
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
            card.img.transform.position = card.transform.position;
            card.removeSelection();
        }
        OnCardTryToPlaceEvent?.Invoke(data, card, canBePlaced);
        SetConnectedToPath();
        SetExits();
    }

    private void SetExits()
    {
        for (int i = 0; i < width - 2; i++)
        for (int j = 0; j < height - 2; j++)
            if (mapArray[i, j].isConnectedToPath)
                mapArray[i, j].isExit = ((j == 0 && mapArray[i, j].hasDoorDown) ||
                                         (mapArray[i, j].hasDoorDown && !mapArray[i, j - 1].PiecePlaced) ||
                                         (j == height - 3 && mapArray[i, j].hasDoorUp) ||
                                         (mapArray[i, j].hasDoorUp && !mapArray[i, j + 1].PiecePlaced) ||
                                         (i == 0 && mapArray[i, j].hasDoorLeft) ||
                                         (mapArray[i, j].hasDoorLeft && !mapArray[i - 1, j].PiecePlaced) ||
                                         (i == width - 3 && mapArray[i, j].hasDoorRight) ||
                                         (mapArray[i, j].hasDoorRight && !mapArray[i + 1, j].PiecePlaced));
    }

    private void SetConnectedToPath()
    {
        for (int i = 0; i < width - 2; i++)
        {
            for (int j = 0; j < height - 2; j++)
            {
                if (mapArray[i, j].isConnectedToPath) continue;
                if (i > 0 && mapArray[i - 1, j].isConnectedToPath && mapArray[i, j].hasDoorLeft)
                    mapArray[i, j].isConnectedToPath = true;
                else if (i < width - 3 && mapArray[i + 1, j].isConnectedToPath && mapArray[i, j].hasDoorRight)
                    mapArray[i, j].isConnectedToPath = true;
                else if (j > 0 && mapArray[i, j - 1].isConnectedToPath && mapArray[i, j].hasDoorDown)
                    mapArray[i, j].isConnectedToPath = true;
                else if (j < height - 3 && mapArray[i, j + 1].isConnectedToPath && mapArray[i, j].hasDoorUp)
                    mapArray[i, j].isConnectedToPath = true;
            }
        }
    }

    public void InitEnterDungeon(CardInfoInstance card, Vector2Int normsX, Vector2Int normsY, out Vector3 pos, out Vector2Int startPos)
    {
        
        startPos = new Vector2Int(Random.Range(normsX.x, normsX.y), Random.Range(normsY.x, normsY.y));
        
        SetTileAtPosition(card, startPos.x, startPos.y);

        mapArray[startPos.x, startPos.y].isConnectedToPath = true;
        mapArray[startPos.x, startPos.y].isVisited = true;
        SetConnectedToPath();
        SetExits();
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

        CheckAllTilesTypeAndRotation();
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
            if (minionData is MinionData minion)
                minion.Move(posToGo + offset, 0.5f);
            data.enemies.Add(minionData);
            return true;
        }

        return false;
    }

    public void CheckAllTilesTypeAndRotation()
    {
        for (int i = 0; i < width - 2; i++)
        {
            for (int j = 0; j < height - 2; j++)
            {
                CheckTileTypeAndRotation(mapArray[i, j]);
            }
        }

        SetConnectedToPath();
        SetExits();
    }

    private void CheckTileTypeAndRotation(TileData tileData)
    {
        if (tileData.img.sprite.name == "EnterDungeon") return;
        if (isLTile(tileData))
        {
            tileData.img.sprite = cards.First(x => x.imgOnMap.name == "LWay").imgOnMap;
            tileData.transform.rotation = Quaternion.Euler(90, GetRotationFromLTile(tileData), 0);
        }
        else if (isStraightTile(tileData))
        {
            tileData.img.sprite = cards.First(x => x.imgOnMap.name == "SimpleWay").imgOnMap;
            tileData.transform.rotation = Quaternion.Euler(90, GetRotationFromStraightTile(tileData), 0);
        }
        else if (isTTile(tileData))
        {
            tileData.img.sprite = cards.First(x => x.imgOnMap.name == "TWay").imgOnMap;
            tileData.transform.rotation = Quaternion.Euler(90, GetRotationFromTTile(tileData), 0);
        }
        else if (isCrossTile(tileData))
        {
            tileData.img.sprite = cards.First(x => x.imgOnMap.name == "XRoom").imgOnMap;
            tileData.img.sprite = cards[3].imgOnMap;
        }
        else if (isDeadEndTile(tileData))
        {
            tileData.img.sprite = cards.First(x => x.imgOnMap.name == "Sas").imgOnMap;
            tileData.transform.rotation = Quaternion.Euler(90, GetRotationFromDeadEndTile(tileData), 0);
        }
    }

    private bool isLTile(TileData data)
    {
        return data.hasDoorDown && data.hasDoorLeft && !data.hasDoorRight && !data.hasDoorUp ||
               data.hasDoorDown && !data.hasDoorLeft && data.hasDoorRight && !data.hasDoorUp ||
               !data.hasDoorDown && data.hasDoorLeft && !data.hasDoorRight && data.hasDoorUp ||
               !data.hasDoorDown && !data.hasDoorLeft && data.hasDoorRight && data.hasDoorUp;
    }

    private float GetRotationFromLTile(TileData data)
    {
        return data.hasDoorDown switch
        {
            true when data.hasDoorLeft && !data.hasDoorRight && !data.hasDoorUp => 0,
            true when !data.hasDoorLeft && data.hasDoorRight && !data.hasDoorUp => 270,
            false when !data.hasDoorLeft && data.hasDoorRight && data.hasDoorUp => 180,
            false when data.hasDoorLeft && !data.hasDoorRight && data.hasDoorUp => 90,
            _ => 0
        };
    }

    private bool isStraightTile(TileData data)
    {
        return data.hasDoorDown && !data.hasDoorLeft && !data.hasDoorRight && data.hasDoorUp ||
               !data.hasDoorDown && !data.hasDoorLeft && data.hasDoorRight && !data.hasDoorUp;
    }

    private float GetRotationFromStraightTile(TileData data)
    {
        return data.hasDoorDown switch
        {
            true when !data.hasDoorLeft && !data.hasDoorRight && data.hasDoorUp => 90,
            false when !data.hasDoorLeft && data.hasDoorRight && !data.hasDoorUp => 0,
            _ => 0
        };
    }

    private bool isTTile(TileData data)
    {
        return data.hasDoorDown && data.hasDoorLeft && data.hasDoorRight && !data.hasDoorUp ||
               !data.hasDoorDown && data.hasDoorLeft && data.hasDoorRight && data.hasDoorUp ||
               data.hasDoorDown && !data.hasDoorLeft && data.hasDoorRight && data.hasDoorUp ||
               data.hasDoorDown && data.hasDoorLeft && !data.hasDoorRight && data.hasDoorUp;
    }

    private float GetRotationFromTTile(TileData data)
    {
        return data.hasDoorDown switch
        {
            true when data.hasDoorLeft && data.hasDoorRight && !data.hasDoorUp => 270,
            false when data.hasDoorLeft && data.hasDoorRight && data.hasDoorUp => 90,
            true when !data.hasDoorLeft && data.hasDoorRight && data.hasDoorUp => 180,
            true when data.hasDoorLeft && !data.hasDoorRight && data.hasDoorUp => 0,
            _ => 0
        };
    }

    private bool isCrossTile(TileData data)
    {
        return data.hasDoorDown && data.hasDoorLeft && data.hasDoorRight && data.hasDoorUp;
    }

    private bool isDeadEndTile(TileData data)
    {
        return data.hasDoorDown && !data.hasDoorLeft && !data.hasDoorRight && !data.hasDoorUp ||
               !data.hasDoorDown && data.hasDoorLeft && !data.hasDoorRight && !data.hasDoorUp ||
               !data.hasDoorDown && !data.hasDoorLeft && data.hasDoorRight && !data.hasDoorUp ||
               !data.hasDoorDown && !data.hasDoorLeft && !data.hasDoorRight && data.hasDoorUp;
    }

    private float GetRotationFromDeadEndTile(TileData data)
    {
        return data.hasDoorDown switch
        {
            true when !data.hasDoorLeft && !data.hasDoorRight && !data.hasDoorUp => 270,
            false when data.hasDoorLeft && !data.hasDoorRight && !data.hasDoorUp => 0,
            false when !data.hasDoorLeft && data.hasDoorRight && !data.hasDoorUp => 180,
            false when !data.hasDoorLeft && !data.hasDoorRight && data.hasDoorUp => 90,
            _ => 0
        };
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
        CardsManager.DistributeCardEvent -= InitCards;
        CardsManager.OnCardTryToPlaceEvent -= CheckCardPos;
        OnCardTryToPlaceEvent = null;
    }
}