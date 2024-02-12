using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class HandsManager : MonoBehaviour
{
    public static event Action<TileData, CardHand> OnCardTryToPlaceEvent;
    public static event Action<CardHand> OnCardWasDiscardedEvent;
    
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
        PlayerCardController.OnTilePosedEvent += MoveCard;
        PlayerCardController.OnFinishToPose += ReorganizeHand;
        PlayerCardController.OnTileSelectedEvent += PlaceSolution;
    }

    private void OnDisable()
    {
        PlayerCardController.OnTilePosedEvent -= MoveCard;
        PlayerCardController.OnFinishToPose -= ReorganizeHand;
        PlayerCardController.OnTileSelectedEvent -= PlaceSolution;
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
            
            
            
            slotsHand[i].GetImage().gameObject.transform.position = slotsHand[i+1].transform.position;
            slotsHand[i].GetImage().gameObject.transform.DOMove(slotsHand[i].transform.position, TickManager.Instance.calculateBPM());
        }
        UpdateHand();
    }
    
    private void PlaceSolution(TileData obj)
    {
        if (selectedCard == null) return;
        if (selectedCard.Card == null)
        {
            // selectedCard.removeSelection();
            // selectedCard = null;
            return;
        }

        if (obj.PiecePlaced)
        {
            // selectedCard.GetImage().transform.position = selectedCard.transform.position;
            // selectedCard.removeSelection();
            // selectedCard = null;
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
            PlayerCardController.Instance.SetSelectedCard(null);
            return;
        }
        if (selectedCard != null)
        {
            selectedCard.removeSelection();
        }
        selectedCard = card;
        selectedCard.addSelection();
        PlayerCardController.Instance.SetSelectedCard(card);
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
                slotsHand[i].RefreshCard();
                
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
    
    private void DiscardCard(CardHand obj)
    {
        MoveCard(null, obj.Card);
        ReorganizeHand(obj.Card);
        PlayerCardController.Instance.SetSelectedCard(null);
        OnCardWasDiscardedEvent?.Invoke(obj);
    }
}