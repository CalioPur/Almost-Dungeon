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
        
        foreach (var target in blackboard.ChosenTarget)
        {
            target.TakeDamage(blackboard.hero.info.So.AttackPoint);
            Vector3 dir = target.transform.position - blackboard.hero.transform.position;
            blackboard.hero.AddAnim(new AnimToQueue(blackboard.hero.transform, dir.normalized * 0.3f, true, delay, Ease.InBack, 2));
        }
        


        return NodeState.Success;
    }
}
