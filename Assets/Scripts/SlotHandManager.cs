using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotHandManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Image img;

    private bool isSelected = false;
    private Color HoverColor = Color.gray;
    private Color NormalColor = Color.white;
    private Color SelectedColor = Color.yellow;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Enter");
        if (!isSelected) img.color = HoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelected) img.color = NormalColor;
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