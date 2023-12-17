using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSO", menuName = "ScriptableObjects/CreationDungeonTool/LevelSO", order = 7)]

public class LevelSO : ScriptableObject
{
    [SerializeField] public List<TilePresetSO> terrains;
    [SerializeField] public List<HeroSO> heros;
    [SerializeField] public List<DeckSO> decks;
    
    //-----TEMPORAIRE-----//
    [SerializeField] public Vector2Int ClampSpawnPositionX;
    [SerializeField] public Vector2Int ClampSpawnPositionY;
}