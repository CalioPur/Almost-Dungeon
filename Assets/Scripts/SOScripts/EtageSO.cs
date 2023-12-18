using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EtageSO", menuName = "ScriptableObjects/CreationDungeonTool/EtageSO", order = 6)]

public class EtageSO : ScriptableObject
{
    [field: SerializeField] public List<LevelSO> Levels;
    [Range(1,6)] public int nbCardToDraw;
}
