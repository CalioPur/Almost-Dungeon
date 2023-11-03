using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

[Serializable]
public class CardsManager : MonoBehaviour
{
    public static event Action<TileData, CardHand> OnCardTryToPlaceEvent;


    [Header("Deck builder")] [SerializeField]
    private List<CardInfo> deckToBuild = new();

    [Header("Values initializer")] [SerializeField]
    private int nbCardOnStartToDraw = 3;

    [SerializeField] private float TimerBeforeDrawCard = 2;
    [SerializeField] private float TimerAnimationDrawCard = 0.2f;

    [Header("References")] [SerializeField]
    private List<CardHand> slotsHand;

    [SerializeField] private Transform DeckTr;

    private List<CardInfoInstance> deckCreate = new();
    private List<CardInfoInstance> Hand = new();
    private CardHand selectedCard;
    private int cptCardsObtained = 0;
    private int nbStackingCard = 0;

    private void Awake()
    {
        GameManager.OnGameStartEvent += BeginToDraw;
    }

    private void BeginToDraw()
    {
        StartCoroutine(CheckDrawCard());
    }

    void Start()
    {
        CardHand.OnCardSelectedEvent += CartSelected;
        DragAndDropManager.OnTileSelectedEvent += PlaceSolution;
        MapManager.OnCardTryToPlaceEvent += RemoveCard;
        cptCardsObtained = 0;
        nbStackingCard = nbCardOnStartToDraw;

        InitDeck();
        InitSlots();
        ShuffleDeck();
        StartCoroutine(DrawStartedCard());
    }

    private void RemoveCard(TileData tileData, CardHand cardHand, bool canBePlaced)
    {
        if (!canBePlaced) return;
        cptCardsObtained--;
        DrawAllCards();
        StartCoroutine(CheckDrawCard());
        selectedCard = null;
    }

    private void PlaceSolution(TileData obj)
    {
        if (selectedCard == null) return;

        if (obj.PiecePlaced) return;
        OnCardTryToPlaceEvent?.Invoke(obj, selectedCard);
    }

    IEnumerator DrawStartedCard()
    {
        yield return new WaitForSeconds(TimerAnimationDrawCard);
        for (int i = 0; i < nbCardOnStartToDraw; i++)
        {
            DrawCard();
            yield return new WaitForSeconds(TimerAnimationDrawCard);
        }
    }

    private void InitSlots()
    {
        for (int i = 0; i < slotsHand.Count; i++)
        {
            slotsHand[i].EmptyCard();
        }
    }

    private bool CheckHandFull()
    {
        for (int i = 0; i < slotsHand.Count; i++)
        {
            if (slotsHand[i].Occupied == false)
                return false;
        }

        return true;
    }

    private void DrawAllCards()
    {
        return;
        for (int i = 0; i < nbStackingCard; i++)
        {
            if (deckCreate.Count > 0 && cptCardsObtained < slotsHand.Count)
            {
                DrawCard();
            }
        }
    }

    private IEnumerator CheckDrawCard()
    {
        if (deckCreate.Count <= 0 || cptCardsObtained >= slotsHand.Count) yield break;
        yield return new WaitForSeconds(TimerBeforeDrawCard);
        nbStackingCard++;
        if (deckCreate.Count <= 0 || cptCardsObtained >= slotsHand.Count) yield break;
        Debug.Log("stack: " +nbStackingCard);
        if (nbStackingCard > 0)
            DrawCard();
        StartCoroutine(CheckDrawCard());
    }

    IEnumerator AnimationDrawCard(CardHand Slot)
    {
        Slot.GetImage().gameObject.SetActive(true);
        Slot.GetImage().transform.position = DeckTr.position;
        Slot.GetImage().transform.DOMove(Slot.transform.position, TimerAnimationDrawCard);
        yield return new WaitForSeconds(TimerAnimationDrawCard);
    }

    private void DrawCard()
    {
        if (deckCreate.Count == 0 || cptCardsObtained >= slotsHand.Count) return;

        cptCardsObtained++;
        nbStackingCard--;
        CardInfoInstance card = deckCreate[0];
        if (deckCreate.Count > 0)
        {
            Hand.Add(deckCreate[0]);
            deckCreate.RemoveAt(0);
        }

        if (deckCreate.Count == 0)
            DeckTr.gameObject.SetActive(false);

        foreach (var t in slotsHand.Where(t => t.Occupied == false))
        {
            StartCoroutine(AnimationDrawCard(t));
            t.InitCard(card);
            t.Occupied = true;
            break;
        }
    }

    private void InitDeck()
    {
        deckCreate = new List<CardInfoInstance>();
        foreach (var card in deckToBuild)
        {
            for (int i = 0; i < card.nbToBuild; i++)
            {
                CardInfoInstance instance = card.CreateInstance();
                deckCreate.Add(instance);
            }
        }
    }

    private void ShuffleDeck()
    {
        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < deckCreate.Count; i++)
            {
                CardInfoInstance temp = deckCreate[i];
                int randomIndex = UnityEngine.Random.Range(i, deckCreate.Count);
                deckCreate[i] = deckCreate[randomIndex];
                deckCreate[randomIndex] = temp;
            }
        }
    }

    public void CartSelected(CardHand imgSelected)
    {
        if (imgSelected == null)
        {
            if (selectedCard != null)
            {
                selectedCard.ChangeSelection(false);
                selectedCard = null;
            }

            return;
        }

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

    private void RotateSelection(bool direction)
    {
        if (selectedCard != null)
        {
            selectedCard.GetImage().transform.Rotate(0, 0, 90 * (direction ? 1 : -1));
            selectedCard.Card.AddRotation(direction);
        }
    }

    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            RotateSelection(true);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            RotateSelection(false);
        }
    }
}