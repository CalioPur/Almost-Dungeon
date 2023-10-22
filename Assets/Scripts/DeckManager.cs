using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Card
{
    public Sprite img;
    public int nbToBuild;
    public TileType type;
    public bool DoorOnTop;
    public bool DoorOnBottom;
    public bool DoorOnLeft;
    public bool DoorOnRight;
}

[Serializable]
public enum TileType
{
    HorizontalWall = 0,
    VerticalWall,
    Start,
    End,
    SimpleLine,
    LLine,
    TLine,
    XLine,
    ERROR = -1,
}

public class DeckManager : MonoBehaviour
{
    [SerializeField] private List<Card> deckToBuild = new ();
    [SerializeField] private List<Card> deckCreate = new ();
    [SerializeField] private int nbCardOnStartToDraw = 3;
    [SerializeField] private float TimerBeforeDrawCard = 2;
    [SerializeField] private List<SlotHandManager> slotsHand;
    [SerializeField] private Transform DeckTr;
    
    private Coroutine drawCardCoroutine;
    private List<Card> Hand = new();

    void Start()
    {
        InitDeck();
        ShuffleDeck();
        for (int i = 0; i < nbCardOnStartToDraw; i++)
        {
            DrawCard();
        }
        drawCardCoroutine = StartCoroutine(CheckDrawCard());
    }
    
    private IEnumerator CheckDrawCard()
    {
        yield return new WaitForSeconds(TimerBeforeDrawCard);
        DrawCard();
        if (deckCreate.Count > 0)
            drawCardCoroutine = StartCoroutine(CheckDrawCard());
        else
        {
            StopCoroutine(drawCardCoroutine);
            drawCardCoroutine = null;
        }
    }
    
    private void DrawCard()
    {
        if (deckCreate.Count == 0) return;
        Card card = deckCreate[0];
        if (deckCreate.Count > 0)
        {
            Hand.Add(deckCreate[0]);
            deckCreate.RemoveAt(0);
        }

        if (deckCreate.Count == 0)
            DeckTr.gameObject.SetActive(false);

        // SlotHandManager slot = unusedSlotManager.GetSlot();
        // if (slot != null)
        // {
        //     slot.GetImage().sprite = card.img;
        //     slotsHand.Add(slot);
        //     slot.InitCard(card);
        // }
        
        // ajouter la carte dans la main
    }
    
    private void InitDeck()
    {
        deckCreate = new List<Card>();
        foreach (var card in deckToBuild)
        {
            for (int i = 0; i < card.nbToBuild; i++)
            {
                deckCreate.Add(card);
            }
        }
        slotsHand.Clear();
    }

    private void ShuffleDeck()
    {
        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < deckCreate.Count; i++)
            {
                Card temp = deckCreate[i];
                int randomIndex = UnityEngine.Random.Range(i, deckCreate.Count);
                deckCreate[i] = deckCreate[randomIndex];
                deckCreate[randomIndex] = temp;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
