using BehaviourTree;
using UnityEngine;

public class BTFighter : MinionBTBase
{
    protected override Node InitTree()
    {
        if (!blackboard) Debug.LogError("Blackboard is null");

        origin = new Sequence(
            new GetHeroPos(blackboard),
            new HeroIsInSight(blackboard),
            new MoveInDirection(blackboard),
            new CheckifFrontOfHero(blackboard),
            new AttackHero(blackboard)
        );
        return origin;
    }
}
