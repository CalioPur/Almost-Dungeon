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
    [SerializeField] private List<ItemSpritesDictionary> itemSpritesDictionary;
    

    private void Awake()
    {
        
        SetSpritesInDictionary();
    }

    private void SetSpritesInDictionary()
    {
        
    }

    public void SetItem(string itemName)
    {
        
    }
}

