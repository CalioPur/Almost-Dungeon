using BehaviourTree;
using UnityEngine;
using Tree = BehaviourTree.Tree;

public class BTBarbare : Tree
{
    [SerializeField] private HeroBlackboard BB;
    
    protected override Node InitTree()
    {
        if (!BB) Debug.LogError("Blackboard is null");
        
        origin = new Selector(
            new Sequence(
                new FindMinionInCaC(BB),
                new ChooseTargetToHit(BB, false, false),
                new AttackMinion(BB),
                new CheckTargetIsAlive(BB)
            ),
            new HeroMovementBehavior(BB)
        );
        return origin;
    }
}

