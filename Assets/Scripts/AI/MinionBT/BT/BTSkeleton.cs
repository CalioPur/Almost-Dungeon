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
            new Selector(
                new Sequence(
                    new CheckOnDig(blackboard),
                    new Selector(
                        new Sequence(
                            new CheckIsAloneOnTile(blackboard),
                            new CheckIfReadyToUndig(blackboard),
                            new FinishToDig(blackboard),
                            new ResetDigSystem(blackboard)
                        ),
                        new Sequence(
                            new Selector(
                                new CheckIsAloneOnTile(blackboard),
                                new IsReadyToUndig(blackboard)
                            )
                        )
                    )
                ),
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
            )
        );
        return origin;
    }
}