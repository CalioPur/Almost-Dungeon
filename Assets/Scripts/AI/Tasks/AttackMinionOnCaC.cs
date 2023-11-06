using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using DG.Tweening;
using UnityEngine;

public class AttackMinionOnCaC : Node
{
    private HeroBlackboard blackboard;
    
    public AttackMinionOnCaC(HeroBlackboard _blackboard)
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
        blackboard.hero.AddAnim(new AnimToQueue(blackboard.hero.transform, blackboard.ChosenTarget.transform.position, 0.3f, Ease.InBack, 2));

        return NodeState.Success;
    }
}
