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
    FireCamp,
    Wolf,
    Laden,
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

public enum AttackType
{
    Physical,
    Fire,
}