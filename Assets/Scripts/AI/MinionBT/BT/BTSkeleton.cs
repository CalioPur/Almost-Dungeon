using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class BTSkeleton : MinionBTBase
{
    protected override Node InitTree()
    {
        if (!blackboard) Debug.LogError("Blackboard is null");

        origin = new Sequence(
            new GetHeroPos(blackboard),
            new CheckifFrontOfHero(blackboard),
           new AppearOnWorld(blackboard),
            new AttackHero(blackboard)
        );
        return origin;
    }
}
