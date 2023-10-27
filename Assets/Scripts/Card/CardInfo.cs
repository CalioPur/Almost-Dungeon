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

    public void init(Sprite _img, int _nbToBuild, bool _DoorOnTop, bool _DoorOnBottom, bool _DoorOnLeft, bool _DoorOnRight, string _description)
    {
        img = _img;
        nbToBuild = _nbToBuild;
        DoorOnTop = _DoorOnTop;
        DoorOnBottom = _DoorOnBottom;
        DoorOnLeft = _DoorOnLeft;
        DoorOnRight = _DoorOnRight;
        description = _description;
    }
}