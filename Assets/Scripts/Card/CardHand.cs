using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardHand : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public static event Action<CardHand> OnCardSelectedEvent;

    public bool Occupied { get; set; } = false;
    public CardInfoInstance Card;


    [SerializeField] public Image img;

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
        // if (eventData.button == PointerEventData.InputButton.Right)
        // {
        //     //enleve la selection de la carte 
        //     if (!isSelected) return;
        //     isSelected = false;
        //     img.color = NormalColor;
        //     OnCardSelectedEvent?.Invoke(null);
        // }

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
    
    public void MoveCardTo(CardHand other)
    {

        InitCard(other.Card, false);
        other.img.transform.position = transform.position;
        Occupied = other.Occupied;
        if (other.Occupied && other.isSelected)
        {
            other.removeSelection();
            addSelection();
        }
    }

    public void InitCard(CardInfoInstance _card, bool resetRotation = true)
    {
        img.transform.position = transform.position;
        img.transform.rotation = Quaternion.Euler(0, 0, 0);
        Card = _card;
        img.sprite = (_card != null) ? Card.So.imgOnHand : null;
        img.color = NormalColor;
        isSelected = false;
        DescriptionText.text = (_card != null) ? Card.So.description : "";
        if (!resetRotation && (_card != null))
        {
            int nb = Card.Rotation / 90;
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

    public void ChangeSelection(bool newSelection)
    {
        isSelected = newSelection;
        img.color = isSelected ? SelectedColor : NormalColor;
    }

    public void removeSelection()
    {
        if (!isSelected) return;
        OnCardSelectedEvent?.Invoke(null);
        DragAndDropManager.Instance.SetSelectedCard(null);
    }
    
    public void addSelection()
    {
        isSelected = true;
        OnCardSelectedEvent?.Invoke(this);
    }
}