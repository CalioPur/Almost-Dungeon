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
        blackboard.hero.mapManager.GetNbMonstersOnPos(new Vector2Int(blackboard.hero.indexHeroX, blackboard.hero.indexHeroY), out List<MinionData> minions);
        if (minions.Count > 0)
        {
            blackboard.Targets.Clear();
            blackboard.Targets.AddRange(minions);
            return NodeState.Success;
        }
        return NodeState.Failure;
    }
}
