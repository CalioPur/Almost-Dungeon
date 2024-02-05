using BehaviourTree;
using UnityEngine;

public class BTFighter : MinionBTBase
{
    protected override Node InitTree()
    {
        if (!blackboard) Debug.LogError("Blackboard is null");

        origin = new Sequence(
            new Selector(
                new Sequence(
                    new CheckifFrontOfHero(blackboard),
                    new AttackHero(blackboard, AttackType.Physical)
                ),
                new Sequence(
                        
                    new HeroIsInSight(blackboard),
                    new MoveInDirection(blackboard)
                )
            )
        );
        return origin;
    }
}