using System.Collections.Generic;
using BehaviourTree;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackRandomTargetInTile : Node
{
    MinionBlackboard blackboard;
    AttackType attackType;

    public AttackRandomTargetInTile(MinionBlackboard _blackboard,  AttackType _attackType)
    {
        blackboard = _blackboard;
        attackType = _attackType;
    }

    public override NodeState Evaluate(Node root)
    {
        int random = 0;
        blackboard.minionData.mapManager.GetMonstersOnPos(blackboard.heroPosition, out List<TrapData> minions);
        random = (minions.Count > 0) ? Random.Range(0, minions.Count + 1) : 0;
        Vector3 position = blackboard.minionData.transform.position;
        DirectionToMove dirTarget = DirectionToMove.None;
        if (random == 0)
        {
            TileData tileWhereHeroIs = blackboard.minionData.mapManager.GetTileDataAtPosition(blackboard.heroPosition.x,
                blackboard.heroPosition.y);
            blackboard.minionData.addAnim(new AnimToQueue(blackboard.minionData.transform, tileWhereHeroIs.transform,
                Vector3.zero, true, 0.3f, Ease.InBack, 2));
            blackboard.minionData.Attack(blackboard.minionData.minionInstance.So.damage, attackType);
            dirTarget = FunctionUtils.GetDirectionToMoveWithTilePos(blackboard.heroPosition,
                new Vector2Int(blackboard.minionData.indexX, blackboard.minionData.indexY));
            blackboard.minionData.PlayAttackFX(tileWhereHeroIs.transform, 0.5f, dirTarget);
        }
        else
        {
            Transform target = minions[random - 1].transform;
            blackboard.minionData.addAnim(new AnimToQueue(blackboard.minionData.transform, target,
                Vector3.zero, true, 0.3f,
                Ease.InBack, 2));
            minions[random - 1].TakeDamage(blackboard.minionData.minionInstance.So.damage, attackType);
            dirTarget = FunctionUtils.GetDirectionToMoveWithTilePos(blackboard.heroPosition,
                new Vector2Int(blackboard.minionData.indexX, blackboard.minionData.indexY));
            blackboard.minionData.PlayAttackFX(target, 0.5f, dirTarget);
        }

        return NodeState.Success;
    }
}