using System;
using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class MoveToDestination : Node
{
    private HeroBlackboard blackboard;
    
    public MoveToDestination(HeroBlackboard _blackboard)
    {
        blackboard = _blackboard;
    }
    
    public override NodeState Evaluate(Node root)
    {
        if (blackboard.directionToMove == DirectionToMove.None)
            return NodeState.Failure;

        switch (blackboard.directionToMove)
        {
            case DirectionToMove.Up:
                blackboard.hero.indexHeroY++;
                break;
            case DirectionToMove.Down:
                blackboard.hero.indexHeroY--;
                break;
            case DirectionToMove.Left:
                blackboard.hero.indexHeroX--;
                break;
            case DirectionToMove.Right:
                blackboard.hero.indexHeroX++;
                break;
        }
        Vector3 pos = Vector3.zero;
        blackboard.hero.mapManager.GetWorldPosFromTilePos(blackboard.hero.indexHeroX, blackboard.hero.indexHeroY, out pos);

        blackboard.hero.Move(pos);
        return NodeState.Success;
    }
}
