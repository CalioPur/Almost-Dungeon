
using BehaviourTree;
using UnityEngine;

public class SimpleHeroBT : BehaviourTree.Tree
{
    [SerializeField] private HeroBlackboard BB;
    
    protected override Node InitTree()
    {
        if (!BB) Debug.LogError("Blackboard is null");
        
        origin = new Selector(
            /*,
            new Sequence(
                new CanAttack(),
                new AttackCaC()
            ),*/
            new Sequence(
                new FindMinionInCaC(BB),
                new ChooseTargetToHitOnCaC(BB),
                new AttackMinionOnCaC(BB),
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
