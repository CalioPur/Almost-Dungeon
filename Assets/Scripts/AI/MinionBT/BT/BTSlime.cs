using BehaviourTree;
using UnityEngine;

public class BTSlime : MinionBTBase
{
    protected override Node InitTree()
    {
        if (!blackboard) Debug.LogError("Blackboard is null");

        origin = new Sequence(
            new GetHeroPos(blackboard),
            new HeroIsInSight(blackboard),
            new MoveToHero(blackboard),
            new CheckifFrontOfHero(blackboard),
            new AttackHero(blackboard)
        );
        return origin;
    }
}
