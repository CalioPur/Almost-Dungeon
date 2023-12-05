using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeckManager : MonoBehaviour
{
    public static event Action<CardInfo[]> DistributeCardEvent;

    [Header("Deck builder")] [SerializeField]
    public List<CardToBuild> deckToBuild = new();

    [Header("Values initializer")] [SerializeField]
    public int nbCardOnStartToDraw = 3;

    [SerializeField] private float TimerBeforeDrawCard = 2;
    [SerializeField] private float TimerAnimationDrawCard = 0.2f;

    [SerializeField] private RectTransform DeckTr;
    [SerializeField] private HandsManager handsManager;

    private List<CardInfoInstance> deckCreate = new();
    private int cptCardsObtained = 0;
    private Coroutine currentlyDrawing;
    private Vector3 centerDeck;

    private void Awake()
    {
        GameManager.OnGameStartEvent += BeginToDraw;
        MovementManager.OnFinishToPose += RemoveCard;
        HandsManager.OnCardWasDiscardedEvent += RemoveCard;
    }

    private void OnDisable()
    {
        GameManager.OnGameStartEvent -= BeginToDraw;
        MovementManager.OnFinishToPose -= RemoveCard;
        HandsManager.OnCardWasDiscardedEvent -= RemoveCard;
    }

    private void RemoveCard(CardHand _)
    {
        cptCardsObtained--;
    }

    private void RemoveCard(CardInfoInstance _)
    {
        cptCardsObtained--;
    }

    void Start()
    {
        cptCardsObtained = 0;
        centerDeck = DeckTr.position - new Vector3(-1 * DeckTr.rect.width * 0.5f, DeckTr.rect.height * 0.5f, 0);
        InitDeck();
        ShuffleDeck();
        StartCoroutine(DrawStartedCard());
    }

    private void BeginToDraw()
    {
        CardInfo[] deck = new CardInfo[deckCreate.Count];
        for (int i = 0; i < deckCreate.Count; i++)
        {
            deck[i] = deckCreate[i].So;
        }

        DistributeCardEvent?.Invoke(deck);
        StartCoroutine(CheckDrawCard());
    }

    private IEnumerator CheckDrawCard()
    {
        yield return new WaitForSeconds(TimerBeforeDrawCard);
        if (deckCreate.Count <= 0)
        {
            InitDeck();
            ShuffleDeck();
        }

        if (cptCardsObtained < handsManager.GetMaxCard())
            DrawCard();

        if (currentlyDrawing != null)
            StopCoroutine(currentlyDrawing);
        currentlyDrawing = StartCoroutine(CheckDrawCard());
    }

    private void InitDeck()
    {
        deckCreate = new List<CardInfoInstance>();
        foreach (var card in deckToBuild)
        {
            for (int i = 0; i < card.nbToBuild; i++)
            {
                CardInfoInstance instance = card.cardToBuild.CreateInstance();
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
                int randomIndex = Random.Range(i, deckCreate.Count);
                deckCreate[i] = deckCreate[randomIndex];
                deckCreate[randomIndex] = temp;
            }
        }
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

    private void DrawCard()
    {
        if (deckCreate.Count == 0 || cptCardsObtained >= handsManager.GetMaxCard()) return;

        cptCardsObtained++;
        CardInfoInstance newCard = null;
        if (deckCreate.Count <= 0) return;
        handsManager.AddCard(deckCreate[0], out newCard);
        CardHand availableSlot = handsManager.getAvailableSlot();
        availableSlot.GetImage().enabled = true;
        StartCoroutine(AnimationDrawCard(availableSlot, newCard));
        deckCreate.RemoveAt(0);
    }

    IEnumerator AnimationDrawCard(CardHand Slot, CardInfoInstance card)
    {
        Slot.GetImage().gameObject.SetActive(true);
        Slot.GetTr().position = centerDeck;
        Slot.GetTr().DOMove(Slot.transform.position, TimerAnimationDrawCard);
        yield return new WaitForSeconds(TimerAnimationDrawCard);
        handsManager.UpdateHand();
    }
}