using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class HandsManager : MonoBehaviour
{
    public static event Action<CardHand> OnCardSelectedEvent;
    
    [Header("References")] [SerializeField]
    private List<CardHand> slotsHand;

    private List<CardInfoInstance> Hand = new();
    private CardHand selectedCard;

    private void Start()
    {
        InitSlots();
    }
        
    private void InitSlots()
    {
        for (int i = 0; i < slotsHand.Count; i++)
        {
            slotsHand[i].EmptyCard();
            slotsHand[i].OnCardWasPointedEvent += OnCardWasPointed;
        }
    }
    
    private void OnCardWasPointed(CardHand card)
    {
        if (selectedCard == card)
        {
            selectedCard.removeSelection();
            selectedCard = null;
            OnCardSelectedEvent?.Invoke(null);
            DragAndDropManager.Instance.SetSelectedCard(null);
            return;
        }
        if (selectedCard != null)
        {
            selectedCard.removeSelection();
        }
        selectedCard = card;
        selectedCard.addSelection();
        OnCardSelectedEvent?.Invoke(card);
        DragAndDropManager.Instance.SetSelectedCard(card);
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