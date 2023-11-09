using BehaviourTree;
using UnityEngine;

public class BTArcher : MinionBTBase
{
    protected override Node InitTree()
    {
        if (!blackboard) Debug.LogError("Blackboard is null");

        origin = new Sequence(
            new GetHeroPos(blackboard),
            new IsInSight(blackboard),
            new AttackHero(blackboard)
        );
        return origin;
    }
}
