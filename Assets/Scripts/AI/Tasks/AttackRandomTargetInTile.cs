

using System.Collections.Generic;
using BehaviourTree;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackRandomTargetInTile : Node
{
    MinionBlackboard blackboard;
    
    public AttackRandomTargetInTile(MinionBlackboard _blackboard)
    {
        blackboard = _blackboard;
    }
    
    public override NodeState Evaluate(Node root)
    {
        int random = 0;
        blackboard.minionData.mapManager.GetMonstersOnPos(blackboard.heroPosition, out List<TrapData> minions);
        random = (minions.Count > 0) ? Random.Range(0, minions.Count + 1) : 0;
        Vector3 position = blackboard.minionData.transform.position;
        
        if (random == 0)
        {
            TileData tileWhereHeroIs =  blackboard.minionData.mapManager.GetTileDataAtPosition(blackboard.heroPosition.x,
                blackboard.heroPosition.y);
            blackboard.minionData.addAnim(new AnimToQueue(blackboard.minionData.transform,tileWhereHeroIs.transform,
                Vector3.zero, true, 0.3f, Ease.InBack, 2));
            blackboard.minionData.Attack(blackboard.minionData.minionInstance.So.damage);
        }
        else
        {
            Transform target = minions[random - 1].transform;
            blackboard.minionData.addAnim(new AnimToQueue(blackboard.minionData.transform, target,
                Vector3.zero, true, 0.3f,
                Ease.InBack, 2));
            minions[random - 1].TakeDamage(blackboard.minionData.minionInstance.So.damage);
        }
        return NodeState.Success;
    }
}
