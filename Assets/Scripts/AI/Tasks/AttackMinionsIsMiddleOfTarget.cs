using System;
using BehaviourTree;
using UnityEngine;

public class AttackMinionsIsMiddleOfTarget : Node
{
    MinionBlackboard blackboard;
    AttackType attackType;

    public AttackMinionsIsMiddleOfTarget(MinionBlackboard _blackboard, AttackType _attackType)
    {
        blackboard = _blackboard;
        attackType = _attackType;
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
                    if (blackboard.minionData.mapManager.GetNbMonstersOnPos(minionPos) > 0)
                    {
                        blackboard.minionData.mapManager.GetMonstersOnPos(minionPos, out var monsters);
                        monsters[0].TakeDamage(blackboard.minionData.minionInstance.So.damage, attackType);
                        blackboard.minionData.PlayAttackFX(monsters[0].transform, 1.0f, blackboard.dir);
                        return NodeState.Success;
                    }
                    minionPos.y++;
                }
                break;
            case DirectionToMove.Down:
                minionPos.y--;
                while (minionPos.y > heroPos.y)
                {
                    if (blackboard.minionData.mapManager.GetNbMonstersOnPos(minionPos) > 0)
                    {
                        blackboard.minionData.mapManager.GetMonstersOnPos(minionPos, out var monsters);
                        monsters[0].TakeDamage(blackboard.minionData.minionInstance.So.damage, attackType);
                        blackboard.minionData.PlayAttackFX(monsters[0].transform, 1.0f, blackboard.dir);
                        return NodeState.Success;
                    }
                    minionPos.y--;
                }
                break;
            case DirectionToMove.Left:
                minionPos.x--;
                while (minionPos.x > heroPos.x)
                {
                    if (blackboard.minionData.mapManager.GetNbMonstersOnPos(minionPos) > 0)
                    {
                        blackboard.minionData.mapManager.GetMonstersOnPos(minionPos, out var monsters);
                        monsters[0].TakeDamage(blackboard.minionData.minionInstance.So.damage, attackType);
                        blackboard.minionData.PlayAttackFX(monsters[0].transform, 1.0f, blackboard.dir);
                        return NodeState.Success;
                    }
                    minionPos.x--;
                }
                break;
            case DirectionToMove.Right:
                minionPos.x++;
                while (minionPos.x < heroPos.x)
                {
                    if (blackboard.minionData.mapManager.GetNbMonstersOnPos(minionPos) > 0)
                    {
                        blackboard.minionData.mapManager.GetMonstersOnPos(minionPos, out var monsters);
                        monsters[0].TakeDamage(blackboard.minionData.minionInstance.So.damage, attackType);
                        blackboard.minionData.PlayAttackFX(monsters[0].transform, 1.0f, blackboard.dir);
                        return NodeState.Success;
                    }
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
        return NodeState.Failure;
    }
}
