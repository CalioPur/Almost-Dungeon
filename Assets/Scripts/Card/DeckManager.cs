using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeckManager : MonoBehaviour
{
    public static event Action<CardInfo[]> DistributeCardEvent;

    [Header("Deck builder")] [SerializeField]
    public List<CardToBuild> deckToBuild = new();
    public List<CardToBuild> handToBuild = new();

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
    public static DeckManager Instance { get; set; }

    private void Awake()
    {
        GameManager.OnGameStartEvent += BeginToDraw;
        PlayerCardController.OnFinishToPose += RemoveCard;
        HandsManager.OnCardWasDiscardedEvent += RemoveCard;
        Instance = this;
    }

    private void OnDisable()
    {
        GameManager.OnGameStartEvent -= BeginToDraw;
        PlayerCardController.OnFinishToPose -= RemoveCard;
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
        centerDeck = DeckTr.position + new Vector3(-1 * DeckTr.rect.width * 0.5f, DeckTr.rect.height * 0.5f, 0);
        InitDeck();
        ShuffleDeck();
        StartCoroutine(InitHand());
    }

    public void RefreshHand(CardToBuild[] newHand)
    {
        handsManager.ClearHand();
        cptCardsObtained = 0;
        foreach (var card in newHand)
        {
            if (deckCreate.Count == 0 || cptCardsObtained >= handsManager.GetMaxCard()) return;
        
            cptCardsObtained++;
            CardInfoInstance newCard = null;
                
            handsManager.AddCard(card.cardToBuild.CreateInstance(), out newCard);
            CardHand availableSlot = handsManager.getAvailableSlot();
            availableSlot.GetImage().enabled = true;
            StartCoroutine(AnimationDrawCard(availableSlot, newCard));
        }
    }

    private IEnumerator InitHand()
    {
        yield return new WaitForSeconds(0.2f);
        if (handToBuild.Count > 0)
        {
            foreach (var card in handToBuild)
            {
                if (deckCreate.Count == 0 || cptCardsObtained >= handsManager.GetMaxCard()) yield break;
        
                cptCardsObtained++;
                CardInfoInstance newCard = null;
                
                handsManager.AddCard(card.cardToBuild.CreateInstance(), out newCard);
                CardHand availableSlot = handsManager.getAvailableSlot();
                availableSlot.GetImage().enabled = true;
                StartCoroutine(AnimationDrawCard(availableSlot, newCard));

            }
        }
        else
        {
            StartCoroutine(DrawStartedCard());
        }
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
        while (TickManager.Instance.TickOnPaused)
            yield return new WaitForEndOfFrame();

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
        CardHand availableSlot = handsManager.getAvailableSlot();
        if (availableSlot == null)
        {
            return;
        }
        cptCardsObtained++;
        CardInfoInstance newCard = null;
        if (deckCreate.Count <= 0) return;
        handsManager.AddCard(deckCreate[0], out newCard);
       

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