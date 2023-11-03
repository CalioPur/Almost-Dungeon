using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "CardInfo", menuName = "ScriptableObjects/CardInfo", order = 1)]
public class CardInfo : ScriptableObject
{
    [Header("Visual")]
    public Sprite imgOnHand;
    public Sprite imgOnMap;
    
    [Header("spawn parameters")]
    public int nbToBuild;
    public int nbMinionOnCard;
    public bool Web;
    public bool Pyke;
    
    [Header("Doors")]
    public bool DoorOnTop;
    public bool DoorOnBottom;
    public bool DoorOnLeft;
    public bool DoorOnRight;
    
    [Header("Others")]
    public string description;
    public int Rotation;
    
    public CardInfoInstance CreateInstance()
    {
        return new CardInfoInstance(this);
    }
}

//[Serializable]
public class CardInfoInstance
{
    public CardInfo So { get;}
    public int Rotation;
    public bool DoorOnTop { get;  set; }
    public bool DoorOnBottom { get; set; }
    public bool DoorOnLeft { get;  set; }
    public bool DoorOnRight { get;  set; }
    
    public event Action OnRotationChangedEvent; 
    
    public CardInfoInstance(CardInfo info)
    {
        So = info;
        Rotation = info.Rotation;
        DoorOnTop = info.DoorOnTop;
        DoorOnBottom = info.DoorOnBottom;
        DoorOnLeft = info.DoorOnLeft;
        DoorOnRight = info.DoorOnRight;
    }
    
    public void AddRotation(bool NotClockwise)
    {
        Rotation += 90 * (NotClockwise ? 1 : -1);
        if (Rotation < 0) Rotation = 270;
        if (Rotation >= 360) Rotation = 0;

        bool Tmp = DoorOnTop; // counter-clockwise rotation

        
        if (NotClockwise)
        {
            DoorOnTop = DoorOnRight;
            DoorOnRight = DoorOnBottom;
            DoorOnBottom = DoorOnLeft;
            DoorOnLeft = Tmp;
        }
        else
        {
            DoorOnTop = DoorOnLeft;
            DoorOnLeft = DoorOnBottom;
            DoorOnBottom = DoorOnRight;
            DoorOnRight = Tmp;
        }
        
        OnRotationChangedEvent?.Invoke();
        
        
    }
}