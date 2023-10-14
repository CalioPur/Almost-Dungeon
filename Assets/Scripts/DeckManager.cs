using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private Transform DeckObject;
    [SerializeField] private List<Image> slotsHand;
    [SerializeField] private List<Card> deckToBuild;
    [SerializeField] private float TimerDrawCard;

    private List<Card> deck;
    private List<Card> Hand = new();

    [Serializable]
    private struct Card
    {
        public Sprite img;
        public int nbToBuild;
    }

    private void Start()
    {
        InitDeck();
        ShuffleDeck();
        StartCoroutine(CheckDrawCard());
    }

    private IEnumerator CheckDrawCard()
    {
        yield return new WaitForSeconds(TimerDrawCard);
        DrawCard();
        if (Hand.Count < slotsHand.Count && deck.Count > 0)
            StartCoroutine(CheckDrawCard());
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
        if (deck.Count > 0)
        {
            Hand.Add(deck[0]);
            deck.RemoveAt(0);
        }
        if (deck.Count == 0)
            DeckObject.gameObject.SetActive(false);

        Debug.Log(Hand.Count);
        for (int i = 0; i < slotsHand.Count; i++)
        {
            if (Hand.Count > i)
            {
                slotsHand[i].sprite = Hand[i].img;
            }
            else
            {
                slotsHand[i].sprite = null;
            }
        }
    }
}