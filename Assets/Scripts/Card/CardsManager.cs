using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class CardsManager : MonoBehaviour
{
    public static event Action<TileData, CardHand> OnCardTryToPlaceEvent;
    public static event Action<CardInfo[]> DistributeCardEvent;


    [Header("Deck builder")] [SerializeField]
    public List<CardToBuild> deckToBuild = new();

    [Header("Values initializer")] [SerializeField]
    public int nbCardOnStartToDraw = 3;

    [SerializeField] private float TimerBeforeDrawCard = 2;
    [SerializeField] private float TimerAnimationDrawCard = 0.2f;

    [Header("References")] [SerializeField]
    private List<CardHand> slotsHand;

    [SerializeField] private Transform DeckTr;

    private List<CardInfoInstance> deckCreate = new();
    private List<CardInfoInstance> Hand = new();
    private CardHand selectedCard;
    private int cptCardsObtained = 0;

    private Coroutine currentlyDrawing;
    private List<CardInfoInstance> defausseCard = new();
    private int indexCardToDraw = 0;

    public static CardsManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
        GameManager.OnGameStartEvent += BeginToDraw;
    }

    private void OnEnable()
    {
        DragAndDropManager.OnTilePosedEvent += MoveCard;
        DragAndDropManager.OnFinishToPose += ReorganizeHand;
    }


    private void ReorganizeHand()
    {
        selectedCard = null;
        for (int i = indexCardToDraw; i < slotsHand.Count - 1; i++)
        {
            slotsHand[i].removeSelection();
            slotsHand[i].MoveCardTo(slotsHand[i + 1]);
        }
    }

    private void MoveCard(TileData _, CardInfoInstance instance)
    {
        indexCardToDraw = slotsHand.FindIndex(t => t.Card == instance);
        if (indexCardToDraw == -1) return;
        if (indexCardToDraw == slotsHand.Count - 1)
            slotsHand[indexCardToDraw].EmptyCard();
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

    void Start()
    {
        CardHand.OnCardSelectedEvent += CartSelected;
        DragAndDropManager.OnTileSelectedEvent += PlaceSolution;
        MapManager.OnCardTryToPlaceEvent += RemoveCard;
        cptCardsObtained = 0;

        InitDeck();
        InitSlots();
        ShuffleDeck();
        StartCoroutine(DrawStartedCard());
    }

    private void OnDisable()
    {
        CardHand.OnCardSelectedEvent -= CartSelected;
        DragAndDropManager.OnTileSelectedEvent -= PlaceSolution;
        MapManager.OnCardTryToPlaceEvent -= RemoveCard;
        GameManager.OnGameStartEvent -= BeginToDraw;
        DragAndDropManager.OnTilePosedEvent -= MoveCard;
        DragAndDropManager.OnFinishToPose -= ReorganizeHand;
    }

    private void RemoveCard(TileData tileData, CardHand cardHand, bool canBePlaced)
    {
        if (!canBePlaced)
        {
            cardHand.img.transform.position = cardHand.transform.position;
            cardHand.removeSelection();
            selectedCard = null;
            return;
        }

        cptCardsObtained--;
        selectedCard = null;
    }

    private void PlaceSolution(TileData obj)
    {
        if (selectedCard == null) return;

        if (obj.PiecePlaced)
        {
            selectedCard.img.transform.position = selectedCard.transform.position;
            selectedCard.removeSelection();
            selectedCard = null;
            return;
        }

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

    private bool DefausseToDeck()
    {
        if (defausseCard.Count <= 0) return false;
        foreach (var t in defausseCard)
        {
            deckCreate.Add(t);
        }

        defausseCard.Clear();
        DeckTr.gameObject.SetActive(true);
        return true;
    }

    private IEnumerator CheckDrawCard()
    {
        yield return new WaitForSeconds(TimerBeforeDrawCard);
        if (deckCreate.Count <= 0)
        {
            InitDeck();
        }

        // if (!DefausseToDeck())
        //     yield break;
        // if (cptCardsObtained >= slotsHand.Count)
        //     RemoveCardAtIndex(0);
        if (cptCardsObtained < slotsHand.Count)
            DrawCard();

        if (currentlyDrawing != null)
            StopCoroutine(currentlyDrawing);
        currentlyDrawing = StartCoroutine(CheckDrawCard());
    }

    // public void MoveCardToDefausse(int index, bool nextCanBeNull = false)
    // {
    //     for (int i = index; i < slotsHand.Count - 1; i++)
    //     {
    //         if ((slotsHand[i + 1].Card != null && slotsHand[i + 1].Card.So != null) || nextCanBeNull)
    //             slotsHand[i].MoveCardTo(slotsHand[i + 1]);
    //     }
    //
    //     slotsHand[^1].EmptyCard();
    // }

    // private void RemoveCardAtIndex(int index)
    // {
    //     slotsHand[index].removeSelection();
    //     CardInfoInstance defausseSO = new CardInfoInstance(slotsHand[index].Card.So);
    //     defausseCard.Add(defausseSO);
    //
    //     //MoveCardToDefausse(index);
    //     cptCardsObtained--;
    // }

    IEnumerator AnimationDrawCard(CardHand Slot, CardInfoInstance card)
    {
        Slot.GetImage().gameObject.SetActive(true);
        Slot.GetImage().transform.position = DeckTr.position;
        Slot.GetImage().transform.DOMove(Slot.transform.position, TimerAnimationDrawCard);
        yield return new WaitForSeconds(TimerAnimationDrawCard);
        Slot.InitCard(card);
    }

    private void DrawCard()
    {
        if (deckCreate.Count == 0 || cptCardsObtained >= slotsHand.Count) return;

        cptCardsObtained++;
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
            StartCoroutine(AnimationDrawCard(t, card));
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
                // selectedCard.ChangeSelection(false);
                // selectedCard = null;
            }
            else
            {
                if (selectedCard != null)
                {
                    selectedCard.ChangeSelection(false);
                }

                t.ChangeSelection(true);
                DragAndDropManager.Instance.SetSelectedCard(t);
                selectedCard = t;
            }
        }
    }

    public void SetSelectedCard(CardHand card)
    {
        selectedCard = card;
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

        if (Input.GetMouseButtonDown(1))
        {
            RotateSelection(false);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateSelection(false);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            RotateSelection(true);
        }
    }
}