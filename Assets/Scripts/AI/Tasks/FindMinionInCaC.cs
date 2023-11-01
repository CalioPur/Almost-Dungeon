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
        blackboard.hero.mapManager.GetNbMonstersOnPos(new Vector2Int(blackboard.hero.indexHeroX, blackboard.hero.indexHeroY), out List<Hero> nbLil, out List<Hero> nbBig, out List<Hero> nbArcher);
        if (nbLil.Count > 0 || nbBig.Count > 0 || nbArcher.Count > 0)
        {
            blackboard.Targets.Clear();
            blackboard.Targets.AddRange(nbLil);
            blackboard.Targets.AddRange(nbBig);
            blackboard.Targets.AddRange(nbArcher);
            return NodeState.Success;
        }
        return NodeState.Failure;
    }
}
