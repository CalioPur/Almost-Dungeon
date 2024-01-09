using BehaviourTree;
using UnityEngine;

public class WolfBT : MinionBTBase
{
    protected override Node InitTree()
    {
        if (!blackboard) Debug.LogError("Blackboard is null");

        origin = new Sequence(
            new GetHeroPos(blackboard),
            new MoveToHero(blackboard),
            new MoveInDirection(blackboard),
            new CheckifFrontOfHero(blackboard),
            new AttackHero(blackboard, AttackType.Physical)
        );
        return origin;
    }
}