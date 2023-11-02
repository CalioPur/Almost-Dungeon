using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "minionSO", menuName = "ScriptableObjects/minionSO", order = 1)]
public class minionSOScript : TrapSO
{
    public MinionInstance CreateInstance()
    {
        return new MinionInstance(this);
    }
}



public class MinionInstance
{
    public minionSOScript So { get; }

    public int CurrentHealthPoint;
    public int damagePoint;
    public MinionInstance(minionSOScript info)
    {
        So = info;
        CurrentHealthPoint = info.health;
        damagePoint = info.damage;
    }
}

