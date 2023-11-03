using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trap", menuName = "ScriptableObjects/TrapSO", order = 1)]
public class TrapSO : ScriptableObject
{
    [field:SerializeField] public int health { get; private set; }
    [field:SerializeField]public int damage { get; private set; }
    [field:SerializeField]public int range { get; private set; }
    [field:SerializeField]public byte targetPriority { get; private set; }
    
    public TrapInstance CreateInstance()
    {
        return new TrapInstance(this);
    }
}


public class TrapInstance
{
    public TrapSO So { get; }

    public int CurrentHealthPoint;
    public int damagePoint;
    public TrapInstance(TrapSO info)
    {
        So = info;
        CurrentHealthPoint = info.health;
        damagePoint = info.damage;
    }
}