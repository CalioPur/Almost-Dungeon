using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "minionSO", menuName = "ScriptableObjects/minionSO", order = 1)]
public class minionSOScript : ScriptableObject
{
    [field:SerializeField] public int health { get; private set; }
    [field:SerializeField]public int damage { get; private set; }
    [field:SerializeField]public int range { get; private set; }
    
    public MinionInstance CreateInstance()
    {
        return new MinionInstance(this);
    }
}



public class MinionInstance
{
    public minionSOScript So { get; }

    public int CurrentHealthPoint;

    public MinionInstance(minionSOScript info)
    {
        So = info;
        CurrentHealthPoint = info.health;
    }
}

