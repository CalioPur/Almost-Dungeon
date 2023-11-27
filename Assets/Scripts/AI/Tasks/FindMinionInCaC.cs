using System.Collections;
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
        //je regarde si il y a un ou des ennemis dessus
        blackboard.hero.mapManager.GetMonstersOnPos(blackboard.hero.GetIndexHeroPos(), out List<TrapData> minions);
        if (minions.Count > 0)
        {
            blackboard.Targets.Clear();
            blackboard.Targets.AddRange(minions);
            return NodeState.Success;
        }
        return NodeState.Failure;
    }
}
