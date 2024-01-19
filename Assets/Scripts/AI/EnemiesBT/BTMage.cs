using BehaviourTree;
using UnityEngine;
using Tree = BehaviourTree.Tree;

public class BTMage : Tree
{
    [SerializeField] private HeroBlackboard BB;
    
    protected override Node InitTree()
    {
        if (!BB) Debug.LogError("Blackboard is null");
        
        origin = new Selector(
            new Sequence(
                new GetDirectionOfNearestEnemyInSight(BB),
                new AttackMinionsInLine(BB)
                // new ChooseTargetToHit(BB, true, true),
                // new AttackMinion(BB),
                // new CheckTargetIsAlive(BB)
            ),
            new HeroMovementBehavior(BB)
        );
        return origin;
    }
}