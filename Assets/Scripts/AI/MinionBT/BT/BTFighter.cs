using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class BTFighter : MinionBTBase
{
    protected override Node InitTree()
    {
        if (!blackboard) Debug.LogError("Blackboard is null");
        print("test ?");
        origin = new Selector(
        );
        return origin;
    }
}
