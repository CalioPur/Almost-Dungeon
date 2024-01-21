using System;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class FindMinionInCaC : Node
{
    private HeroBlackboard blackboard;

    public FindMinionInCaC(HeroBlackboard _blackboard)
    {
        blackboard = _blackboard;
    }

    public override NodeState Evaluate(Node root)
    {
        List<TrapData> minions = new List<TrapData>();
        //je regarde si il y a un ou des ennemis dessus
        blackboard.Targets.Clear();
        if (blackboard.hero.mapManager.GetMonstersOnPos(blackboard.hero.GetIndexHeroPos(), out minions) &&
            minions is { Count: > 0 })
            blackboard.Targets.AddRange(minions);
        if (blackboard.hero.mapManager.DoorIsOpenAtPosition(blackboard.hero.GetIndexHeroPos(),
                     DirectionToMove.Right) && blackboard.hero.mapManager.GetMonstersOnPos(
                     blackboard.hero.GetIndexHeroPos() + new Vector2Int(1, 0),
                     out minions) && minions is { Count: > 0 })
            blackboard.Targets.AddRange(minions);
        if (blackboard.hero.mapManager.DoorIsOpenAtPosition(blackboard.hero.GetIndexHeroPos(),
                     DirectionToMove.Left) && blackboard.hero.mapManager.GetMonstersOnPos(
                     blackboard.hero.GetIndexHeroPos() + new Vector2Int(-1, 0),
                     out minions) && minions is { Count: > 0 })
            blackboard.Targets.AddRange(minions);
        if (blackboard.hero.mapManager.DoorIsOpenAtPosition(blackboard.hero.GetIndexHeroPos(),
                     DirectionToMove.Up) && blackboard.hero.mapManager.GetMonstersOnPos(
                     blackboard.hero.GetIndexHeroPos() + new Vector2Int(0, 1),
                     out minions) && minions is { Count: > 0 })
            blackboard.Targets.AddRange(minions);
        if (blackboard.hero.mapManager.DoorIsOpenAtPosition(blackboard.hero.GetIndexHeroPos(),
                     DirectionToMove.Down) && blackboard.hero.mapManager.GetMonstersOnPos(
                     blackboard.hero.GetIndexHeroPos() + new Vector2Int(0, -1),
                     out minions) && minions is { Count: > 0 })
            blackboard.Targets.AddRange(minions);
        
        if (blackboard.Targets.Count > 0)
        {
            return NodeState.Success;
        }
        return NodeState.Failure;
    }
}