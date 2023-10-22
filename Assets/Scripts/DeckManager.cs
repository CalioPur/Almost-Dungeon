using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

[Serializable]
public class DeckManager : MonoBehaviour
{
    [Header("Deck builder")] [SerializeField]
    private List<CardInfo> deckToBuild = new();

    [Header("Values initializer")] [SerializeField]
    private int nbCardOnStartToDraw = 3;

    [SerializeField] private float TimerBeforeDrawCard = 2;
    [SerializeField] private float TimerAnimationDrawCard = 0.2f;

    [Header("References")] [SerializeField]
    private List<CardHand> slotsHand;

    [SerializeField] private Transform DeckTr;

    private List<CardInfo> deckCreate = new();
    private Coroutine drawCardCoroutine;
    private List<CardInfo> Hand = new();
    private CardHand selectedCard;

    void Start()
    {
        CardHand.OnCardSelectedEvent += CartSelected;

        InitDeck();
        InitSlots();
        ShuffleDeck();
        StartCoroutine(DrawStartedCard());
    }

    IEnumerator DrawStartedCard()
    {
        yield return new WaitForSeconds(TimerAnimationDrawCard);
        for (int i = 0; i < nbCardOnStartToDraw; i++)
        {
            DrawCard();
            yield return new WaitForSeconds(TimerAnimationDrawCard);
        }

        drawCardCoroutine = StartCoroutine(CheckDrawCard());
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

    private IEnumerator CheckDrawCard()
    {
        yield return new WaitForSeconds(TimerBeforeDrawCard);
        DrawCard();
        if (deckCreate.Count > 0 && CheckHandFull() == false)
            drawCardCoroutine = StartCoroutine(CheckDrawCard());
        else
        {
            StopCoroutine(drawCardCoroutine);
            drawCardCoroutine = null;
        }
    }

    IEnumerator AnimationDrawCard(CardHand Slot)
    {
        Slot.GetImage().gameObject.SetActive(true);
        Slot.GetImage().transform.position = DeckTr.position;
        Slot.GetImage().transform.DOMove(Slot.transform.position, TimerAnimationDrawCard);
        yield return new WaitForSeconds(TimerAnimationDrawCard);

        DrawCard();
    }

    private void DrawCard()
    {
        if (deckCreate.Count == 0 || CheckHandFull()) return;
        CardInfo card = deckCreate[0];
        if (deckCreate.Count > 0)
        {
            Hand.Add(deckCreate[0]);
            deckCreate.RemoveAt(0);
        }

        if (deckCreate.Count == 0)
            DeckTr.gameObject.SetActive(false);

        for (int i = 0; i < slotsHand.Count; i++)
        {
            if (slotsHand[i].Occupied == false)
            {
                StartCoroutine(AnimationDrawCard(slotsHand[i]));
                slotsHand[i].InitCard(card);
                slotsHand[i].Occupied = true;
                break;
            }
        }
    }

    private void InitDeck()
    {
        deckCreate = new List<CardInfo>();
        foreach (var card in deckToBuild)
        {
            for (int i = 0; i < card.nbToBuild; i++)
            {
                deckCreate.Add(card);
            }
        }
    }

    private void ShuffleDeck()
    {
        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < deckCreate.Count; i++)
            {
                CardInfo temp = deckCreate[i];
                int randomIndex = UnityEngine.Random.Range(i, deckCreate.Count);
                deckCreate[i] = deckCreate[randomIndex];
                deckCreate[randomIndex] = temp;
            }
        }
    }

    public void CartSelected(CardHand imgSelected)
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

    private void RotateSelection()
    {
        if (selectedCard != null)
        {
            selectedCard.GetImage().transform.Rotate(0, 0, 90);
            selectedCard.addRotation();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            RotateSelection();
        }
    }
}