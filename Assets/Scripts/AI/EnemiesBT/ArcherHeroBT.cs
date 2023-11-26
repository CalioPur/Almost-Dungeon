using BehaviourTree;
using UnityEngine;
using Tree = BehaviourTree.Tree;

public class ArcherHeroBT : Tree
{
    [SerializeField] private HeroBlackboard BB;
    
    protected override Node InitTree()
    {
        if (!BB) Debug.LogError("Blackboard is null");
        
        origin = new Selector(
            new Sequence(
                new ClearMinionTarget(BB),
                new FirstMinionInSight(BB),
                new ChooseTargetToHit(BB),
                new AttackMinion(BB),
                new CheckTargetIsAlive(BB)
            ),
            new Sequence(
                new CheckDirectionToMove(BB),
                new MoveToDestination(BB),
                new CheckPlayerOutOfMap(BB)
            )
        );
        return origin;
    }
}
