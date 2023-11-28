using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonSO : ScriptableObject
{
    [field: Header("Deck")]
    [field: SerializeField]
    public List<CardInfo> Deck { get; private set; } = new();
    [field: SerializeField] public int initialNbCardInHand { get; private set; }

    [field: Header("Heroes Data")]
    [field: SerializeField] public HeroesInfo HeroesInfo { get; private set; }
    [field: SerializeField] public int nbHealthHeroInitial { get; private set; }
    
    [field: Header("Others")]
    [field: SerializeField] public float tickData { get; private set; }
    [field: SerializeField] public Vector2Int clampedSpawnEnterDungeonX { get; private set; }
    [field: SerializeField] public Vector2Int clampedSpawnEnterDungeonY { get; private set; }
}