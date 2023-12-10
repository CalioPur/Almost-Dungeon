using System;

[Serializable]
public struct EnemiDataOnHand
{
    public TrapType type;
    public int indexOffsetTile;
}

[Serializable]
public struct ListOfTraps
{
    public TrapType type;
    public TrapData prefab;
}
