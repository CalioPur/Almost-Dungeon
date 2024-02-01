using System;

[Serializable]
public struct EnemiDataOnHand
{
    public TrapType type;
    public bool canBeRevive;
    public int indexOffsetTile;
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
