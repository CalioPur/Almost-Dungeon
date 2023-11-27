

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
            blackboard.minionData.mapManager.GetWorldPosFromTilePos(
                new Vector2Int(blackboard.heroPosition.x + 1, blackboard.heroPosition.y + 1), out Vector3 heroPosVec3);
            Vector3 dir = heroPosVec3 - blackboard.minionData.transform.position;

            blackboard.minionData.addAnim(new AnimToQueue(blackboard.minionData.transform,
                dir.normalized * 0.3f, true, 0.3f,
                Ease.InBack, 2));
            blackboard.minionData.Attack(blackboard.minionData.minionInstance.So.damage);
        }
        else
        {
            Vector3 dir = minions[random - 1].transform.position - position;
            blackboard.minionData.addAnim(new AnimToQueue(blackboard.minionData.transform,
                dir.normalized * 0.3f, true, 0.3f,
                Ease.InBack, 2));
            minions[random - 1].TakeDamage(blackboard.minionData.minionInstance.So.damage);
        }
        return NodeState.Success;
    }
}
