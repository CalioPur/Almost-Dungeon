using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

public class MapManager : MonoBehaviour
{
    public static event Action<TileData, CardHand, bool> OnCardTryToPlaceEvent;

    [SerializeField] private int width, height;
    [SerializeField] private GameObject walls, floor;
    [SerializeField] private Transform map;
    [SerializeField] private CardInfo[] cards;

    private TileData[,] mapArray;

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
        for (int i = 0; i < 5; i++)
        {
            CardInfo card = cards[Random.Range(0, cards.Length)];
            CardInfoInstance cardInstance = card.CreateInstance();
            int nbRot = Random.Range(0, 3);
            for (int j = 0; j < nbRot; j++)
            {
                cardInstance.AddRotation(true);
            }
            Oui(cardInstance);
        }
    }

    private void Oui(CardInfoInstance cardInstance)
    {
        int x = Random.Range(1, width - 3);
        int y = Random.Range(1, height - 3);

        if (!CheckPosWithPosition(x, y, cardInstance)) Oui(cardInstance);
        else SetTileAtPosition(cardInstance, x, y);
    }

    private void CheckCardPos(TileData data, CardHand card)
    {
        bool canBePlaced = true;
        canBePlaced = CheckPosWithData(data, card);
        OnCardTryToPlaceEvent?.Invoke(data, card, canBePlaced);
    }

    public void InitEnterDungeon(CardInfoInstance card, out Vector3 pos, out int _x, out int _y)
    {
        int y = Random.Range(0, height - 2);

        SetTileAtPosition(card, 0, y);

        GetWorldPosFromTilePos(0, y, out pos);
        _x = 0;
        _y = y;
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

    public TileData GetTileData(int x, int y)
    {
        return mapArray[x, y];
    }

    void SetTileAtPosition(CardInfoInstance card, int posX, int posY)
    {
        mapArray[posX, posY].SetInstance(card);
    }

    public void GetWorldPosFromTilePos(int x, int y, out Vector3 pos)
    {
        pos = new Vector3(x - ((float)(width - 1) / 2), 0, y - (float)(height - 1) / 2);
    }
    
    public bool CheckIfTileIsFree(int x, int y)
    {
        return mapArray[x, y].PiecePlaced;
    }
}