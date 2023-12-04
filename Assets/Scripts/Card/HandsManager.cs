using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class HandsManager : MonoBehaviour
{
    public static event Action<CardHand> OnCardSelectedEvent;
    public static event Action<TileData, CardHand> OnCardTryToPlaceEvent;
    
    [Header("References")] [SerializeField]
    private List<CardHand> slotsHand;

    private List<CardInfoInstance> Hand = new();
    private CardHand selectedCard;
    private int indexCardToDraw = 0;

    private void Start()
    {
        InitSlots();
       
    }
    
    private void OnEnable()
    {
        MovementManager.OnTilePosedEvent += MoveCard;
        MovementManager.OnFinishToPose += ReorganizeHand;
        MovementManager.OnTileSelectedEvent += PlaceSolution;
    }
    
    private void OnDisable()
    {
        MovementManager.OnTilePosedEvent -= MoveCard;
        MovementManager.OnFinishToPose -= ReorganizeHand;
        MovementManager.OnTileSelectedEvent -= PlaceSolution;
    }

    private void MoveCard(TileData _, CardInfoInstance cardInfo)
    {
        indexCardToDraw = slotsHand.FindIndex(t => t.Card == cardInfo);
    }

    private void InitSlots()
    {
        for (int i = 0; i < slotsHand.Count; i++)
        {
            slotsHand[i].EmptyCard();
            slotsHand[i].OnCardWasPointedEvent += OnCardWasPointed;
        }
    }
    
    private void ReorganizeHand(CardInfoInstance cardPosed)
    {
        Hand.Remove(cardPosed);
        selectedCard.EmptyCard();
        selectedCard.removeSelection();
        selectedCard = null;
        for (int i = indexCardToDraw; i < slotsHand.Count - 1; i++)
        {
            slotsHand[i].MoveCardTo(slotsHand[i + 1]);
            slotsHand[i+1].Occupied = false;
            slotsHand[i].GetImage().gameObject.SetActive(slotsHand[i].Card != null);
        }
        UpdateHand();
    }
    
    private void PlaceSolution(TileData obj)
    {
        if (selectedCard == null) return;
        if (selectedCard.Card == null)
        {
            selectedCard.removeSelection();
            selectedCard = null;
            return;
        }

        if (obj.PiecePlaced)
        {
            selectedCard.GetImage().transform.position = selectedCard.transform.position;
            selectedCard.removeSelection();
            selectedCard = null;
            return;
        }

        OnCardTryToPlaceEvent?.Invoke(obj, selectedCard);
    }
    
    
    private void OnCardWasPointed(CardHand card)
    {
        if (selectedCard == card)
        {
            selectedCard.removeSelection();
            selectedCard = null;
            OnCardSelectedEvent?.Invoke(null);
            MovementManager.Instance.SetSelectedCard(null);
            return;
        }
        if (selectedCard != null)
        {
            selectedCard.removeSelection();
        }
        selectedCard = card;
        selectedCard.addSelection();
        OnCardSelectedEvent?.Invoke(card);
        MovementManager.Instance.SetSelectedCard(card);
    }
    public int GetMaxCard()
    {
        return slotsHand.Count;
    }

    public void AddCard(CardInfoInstance _card, out CardInfoInstance cardInfoInstance)
    {
        CardInfoInstance card = new CardInfoInstance(_card.So);
        cardInfoInstance = card;

        Hand.Add(card);
    }
    
    public void UpdateHand()
    {
        for (int i = 0; i < slotsHand.Count; i++)
        {
            if (i < Hand.Count)
            {
                slotsHand[i].SetCard(Hand[i]);
                
            }
            else
            {
                slotsHand[i].EmptyCard();
            }
        }
    }

    public CardHand getAvailableSlot()
    {
        List<CardHand> availableSlots = slotsHand.Where(t => t.Occupied == false).ToList();
        return (availableSlots.Count > 0 ) ? availableSlots[0] : null;
    }
}