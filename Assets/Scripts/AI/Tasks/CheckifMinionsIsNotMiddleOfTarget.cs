using System;
using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class CheckifMinionsIsNotMiddleOfTarget : Node
{
    MinionBlackboard blackboard;

    public CheckifMinionsIsNotMiddleOfTarget(MinionBlackboard _blackboard)
    {
        blackboard = _blackboard;
    }

    public override NodeState Evaluate(Node root)
    {
        Vector2Int minionPos = new Vector2Int(blackboard.minionData.indexX, blackboard.minionData.indexY);

        switch (blackboard.dir)
        {
            case DirectionToMove.Up:
                while (minionPos.y < blackboard.heroPosition.y)
                {
                    if (blackboard.minionData.mapManager.GetNbMonstersOnPos(minionPos) > 0) return NodeState.Failure;
                    minionPos.y++;
                }
                break;
            case DirectionToMove.Down:
                while (minionPos.y > blackboard.heroPosition.y)
                {
                    if (blackboard.minionData.mapManager.GetNbMonstersOnPos(minionPos) > 0) return NodeState.Failure;
                    minionPos.y--;
                }
                break;
            case DirectionToMove.Left:
                while (minionPos.x > blackboard.heroPosition.x)
                {
                    if (blackboard.minionData.mapManager.GetNbMonstersOnPos(minionPos) > 0) return NodeState.Failure;
                    minionPos.x--;
                }
                break;
            case DirectionToMove.Right:
                while (minionPos.x < blackboard.heroPosition.x)
                {
                    if (blackboard.minionData.mapManager.GetNbMonstersOnPos(minionPos) > 0) return NodeState.Failure;
                    minionPos.x++;
                }
                break;
            case DirectionToMove.None:
                break;
            case DirectionToMove.Error:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        return NodeState.Success;
    }
}