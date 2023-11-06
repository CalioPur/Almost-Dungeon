using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class CheckPlayerOutOfMap : Node
{
    private HeroBlackboard blackboard;

    public CheckPlayerOutOfMap(HeroBlackboard _blackboard)
    {
        blackboard = _blackboard;
    }

    public override NodeState Evaluate(Node root)
    {
        if (blackboard.hero.mapManager.CheckIfTileIsFree(blackboard.hero.GetIndexHeroPos()))
            return NodeState.Failure;

        blackboard.hero.OutOfMap();
        return NodeState.Success;
    }
}