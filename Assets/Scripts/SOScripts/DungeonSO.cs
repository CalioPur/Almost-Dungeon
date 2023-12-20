using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonSO", menuName = "ScriptableObjects/CreationDungeonTool/DungeonSO", order = 5)]
public class DungeonSO : ScriptableObject
{
    [field: SerializeField] public List<LevelSO> levels;
}
