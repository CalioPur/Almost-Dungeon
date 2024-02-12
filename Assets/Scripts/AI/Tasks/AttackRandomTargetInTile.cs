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
        Vector2Int heroPos = GameManager.Instance.GetHeroPos();
        blackboard.minionData.mapManager.GetMonstersOnPos(heroPos, out List<TrapData> minions);
        random = (minions.Count > 0) ? Random.Range(0, minions.Count + 1) : 0;
        Vector3 position = blackboard.minionData.transform.position;
        DirectionToMove dirTarget = DirectionToMove.None;
        if (random == 0)
        {
            TileData tileWhereHeroIs = blackboard.minionData.mapManager.GetTileDataAtPosition(heroPos.x,
                heroPos.y);
            blackboard.minionData.addAnim(new AnimToQueue(blackboard.minionData.transform, tileWhereHeroIs.transform,
                Vector3.zero, true, 0.6f, Ease.InBack, 2));
            blackboard.minionData.Attack(blackboard.minionData.minionInstance.So.damage, attackType);
            dirTarget = FunctionUtils.GetDirectionToMoveWithTilePos(heroPos,
                new Vector2Int(blackboard.minionData.indexX, blackboard.minionData.indexY));
            blackboard.minionData.PlayAttackFX(tileWhereHeroIs.transform, 1.0f, dirTarget);
        }
        else
        {
            Transform target = minions[random - 1].transform;
            blackboard.minionData.addAnim(new AnimToQueue(blackboard.minionData.transform, target,
                Vector3.zero, true, 0.6f,
                Ease.InBack, 2));
            minions[random - 1].TakeDamage(blackboard.minionData.minionInstance.So.damage, attackType);
            dirTarget = FunctionUtils.GetDirectionToMoveWithTilePos(heroPos,
                new Vector2Int(blackboard.minionData.indexX, blackboard.minionData.indexY));
            blackboard.minionData.PlayAttackFX(target, 1.0f, dirTarget);
        }

        return NodeState.Success;
    }
}