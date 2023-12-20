using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardHand : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public event Action<CardHand> OnCardWasPointedEvent;
    
    public bool Occupied { get; set; } = false;
    public CardInfoInstance Card;


    [SerializeField] private Image img;
    [SerializeField] private RectTransform Tr;

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

    public void OnPointerDown(PointerEventData eventData)
    {
        //if (eventData.button == PointerEventData.InputButton.Right)
        //{
            //enleve la selection de la carte 
            //if (!isSelected) return;
            //isSelected = false;
            //img.color = NormalColor;
            //OnCardSelectedEvent?.Invoke(null);
        //}

        if (!Occupied) return;
        OnCardWasPointedEvent?.Invoke(this);
        BackgroundDescription.gameObject.SetActive(false);
    }
    
    public RectTransform GetTr()
    {
        return Tr;
    }

    public void EmptyCard()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        Card = null;
        img.sprite = null;
        ChangeSelection(false);
        img.gameObject.SetActive(false);
        isSelected = false;
        Occupied = false;
    }

    public void MoveCardTo(CardHand other)
    {
        InitCard(other.Card, false);
        //other.img.transform.position = transform.position;
        Occupied = other.Occupied;
        bool tmp = Card != null;
        img.enabled = Card != null;
        //     other.Card.CopyValues(Card);
        // RefreshCard();
    }

    public void RefreshCard()
    {
        img.sprite = Card.So.imgOnHand;
        img.color = NormalColor;
        DescriptionText.text = Card.So.description;
    }
    
    public void InitCard(CardInfoInstance _card, bool resetRotation = true)
    {
        if (resetRotation)
            img.transform.position = transform.position;
        Card = (_card != null) ? new CardInfoInstance(_card.So) : null;
        img.sprite = (_card != null) ? _card.So.imgOnHand : null;
        img.color = NormalColor;
        isSelected = false;
        DescriptionText.text = (_card != null) ? _card.So.description : "";
        if (!resetRotation && (_card != null))
        {
            img.transform.rotation = Quaternion.Euler(0, 0, 0);
            int nb = _card.Rotation / 90;
            //Card.Rotation = 0;
            for (int i = 0; i < nb; i++)
            {
                //Card.AddRotation(true);
                GetImage().transform.Rotate(0, 0, 90);
            }
        }
    }

    public Image GetImage()
    {
        return img;
    }
    
    public Sprite GetSprite()
    {
        return img.sprite;
    }

    public void ChangeSelection(bool newSelection)
    {
        isSelected = newSelection;
        img.color = isSelected ? SelectedColor : NormalColor;
    }

    public void removeSelection()
    {
        ChangeSelection(false);
    }
    
    public void addSelection()
    {
        ChangeSelection(true);
    }

    public void SetCard(CardInfoInstance cardInfoInstance)
    {
        Card = cardInfoInstance;
        img.sprite = cardInfoInstance.So.imgOnHand;
        img.gameObject.SetActive(true);
        Occupied = true;
    }
}