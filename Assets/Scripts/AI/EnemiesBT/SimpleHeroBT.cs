
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
                new CheckDirectionToMove(BB),
                new MoveToDestination(BB),
                new CheckPlayerOutOfMap(BB)
            )
        );
        return origin;
    }
}
