using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BehaviourTree;
using DG.Tweening;
using UnityEngine;

public class AttackMinion : Node
{
    private HeroBlackboard blackboard;
    bool withAnim;

    public AttackMinion(HeroBlackboard _blackboard, bool _withAnim = true)
    {
        blackboard = _blackboard;
        withAnim = _withAnim;
    }

    public override NodeState Evaluate(Node root)
    {
        // if (isHeroNextToExit()) blackboard.hero.emotesManager.PlayEmote(EmoteType.NextToExit);

        if (blackboard.ChosenTarget == null) return NodeState.Failure;
        foreach (var target in blackboard.ChosenTarget.Where(target => target.isDead))
        {
            blackboard.ChosenTarget.Remove(target);
            return NodeState.Failure;
        }

        if (blackboard.ChosenTarget.Count <= 0) return NodeState.Failure;

        float delay = 0.3f / blackboard.ChosenTarget.Count;

        DirectionToMove directionWithTarget = DirectionToMove.None;
        if (blackboard.ChosenTarget.Count > 0)
        {
            Vector2Int targetPos = new Vector2Int(blackboard.ChosenTarget[0].indexX, blackboard.ChosenTarget[0].indexY);
            Vector2Int heroPos = blackboard.hero.GetIndexHeroPos();
            if (targetPos.x > heroPos.x)
            {
                directionWithTarget = DirectionToMove.Right;
            }
            else if (targetPos.x < heroPos.x)
            {
                directionWithTarget = DirectionToMove.Left;
            }
            else if (targetPos.y > heroPos.y)
            {
                directionWithTarget = DirectionToMove.Up;
            }
            else if (targetPos.y < heroPos.y)
            {
                directionWithTarget = DirectionToMove.Down;
            }

            blackboard.hero.PlayAttackClip();
            if (blackboard.Targets[0] != null)
                blackboard.hero.PlayAttackFX(blackboard.Targets[0].transform, delay, directionWithTarget);
        }


        foreach (var target in blackboard.ChosenTarget)
        {
            target.TakeDamage(blackboard.hero.info.So.AttackPoint, blackboard.hero.attackType);
            if (withAnim)
                blackboard.hero.AddAnim(new AnimToQueue(blackboard.hero.transform, target.transform, Vector3.zero, true,
                    delay, Ease.InBack, 2));
        }

        return NodeState.Success;
    }

    private bool isHeroNextToExit()
    {
        blackboard.hero.mapManager.GetTile(blackboard.hero.GetIndexHeroPos(), out TileData tile);
        if (tile == null)
            return false;
        if (tile.hasDoorDown)
        {
            if (blackboard.hero.GetIndexHeroPos().y - 1 < 0) return true;
            if (!blackboard.hero.mapManager
                    .mapArray[blackboard.hero.GetIndexHeroPos().x, blackboard.hero.GetIndexHeroPos().y - 1]
                    .isConnectedToPath) return true;
        }

        if (tile.hasDoorUp)
        {
            if (blackboard.hero.GetIndexHeroPos().y + 1 >= blackboard.hero.mapManager.mapArray.GetLength(1))
                return true;
            if (!blackboard.hero.mapManager
                    .mapArray[blackboard.hero.GetIndexHeroPos().x, blackboard.hero.GetIndexHeroPos().y + 1]
                    .isConnectedToPath) return true;
        }

        if (tile.hasDoorLeft)
        {
            if (blackboard.hero.GetIndexHeroPos().x - 1 < 0) return true;
            if (!blackboard.hero.mapManager
                    .mapArray[blackboard.hero.GetIndexHeroPos().x - 1, blackboard.hero.GetIndexHeroPos().y]
                    .isConnectedToPath) return true;
        }

        if (!tile.hasDoorRight) return false;
        if (blackboard.hero.GetIndexHeroPos().x + 1 >= blackboard.hero.mapManager.mapArray.GetLength(0)) return true;
        return !blackboard.hero.mapManager
            .mapArray[blackboard.hero.GetIndexHeroPos().x + 1, blackboard.hero.GetIndexHeroPos().y]
            .isConnectedToPath;
    }
}