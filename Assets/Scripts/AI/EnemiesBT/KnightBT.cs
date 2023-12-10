using BehaviourTree;
using UnityEngine;
using Tree = BehaviourTree.Tree;

public class KnightBT : Tree
{
    [SerializeField] private HeroBlackboard BB;

    protected override Node InitTree()
    {
        if (!BB) Debug.LogError("Blackboard is null");

        origin = new Selector
        (
            new Sequence
            (
                new FindMinionInCaC(BB),
                new ChooseTargetToHit(BB, false, false),
                new AttackMinion(BB),
                new CheckTargetIsAlive(BB)
            ),
            new Sequence
            (
                new CheckIfDoorToRemember(BB),
                new Selector
                (
                    // new Sequence
                    // (
                    //     //faire ici le deplacement vers les doors si j'en ai croisé une et que je suis pas en train de me deplacer vers une autre et que j'ai une clé dans mon inventaire
                    //     // je check si j'ai une porte dans mon blackboard et si je n'en suis pas une deja, je set ma destination vers la porte et si je n'ai pas de porte dans mon blackboard, j'abort
                    //     //si j'ai pas une clé j'abort la sequence
                    //     //si j'ai une clé je check si je suis sur une porte
                    //     //sinon je move vers la porte
                    // ),
                    new Sequence
                    (
                        new CheckDirectionToMove(BB),
                        new MoveToDestination(BB),
                        new CheckPlayerOutOfMap(BB)
                    )
                )
            )
        );
        return origin;
    }
}