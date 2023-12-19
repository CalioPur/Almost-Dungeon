using BehaviourTree;
using UnityEngine;
using Tree = BehaviourTree.Tree;

public class BTArcherHero : Tree
{
    [SerializeField] private HeroBlackboard BB;
    [SerializeField] private Arrow arrow;
    
    protected override Node InitTree()
    {
        if (!BB) Debug.LogError("Blackboard is null");
        
        origin = new Selector(
            new Sequence(
                new ClearMinionTarget(BB),
                new FirstMinionInSight(BB),
                new ChooseTargetToHit(BB, false, false),
                new AttackMinion(BB),
                new CheckTargetIsAlive(BB)
            ),
            new HeroMovementBehavior(BB)
        );
        return origin;
    }
}
