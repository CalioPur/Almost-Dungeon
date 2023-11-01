using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class BTFighter : MinionBTBase
{
    protected override Node InitTree()
    {
        if (!blackboard) Debug.LogError("Blackboard is null");

        origin = new Sequence(
            new GetHeroPos(blackboard),
            new CheckDeplacement(blackboard)
        );
        print(blackboard.dir);
        return origin;
    }
}
