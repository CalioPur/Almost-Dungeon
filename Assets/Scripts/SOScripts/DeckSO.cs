using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "DeckSO", menuName = "ScriptableObjects/CreationDungeonTool/LevelComponents/DeckSO", order = 10)]
public class DeckSO : ScriptableObject
{
    public List<CardToBuild> deck;
    [SerializeField] public TextAsset[] deckDialogue;
}
