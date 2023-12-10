using System;

[Serializable]
public enum ItemDrop
{
    Key,
}

[Serializable]
public enum TrapType
{
    Web,
    Pyke,
    BasicCaC,
    Archer,
    Skeleton,
    Slime,
    None
}

public enum DirectionToMove
{
    Left,
    Right,
    Up,
    Down,
    None,
    Error
}