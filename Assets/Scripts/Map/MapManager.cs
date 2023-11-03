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
    public int width;
    public int height;

    [SerializeField] private GameObject walls, floor;
    [SerializeField] private Transform map;
    [SerializeField] private CardInfo[] cards;

    private TileData[,] mapArray;
    
    public static MapManager Instance { get; private set; }
    
    private void Awake()
    {
        Instance = this;
    }

    struct pp
    {
        public string name;
        public int y;
    }
    
    private void TEST()
    {
    }
    
    
    public void InitMap()
    {
        TEST();
        
        
        mapArray = new TileData[width, height];
        width += 2;
        height += 2;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector3 pos;
                GetWorldPosFromTilePos(i, j, out pos); //pour centrer le tout
                if (i == 0 || j == 0 || i == width - 1 || j == height - 1)
                {
                    Instantiate(walls, pos, walls.transform.rotation, map); //verifie si on est sur un bord
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
        //     checkTileToManipulateRandomPosition(cardInstance);
        // }
        SetConnectedToPath();
        SetExits();
    }

    private void checkTileToManipulateRandomPosition(CardInfoInstance cardInstance)
    {
        int x = Random.Range(1, width - 3);
        int y = Random.Range(1, height - 3);

        if (!CheckPosWithPosition(x, y, cardInstance)) checkTileToManipulateRandomPosition(cardInstance);
        else SetTileAtPosition(cardInstance, x, y);
    }

    private void CheckCardPos(TileData data, CardHand card)
    {
        bool canBePlaced = true;
        canBePlaced = CheckPosWithData(data, card);
        OnCardTryToPlaceEvent?.Invoke(data, card, canBePlaced);
        SetConnectedToPath();
        SetExits();
    }

    private void SetExits()
    {
        for (int i = 0; i < width - 2; i++)
        {
            for (int j = 0; j < height - 2; j++)
            {
                if (mapArray[i, j].isConnectedToPath)
                {
                    mapArray[i, j].isExit = false;
                    if (j == 0 && mapArray[i, j].hasDoorDown)
                    {
                        mapArray[i, j].isExit = true;
                    }
                    else if (mapArray[i, j].hasDoorDown && !mapArray[i, j - 1].PiecePlaced)
                    {
                        mapArray[i, j].isExit = true;
                    }

                    if (j == height - 3 && mapArray[i, j].hasDoorUp)
                    {
                        mapArray[i, j].isExit = true;
                    }
                    else if (mapArray[i, j].hasDoorUp && !mapArray[i, j + 1].PiecePlaced)
                    {
                        mapArray[i, j].isExit = true;
                    }

                    if (i == 0 && mapArray[i, j].hasDoorLeft)
                    {
                        mapArray[i, j].isExit = true;
                    }
                    else if (mapArray[i, j].hasDoorLeft && !mapArray[i - 1, j].PiecePlaced)
                    {
                        mapArray[i, j].isExit = true;
                    }

                    if (i == width - 3 && mapArray[i, j].hasDoorRight)
                    {
                        mapArray[i, j].isExit = true;
                    }
                    else if (mapArray[i, j].hasDoorRight && !mapArray[i + 1, j].PiecePlaced)
                    {
                        mapArray[i, j].isExit = true;
                    }
                }
            }
        }
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
                if (i < width - 3 && mapArray[i + 1, j].isConnectedToPath && mapArray[i, j].hasDoorRight)
                    mapArray[i, j].isConnectedToPath = true;
                if (j > 0 && mapArray[i, j - 1].isConnectedToPath && mapArray[i, j].hasDoorDown)
                    mapArray[i, j].isConnectedToPath = true;
                if (j < height - 3 && mapArray[i, j + 1].isConnectedToPath && mapArray[i, j].hasDoorUp)
                    mapArray[i, j].isConnectedToPath = true;
            }
        }
    }

    public void InitEnterDungeon(CardInfoInstance card, out Vector3 pos, Vector2Int startPos)
    {

        SetTileAtPosition(card, startPos.x, startPos.y);
        mapArray[startPos.x, startPos.y].isConnectedToPath = true;
        mapArray[startPos.x, startPos.y].isVisited = true;
        SetConnectedToPath();
        SetExits();
        GetWorldPosFromTilePos(startPos.x, startPos.y, out pos);
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
    
    public void ChangeTileDataAtPosition(int x, int y, TileData data)
    {
        if (x >= width - 2 || y >= height - 2 || x < 0 || y < 0) return;
        mapArray[x, y] = data;
    }

    void SetTileAtPosition(CardInfoInstance card, int posX, int posY)
    {
        mapArray[posX, posY].SetInstance(card);
    }

    public void GetWorldPosFromTilePos(int x, int y, out Vector3 pos)
    {
        pos = new Vector3(x - ((float)(width - 1) / 2), 0, y - (float)(height - 1) / 2);
    }
    
    public void GetTilePosFromWorldPos(Vector3 pos, out int x, out int y)
    {
        x = Mathf.RoundToInt(pos.x + ((float)(width - 1) / 2))-1;
        y = Mathf.RoundToInt(pos.z + ((float)(height - 1) / 2))-1;
    }

    public bool CheckIfTileIsFree(int x, int y)
    {
        if (x >= width - 2 || y >= height - 2 || x < 0 || y < 0) return false;
        return mapArray[x, y].PiecePlaced;
    }

    public void GetNbMonstersOnPos(Vector2Int pos, out List<TrapData> minions)
    {
        TileData data = GetTileDataAtPosition(pos.x, pos.y);
        minions = new List<TrapData>(data.enemies.OrderBy(x => x.GetSO().targetPriority));
    }

    public void RemoveEnemyOnTile(Vector2Int vector2Int, TrapData minionData)
    {
        TileData data = GetTileDataAtPosition(vector2Int.x, vector2Int.y);
        data.enemies.Remove(minionData);
    }

    public void AddMinionOnTile(Vector2Int vector2Int, TrapData minionData)
    {
        TileData data = GetTileDataAtPosition(vector2Int.x, vector2Int.y);
        data.enemies.Add(minionData);
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
        if (isLTile(tileData))
        {
            tileData.img.sprite = cards[0].imgOnMap;
            tileData.transform.rotation = Quaternion.Euler(90, GetRotationFromLTile(tileData), 0);
        }
        else if (isStraightTile(tileData))
        {
            tileData.img.sprite = cards[1].imgOnMap;
            tileData.transform.rotation = Quaternion.Euler(90, GetRotationFromStraightTile(tileData),0);
        }
        else if (isTTile(tileData))
        {
            tileData.img.sprite = cards[2].imgOnMap;
            tileData.transform.rotation = Quaternion.Euler(90, GetRotationFromTTile(tileData),0);
        }
        else if (isCrossTile(tileData))
        {
            tileData.img.sprite = cards[3].imgOnMap;
        }
        else if (isDeadEndTile(tileData))
        {
            tileData.img.sprite = cards[4].imgOnMap;
            tileData.transform.rotation = Quaternion.Euler(90, GetRotationFromDeadEndTile(tileData),0);
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

void Update()
{
    //debug the tile that are connected to the path with a red line that goes up
    for (int i = 0; i < width - 2; i++)
    {
        for (int j = 0; j < height - 2; j++)
        {
            // if (mapArray[i, j].isConnectedToPath)
            // {
            //     Debug.DrawLine(mapArray[i, j].transform.position, mapArray[i, j].transform.position + Vector3.up,
            //         Color.red, 1f);
            // }
            //
            // if (mapArray[i, j].isExit)
            // {
            //     Debug.DrawLine(mapArray[i, j].transform.position, mapArray[i, j].transform.position + Vector3.up,
            //         Color.magenta, 1f);
            // }
            // if (mapArray[i, j].isVisited)
            // {
            //     Debug.DrawLine(mapArray[i, j].transform.position, mapArray[i, j].transform.position + Vector3.up,
            //         Color.green, 1f);
            // }
        }
    }
}

    public TileData[,] getMapArray()
    {
        return mapArray;
    }

    public bool GetTile(int heroIndexHeroX, int heroIndexHeroY, out TileData tile)
    {
        tile = GetTileDataAtPosition(heroIndexHeroX, heroIndexHeroY);
        return tile != null;
    }
}