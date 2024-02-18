using System;
using UnityEngine;

[Serializable]
public struct EnemiDataOnHand
{
    public TrapType type;
    public bool canBeRevive;
}

[Serializable]
public struct ListOfTraps
{
    public TrapType type;
    
    public TrapData prefab;
}

[Serializable]
public struct DoorLockedData
{
    public bool DoorOnTop;
    public bool DoorOnBottom;
    public bool DoorOnLeft;
    public bool DoorOnRight;
}

[Serializable]
public struct TutorialDialogData
{
    public Vector2Int tilePostion;
    public string Dialog;
    public DirectionToMove direction;
    public bool isExploding;
}
