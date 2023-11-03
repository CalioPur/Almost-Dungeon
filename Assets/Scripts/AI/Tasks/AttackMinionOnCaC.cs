using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
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
        Debug.Log("Attack enemy : " + blackboard.ChosenTarget.name + " with " + blackboard.hero.info.So.AttackPoint + " damage");
        blackboard.ChosenTarget.TakeDamage(blackboard.hero.info.So.AttackPoint);
        return NodeState.Success;
    }
}
