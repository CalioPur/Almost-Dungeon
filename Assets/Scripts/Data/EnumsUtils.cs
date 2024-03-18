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

public enum oldPerso
{
    HurryForTheExit,
    TheExplorer,
    TheKiller,
    TheSissy,
    MoveToHero,
    Nothing
}

public enum Personnalities
{
    INDECIS, // Lorsqu'il y a plusieurs tuiles menant à une zone inexplorée dans son champ de vision, se déplace vers la plus proche.
    IMPATIENT // Lorsqu'il se trouve à plus de 5 tuiles de la sortie, entre en RAGE.
}

public enum VisionType
{
    LIGNEDROITE, // Le héros voit en ligne droite dans les 4 directions
    BIGLEUX, // Le hero se déplace de manière aléatoire sur les tile non visité adjacentes à sa position sinon complètement aléatoire
    CLAIRVOYANT, // Le hero va au chemin le plus court vers son objectif
}

public enum Aggressivity
{
    PEUREUX, // L'aventurier n'attaque pas et s'éloigne des minions s'il peut poursuivre l'exploration du donjon (c'est-à-dire s'il peut encore découvrir une zone inexplorée)
    COURAGEUX, // L'aventurier va attaquer coute que coute l'ennemi le plus proche dans son champ de vision, meme s'il n'entrave pas son exploration du donjon.
    NONE, // L'aventurier n'attaque pas
}

public enum Objectives
{
    EXPLORATION, // L'aventurier cherche à explorer le donjon
    SORTIE, // L'aventurier cherche à atteindre la sortie
}