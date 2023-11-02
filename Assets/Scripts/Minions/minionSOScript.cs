using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "minionSO", menuName = "ScriptableObjects/minionSO")]
public class minionSOScript : ScriptableObject
{
    public int health { get; private set; }
    public int damage { get; private set; }
    public int range { get; private set; }
    
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

