using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

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

    [Serializable]
    private struct Card
    {
        public Sprite img;
        public int nbToBuild;
    }

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
        if (Hand.Count < slotsHand.Count && deck.Count > 0)
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
            }

            selectedCard = t;
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
        //slotsHand.ForEach(t => unusedSlotManager.AddSlot(t));
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

    private void DrawCard()
    {
        return;
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
            //slot.transform.SetParent(trHand.transform);
            slotsHand.Add(slot);
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
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateSelection();
        }
    }


}