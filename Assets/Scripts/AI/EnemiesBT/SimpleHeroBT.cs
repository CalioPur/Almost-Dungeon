
using BehaviourTree;
using UnityEngine;

public class SimpleHeroBT : BehaviourTree.Tree
{
    [SerializeField] private HeroBlackboard BB;
    
    protected override Node InitTree()
    {
        if (!BB) Debug.LogError("Blackboard is null");
        
        origin = new Selector(
            new Sequence(
                new CheckDirectionToMove(BB),
                new MoveToDestination(BB),
                new CheckPlayerOutOfMap(BB)
            )/*,
            new Sequence(
                new CanAttack(),
                new AttackCaC()
            )*/
        );
        return origin;
    }
}
