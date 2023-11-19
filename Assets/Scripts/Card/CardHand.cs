using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardHand : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public static event Action<CardHand> OnCardSelectedEvent;

    public bool Occupied { get; set; } = false;
    public CardInfoInstance Card;


    [SerializeField] private Image img;

    [Header("Color settings")] [SerializeField]
    private Color HoverColor = Color.gray;

    [SerializeField] private Color NormalColor = Color.white;
    [SerializeField] private Color SelectedColor = Color.yellow;

    [Header("Description")] [SerializeField]
    private Transform BackgroundDescription;

    [SerializeField] private TMP_Text DescriptionText;

    private bool isSelected = false;


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!Occupied) return;
        if (!isSelected) img.color = HoverColor;
        BackgroundDescription.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!Occupied) return;
        if (!isSelected) img.color = NormalColor;
        BackgroundDescription.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            //enleve la selection de la carte 
            if (!isSelected) return;
            isSelected = false;
            img.color = NormalColor;
            OnCardSelectedEvent?.Invoke(null);
        }
        if (!Occupied) return;
        OnCardSelectedEvent?.Invoke(this);
        BackgroundDescription.gameObject.SetActive(false);
    }

    public void EmptyCard()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        Card = null;
        img.sprite = null;
        img.gameObject.SetActive(false);
        isSelected = false;
        Occupied = false;
    }

    public void InitCard(CardInfoInstance _card)
    {
        img.transform.rotation = Quaternion.Euler(0, 0, 0);
        Card = _card;
        img.sprite = Card.So.imgOnHand;
        img.color = NormalColor;
        isSelected = false;
        DescriptionText.text = Card.So.description;
    }

    public Image GetImage()
    {
        return img;
    }

    public void ChangeSelection(bool newSelection)
    {
        isSelected = newSelection;
        img.color = isSelected ? SelectedColor : NormalColor;
    }

    public void removeSelection()
    {
        isSelected = false;
        img.color = NormalColor;
        OnCardSelectedEvent?.Invoke(this);
    }
}