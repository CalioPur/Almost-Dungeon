using BehaviourTree;
using UnityEngine;

public class BTArcherMinion : MinionBTBase
{
    protected override Node InitTree()
    {
        if (!blackboard) Debug.LogError("Blackboard is null");

        origin = new Sequence(
            new HeroIsInSight(blackboard),
            new Selector(
                new AttackMinionsIsMiddleOfTarget(blackboard, AttackType.Fire),
                new AttackRandomTargetInTile(blackboard, AttackType.Fire)
            )
        );
        return origin;
    }
}