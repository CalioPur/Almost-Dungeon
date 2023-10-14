using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

[Serializable]
public struct Card
{
    public Sprite img;
    public int nbToBuild;
    public TileType type;
}

public class DeckManager : MonoBehaviour
{
    public static DeckManager Instance;
    public static event Action<Image> OnCardSelectedEvent;

    [SerializeField] private Transform DeckObject;
    [SerializeField] private List<SlotHandManager> slotsHand;
    [SerializeField] private List<Card> deckToBuild;
    [SerializeField] private float TimerDrawCard;
    [SerializeField] private UnusedSlot unusedSlotManager;
    [SerializeField] private RectTransform trHand;

    private List<Card> deck;
    private List<Card> Hand = new();
    private SlotHandManager selectedCard = null;



    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        OnCardSelectedEvent += OnCartSelected;
        InitDeck();
        ShuffleDeck();
        StartCoroutine(CheckDrawCard());
    }

    private void OnCartSelected(Image obj)
    {
        //CartSelected(obj);
    }

    private IEnumerator CheckDrawCard()
    {
        yield return new WaitForSeconds(TimerDrawCard);
        DrawCard();
        if (unusedSlotManager.transform.childCount > 0 && deck.Count > 0)
            StartCoroutine(CheckDrawCard());
    }

    public void CartSelected(SlotHandManager imgSelected)
    {
        foreach (var t in slotsHand.Where(t => t == imgSelected))
        {
            if (t == selectedCard)
            {
                selectedCard.ChangeSelection(false);
                selectedCard = null;
            }
            else
            {
                if (selectedCard != null)
                {
                    selectedCard.ChangeSelection(false);
                }

                t.ChangeSelection(true);
                selectedCard = t;
            }


        }
    }

    private void InitDeck()
    {
        deck = new List<Card>();
        foreach (var card in deckToBuild)
        {
            for (int i = 0; i < card.nbToBuild; i++)
            {
                deck.Add(card);
            }
        }
        slotsHand.ForEach(t => unusedSlotManager.AddSlot(t));
        slotsHand.Clear();
    }

    private void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            Card temp = deck[i];
            int randomIndex = UnityEngine.Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }
    
    public void DiscardCurrentCard()
    {
        if (selectedCard == null) return;
        Hand.Remove(selectedCard.card);
        unusedSlotManager.AddSlot(selectedCard);
        selectedCard = null;
    }

    private void DrawCard()
    {
        Card card = deck[0];
        if (deck.Count > 0)
        {
            Hand.Add(deck[0]);
            deck.RemoveAt(0);
        }

        if (deck.Count == 0)
            DeckObject.gameObject.SetActive(false);
        
        SlotHandManager slot = unusedSlotManager.GetSlot();
        if (slot != null)
        {
            slot.GetImage().sprite = card.img;
            slot.transform.SetParent(trHand.transform);
            slotsHand.Add(slot);
            slot.card = card;
        }

        // for (int i = 0; i < slotsHand.Count; i++)
        // {
        //     if (Hand.Count > i)
        //     {
        //         slotsHand[i].gameObject.SetActive(true);
        //         slotsHand[i].GetImage().sprite = Hand[i].img;
        //     }
        //     else
        //     {
        //         slotsHand[i].gameObject.SetActive(false);
        //     }
        // }
    }

    private void RotateSelection()
    {
        if (selectedCard != null)
        {
            selectedCard.transform.Rotate(0, 0, 90);
            selectedCard.addRotation();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateSelection();
        }
    }


    public TileType getTypeSelectedCard()
    {
        if (selectedCard == null) return TileType.ERROR;
        return selectedCard.card.type;
    }
    
    public Vector3 GetRotationSelectedCard()
    {
        if (selectedCard == null) return Vector3.zero;
        return selectedCard.GetRotation();
    }
}