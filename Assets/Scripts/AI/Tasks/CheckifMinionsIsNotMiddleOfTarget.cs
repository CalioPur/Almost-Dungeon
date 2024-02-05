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
        Vector2Int heroPos = GameManager.Instance.GetHeroPos();
        switch (blackboard.dir)
        {
            case DirectionToMove.Up:
                minionPos.y++;
                while (minionPos.y < heroPos.y)
                {
                    if (blackboard.minionData.mapManager.GetNbMonstersOnPos(minionPos) > 0) return NodeState.Failure;
                    minionPos.y++;
                }
                break;
            case DirectionToMove.Down:
                minionPos.y--;
                while (minionPos.y > heroPos.y)
                {
                    if (blackboard.minionData.mapManager.GetNbMonstersOnPos(minionPos) > 0) return NodeState.Failure;
                    minionPos.y--;
                }
                break;
            case DirectionToMove.Left:
                minionPos.x--;
                while (minionPos.x > heroPos.x)
                {
                    if (blackboard.minionData.mapManager.GetNbMonstersOnPos(minionPos) > 0) return NodeState.Failure;
                    minionPos.x--;
                }
                break;
            case DirectionToMove.Right:
                minionPos.x++;
                while (minionPos.x < heroPos.x)
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