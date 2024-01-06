using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct TilePresetStruct
{
    public Vector2Int position;
    public CardInfo cardInfo;
    public int rotation;
}

[CreateAssetMenu(fileName = "TilePresetSO", menuName = "ScriptableObjects/CreationDungeonTool/LevelComponents/TilePresetSO", order = 8)]
public class TilePresetSO : ScriptableObject
{
    [SerializeField] public List<TilePresetStruct> tilePresets;
    [SerializeField] public Vector2Int ClampSpawnPositionX;
    [SerializeField] public Vector2Int ClampSpawnPositionY;
    [SerializeField] public int spawnRotation;
    [SerializeField] public TextAsset terrainDialogue;
}
