using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private List<Card> deck;
    [SerializeField] private Tile tileFaceDeck;
    
    [Serializable]
    private struct Card
    {
        public Tile tile;
        public int nbOfCard;
    }

    public void InitializeDeckRender(SpawnMap mapManager)
    {
        mapManager.SetTile(new Vector3Int(0, 0), tileFaceDeck);
    }
    
}


