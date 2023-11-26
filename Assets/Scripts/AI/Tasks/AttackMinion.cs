using System.Collections;
using System.Collections.Generic;
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
        if (blackboard.ChosenTarget.isDead)
        {
            blackboard.ChosenTarget = null;
            return NodeState.Failure;
        }
        blackboard.ChosenTarget.TakeDamage(blackboard.hero.info.So.AttackPoint);
        Vector3 dir = blackboard.ChosenTarget.transform.position - blackboard.hero.transform.position;
        blackboard.hero.AddAnim(new AnimToQueue(blackboard.hero.transform, dir.normalized, 0.3f, Ease.InBack, 2));

        return NodeState.Success;
    }
}
