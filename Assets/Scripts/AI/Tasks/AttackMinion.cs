using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BehaviourTree;
using DG.Tweening;
using UnityEngine;

public class AttackMinion : Node
{
    private HeroBlackboard blackboard;

    public AttackMinion(HeroBlackboard _blackboard)
    {
        blackboard = _blackboard;
    }

    public override NodeState Evaluate(Node root)
    {
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
            blackboard.hero.AddAnim(new AnimToQueue(blackboard.hero.transform, target.transform, Vector3.zero, true,
                delay, Ease.InBack, 2));
        }

        return NodeState.Success;
    }
}