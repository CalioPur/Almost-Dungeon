using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public class ItemSpritesDictionary
{
    public string itemName;
    public Sprite itemSprite;
}
public class UI_HeroItem : MonoBehaviour
{
    public List<ItemSpritesDictionary> itemSpritesDictionary;
    Image itemImage;

    private void Awake()
    {
        itemImage = GetComponent<Image>();
        
        SetSpritesInDictionary();
    }

    private void SetSpritesInDictionary()
    {
        
    }

    public void SetItem(string itemName)
    {
        itemImage.sprite = itemSpritesDictionary.Find(x => x.itemName == itemName).itemSprite;
    }
}

