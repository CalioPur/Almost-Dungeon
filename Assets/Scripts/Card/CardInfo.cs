using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardInfo", menuName = "ScriptableObjects/CardInfo", order = 1)]
public class CardInfo : ScriptableObject
{
    public Sprite img;
    public int nbToBuild;
    public bool DoorOnTop;
    public bool DoorOnBottom;
    public bool DoorOnLeft;
    public bool DoorOnRight;
    public string description;
    public int Rotation;

    public void init(CardInfo info)
    {
        img = info.img;
        nbToBuild = info.nbToBuild;
        DoorOnTop = info.DoorOnTop;
        DoorOnBottom = info.DoorOnBottom;
        DoorOnLeft = info.DoorOnLeft;
        DoorOnRight = info.DoorOnRight;
        description = info.description;
        Rotation = info.Rotation;
    }
    
    public void addRotation()
    {
        Rotation += 90;
        if (Rotation >= 360) Rotation = 0;

        bool Tmp = DoorOnTop; // counter-clockwise rotation

        DoorOnTop = DoorOnRight;
        DoorOnRight = DoorOnBottom;
        DoorOnBottom = DoorOnLeft;
        DoorOnLeft = Tmp;
    }
}