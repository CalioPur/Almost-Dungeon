using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct TilePreset
{
    public Vector2Int position;
    public CardInfo cardInfo;
    public int rotation;
}

[CreateAssetMenu(fileName = "DungeonSO", menuName = "ScriptableObjects/DungeonSO", order = 5)]

public class DungeonSO : ScriptableObject
{
    
    [field: Header("Deck")]
    [field: SerializeField]
    public List<CardToBuild> Deck { get; private set; } = new();
    [field: Range(1,6)]
    [field: SerializeField] public int initialNbCardInHand { get; private set; }

    [field: Header("Heroes Data")]
    [field: SerializeField] public HeroesInfo HeroesInfo { get; private set; }
    [field: SerializeField] public int nbHealthHeroInitial { get; private set; }
    
    [field: Header("Others")]
    [field: SerializeField] public float tickData { get; private set; }
    [field: SerializeField] public Vector2Int clampedSpawnEnterDungeonX { get; private set; }
    [field: SerializeField] public Vector2Int clampedSpawnEnterDungeonY { get; private set; }
    
    [field: Header("Dungeon Preset")]
    [field: SerializeField] public List<TilePreset> dungeonPreset { get; private set; } = new();
}