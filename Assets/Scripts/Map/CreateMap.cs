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
    [SerializeField] private Sprite enterDungeon;
    [SerializeField] private Transform map;

    private TileData[,] mapArray;
    public void InitMap()
    {
        
        mapArray = new TileData[width, height];
        width+=2;
        height+=2;
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
    
    public Vector3 InitEnterDungeon(CardInfo card)
    {
        int y = UnityEngine.Random.Range(0, height - 2);
        
        mapArray[0, y].PiecePlaced = true;
        mapArray[0, y].hasDoorRight = card.DoorOnRight;
        mapArray[0, y].hasDoorLeft = card.DoorOnLeft;
        mapArray[0, y].hasDoorUp = card.DoorOnTop;
        mapArray[0, y].hasDoorDown = card.DoorOnBottom;
        mapArray[0, y].GetComponent<TileData>().img.sprite = card.img;
        
        return new Vector3(1 - ((float)(width - 1) / 2), 0.1f, 1 + y- (float)(height - 1) / 2);
    }

    private void CheckPos(TileData data, CardHand card)
    {
        bool canBePlaced = true;
        for(int i =0; i < width - 2; i++)
        {
            for (int j = 0; j < height - 2; j++)
            {
                if (mapArray[i, j] != data) continue;
                if (mapArray[i, j].PiecePlaced) canBePlaced = false;
                else if (i > 0 && mapArray[i - 1, j].PiecePlaced && ((mapArray[i - 1, j].hasDoorRight && !card.Card.DoorOnLeft) || (!mapArray[i - 1, j].hasDoorRight && card.Card.DoorOnLeft))) canBePlaced = false;
                else if (i < width - 3 && mapArray[i + 1, j].PiecePlaced &&  ((mapArray[i + 1, j].hasDoorLeft && !card.Card.DoorOnRight) || (!mapArray[i + 1, j].hasDoorLeft && card.Card.DoorOnRight))) canBePlaced = false;
                else if (j > 0 && mapArray[i, j - 1].PiecePlaced &&  ((mapArray[i, j - 1].hasDoorUp && !card.Card.DoorOnBottom) || (!mapArray[i, j - 1].hasDoorUp && card.Card.DoorOnBottom))) canBePlaced = false;
                else if (j < height - 3 && mapArray[i, j + 1].PiecePlaced &&  ((mapArray[i, j + 1].hasDoorDown && !card.Card.DoorOnTop) || (!mapArray[i, j + 1].hasDoorDown && card.Card.DoorOnTop))) canBePlaced = false;

                OnCardTryToPlaceEvent?.Invoke(data, card, canBePlaced);
                return;
            }
        }
        OnCardTryToPlaceEvent?.Invoke(data, card, false);
    }

    public TileData GetTileData(int x, int y)
    {
        return mapArray[x, y];
    }
    
}
