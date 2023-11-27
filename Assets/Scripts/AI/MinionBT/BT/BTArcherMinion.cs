using BehaviourTree;
using UnityEngine;

public class BTArcherMinion : MinionBTBase
{
    protected override Node InitTree()
    {
        if (!blackboard) Debug.LogError("Blackboard is null");

        origin = new Sequence(
            new GetHeroPos(blackboard),
            new HeroIsInSight(blackboard),
            new CheckifMinionsIsNotMiddleOfTarget(blackboard),
            new AttackRandomTargetInTile(blackboard)
        );
        return origin;
    }
}
