using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trap", menuName = "ScriptableObjects/TrapSO", order = 1)]
public class TrapSO : ScriptableObject
{
    [field:SerializeField] public int health { get; private set; }
    [field:SerializeField]public int damage { get; private set; }
    [field:SerializeField]public int range { get; private set; }
}
