using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class DragAndDropManager : MonoBehaviour
{
    public static event Action<TileData> OnTileSelectedEvent;
    
    private void Start()
    {
        CreateMap.OnCardTryToPlaceEvent += PlaceCard;
    }
    
    private void PlaceCard(TileData data, CardHand card, bool canBePlaced)
    {
        if (!canBePlaced) return;
        
        data.PiecePlaced = true;
        data.img.sprite = card.Card.img;
        data.transform.Rotate(Vector3.forward * card.GetRotation().y);
        data.hasDoorDown = card.Card.DoorOnBottom;
        data.hasDoorUp = card.Card.DoorOnTop;
        data.hasDoorLeft = card.Card.DoorOnLeft;
        data.hasDoorRight = card.Card.DoorOnRight;
        
        card.EmptyCard();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
               if (hit.collider.gameObject.CompareTag("Floor"))
               {
                      TileData tile = hit.collider.gameObject.GetComponent<TileData>();
                      if (tile.PiecePlaced) return;
                      OnTileSelectedEvent?.Invoke(tile);
               }
            }
        }
    }
}