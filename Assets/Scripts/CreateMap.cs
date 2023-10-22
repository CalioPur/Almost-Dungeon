using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CreateMap : MonoBehaviour
{
    public static event Action<TileData, CardHand, bool> OnCardTryToPlaceEvent;
    
    [SerializeField] private int width, height;
    [SerializeField] private GameObject walls, floor;
    [SerializeField] private Transform map;

    private TileData[,] mapArray;
    void Start()
    {
       
        mapArray = new TileData[width-2, height-2];
        for(int i =0; i< width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                
                Vector3 pos = new Vector3(i-((float)(width-1)/2),0,j- (float)(height-1)/2); //pour centrer le tout
                if(i == 0 || j == 0 || i == width - 1 || j == height - 1)
                { 
                    Instantiate( walls, pos, walls.transform.rotation, map); //verifie si on est sur un bord
                }
                else
                {
                    mapArray[i - 1, j - 1] = Instantiate(floor, pos, walls.transform.rotation, map).GetComponent<TileData>(); //verifie si on est sur un bord
                }


            }
        }
        DeckManager.OnCardTryToPlaceEvent += CheckPos;
    }

    private void CheckPos(TileData data, CardHand card)
    {
        bool canBePlaced = true;
        for(int i =1; i < width - 2; i++)
        {
            for (int j = 1; j < height - 2; j++)
            {
                if (mapArray[i, j] == data)
                {
                    if (mapArray[i, j].PiecePlaced) canBePlaced = false;
                    // check if there is another card door was not blocked by the card
                    else if (i > 0 && mapArray[i - 1, j].PiecePlaced && mapArray[i - 1, j].hasDoorRight && !card.Card.DoorOnLeft) canBePlaced = false;
                    else if (i < width - 3 && mapArray[i + 1, j].PiecePlaced && mapArray[i + 1, j].hasDoorLeft && !card.Card.DoorOnRight) canBePlaced = false;
                    else if (j > 0 && mapArray[i, j - 1].PiecePlaced && mapArray[i, j - 1].hasDoorUp && !card.Card.DoorOnBottom) canBePlaced = false;
                    else if (j < height - 3 && mapArray[i, j + 1].PiecePlaced && mapArray[i, j + 1].hasDoorDown && !card.Card.DoorOnTop) canBePlaced = false;
                    // check if door will be connected
                    else if (i > 0 && mapArray[i - 1, j].PiecePlaced && !mapArray[i - 1, j].hasDoorRight && card.Card.DoorOnLeft) canBePlaced = false;
                    else if (i < width - 3 && mapArray[i + 1, j].PiecePlaced && !mapArray[i + 1, j].hasDoorLeft && card.Card.DoorOnRight) canBePlaced = false;
                    else if (j > 0 && mapArray[i, j - 1].PiecePlaced && !mapArray[i, j - 1].hasDoorUp && card.Card.DoorOnBottom) canBePlaced = false;
                    else if (j < height - 3 && mapArray[i, j + 1].PiecePlaced && !mapArray[i, j + 1].hasDoorDown && card.Card.DoorOnTop) canBePlaced = false;

                    break;
                }
            }
        }
        OnCardTryToPlaceEvent?.Invoke(data, card, canBePlaced);
    }

    public TileData GetTileData(int x, int y)
    {
        return mapArray[x, y];
    }
    
}
