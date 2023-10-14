using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotHandManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Card card;
    
    [SerializeField] private Image img;

    private bool isSelected = false;
    private Color HoverColor = Color.gray;
    private Color NormalColor = Color.white;
    private Color SelectedColor = Color.yellow;
    private int Rotation;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isSelected) img.color = HoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelected) img.color = NormalColor;
    }
    
    public Vector3 GetRotation()
    {
        return new Vector3(0, Rotation);
    }
    
    public void addRotation()
    {
        Rotation += 90;
        if (Rotation >= 360) Rotation = 0;
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

    public void OnPointerClick(PointerEventData eventData)
    {
        DeckManager.Instance.CartSelected(this);
        //DeckManager.OnCardSelectedEvent?.Invoke(img);

    }
}