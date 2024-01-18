using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "minionSO", menuName = "ScriptableObjects/minionSO", order = 1)]
public class EnemySo : ScriptableObject
{
    [field:SerializeField] public int health { get; private set; }
    [field:SerializeField]public int damage { get; private set; }
    [field:SerializeField]public int range { get; private set; }
    [field:SerializeField]public byte targetPriority { get; private set; }
    
    [field:SerializeField]public string attackSound { get; private set; }
    [field:SerializeField]public string deathSound { get; private set; }
    
    public EnemyInstance CreateInstance()
    {
        return new EnemyInstance(this);
    }
}


public class EnemyInstance
{
    public EnemySo So { get; }

    public int CurrentHealthPoint;
    public EnemyInstance(EnemySo info)
    {
        So = info;
        CurrentHealthPoint = info.health;
    }
}

