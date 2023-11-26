using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BehaviourTree;
using UnityEngine;

public class ChooseTargetToHit : Node
{
    private HeroBlackboard blackboard;
    private bool AOE = false;

    public ChooseTargetToHit(HeroBlackboard _blackboard, bool _AOE)
    {
        blackboard = _blackboard;
        AOE = _AOE;
    }

    public override NodeState Evaluate(Node root)
    {
        if (blackboard.Targets.Count <= 0) return NodeState.Failure;
        if (blackboard.ChosenTarget != null &&
            blackboard.ChosenTarget.Any(target => blackboard.Targets.Contains(target)))
            return NodeState.Success;

        blackboard.ChosenTarget = new ();

        if (AOE)
        {
            foreach (var target in blackboard.Targets.Where(target => !target.isDead))
            {
                blackboard.ChosenTarget.Add(target);
            }
            return NodeState.Success;
        }

        blackboard.ChosenTarget.Add(blackboard.Targets[Random.Range(0, blackboard.Targets.Count)]);
        return NodeState.Success;
    }
}