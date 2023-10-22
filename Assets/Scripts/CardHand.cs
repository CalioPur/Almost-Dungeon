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
    [HideInInspector] public CardInfo Card;


    [SerializeField] private Image img;

    [Header("Color settings")] [SerializeField]
    private Color HoverColor = Color.gray;

    [SerializeField] private Color NormalColor = Color.white;
    [SerializeField] private Color SelectedColor = Color.yellow;

    [Header("Description")] [SerializeField]
    private Transform BackgroundDescription;

    [SerializeField] private TMP_Text DescriptionText;

    private bool isSelected = false;
    private int Rotation;


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
        if (eventData.button == PointerEventData.InputButton.Right) return; // right click used for rotation
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
        Rotation = 0;
        isSelected = false;
        Occupied = false;
    }

    public void InitCard(CardInfo _card)
    {
        img.transform.rotation = Quaternion.Euler(0, 0, 0);
        Card = _card;
        img.sprite = Card.img;
        img.color = NormalColor;
        Rotation = 0;
        isSelected = false;
        DescriptionText.text = Card.description;
    }

    public Vector3 GetRotation()
    {
        return new Vector3(0, Rotation);
    }
    
    public int GetNbRotation()
    {
        return Rotation / 90;
    }

    public void addRotation()
    {
        Rotation += 90;
        if (Rotation >= 360) Rotation = 0;

        bool Tmp = Card.DoorOnTop; // clockwise rotation

        Card.DoorOnTop = Card.DoorOnRight;
        Card.DoorOnRight = Card.DoorOnBottom;
        Card.DoorOnBottom = Card.DoorOnLeft;
        Card.DoorOnLeft = Tmp;
    }

    public Image GetImage()
    {
        return img;
    }

    public void ChangeSelection(bool newSelection)
    {
        isSelected = newSelection;
        if (isSelected)
        {
            img.color = SelectedColor;
        }
        else
        {
            img.color = NormalColor;
        }
    }
}