using System;
using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using DG.Tweening;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;


public class AttackHero : Node
{
    MinionBlackboard blackboard;
    AttackType attackType;

    public AttackHero(MinionBlackboard blackboard, AttackType _attackType)
    {
        this.blackboard = blackboard;
        attackType = _attackType;
    }

    public override NodeState Evaluate(Node root)
    {
        Vector2Int heroPos = GameManager.Instance.GetHeroPos();
        TileData tileWhereHeroIs =  blackboard.minionData.mapManager.GetTileDataAtPosition(heroPos.x,
            heroPos.y );
        
        blackboard.minionData.addAnim(new AnimToQueue(blackboard.minionData.transform,tileWhereHeroIs.transform ,Vector3.zero, true, 0.3f,
            Ease.InBack, 2));
        blackboard.minionData.Attack(blackboard.minionData.minionInstance.So.damage, attackType);
        DirectionToMove dirTarget = FunctionUtils.GetDirectionToMoveWithTilePos(heroPos,
            new Vector2Int(blackboard.minionData.indexX, blackboard.minionData.indexY));
        blackboard.minionData.PlayAttackFX(tileWhereHeroIs.transform, 0.5f, dirTarget);
        return NodeState.Success;
    }
}