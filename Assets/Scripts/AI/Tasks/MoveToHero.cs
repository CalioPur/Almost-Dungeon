using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class MoveToHero : Node
{
    private MinionBlackboard blackboard;

    public MoveToHero(MinionBlackboard blackboard)
    {
        this.blackboard = blackboard;
    }

    public override NodeState Evaluate(Node root)
    {
        if (blackboard.dir is DirectionToMove.Error)
            return NodeState.Failure;
        if (blackboard.dir is DirectionToMove.None)
            return NodeState.Success;

        Vector2Int temporaryIndex = new Vector2Int(blackboard.minionData.indexX, blackboard.minionData.indexY);
        temporaryIndex.x += (blackboard.dir == DirectionToMove.Right) ? 1 :
            (blackboard.dir == DirectionToMove.Left) ? -1 : 0;
        temporaryIndex.y += (blackboard.dir == DirectionToMove.Up) ? 1 :
            (blackboard.dir == DirectionToMove.Down) ? -1 : 0;
        int index = -1;
        bool isValidPos = blackboard.minionData.mapManager.AddMinionOnTile(
            new Vector2Int(temporaryIndex.x, temporaryIndex.y), blackboard.minionData, out index);
        if (isValidPos)
        {
            blackboard.minionData.mapManager.RemoveEnemyOnTile(
                new Vector2Int(blackboard.minionData.indexX, blackboard.minionData.indexY), blackboard.minionData,
                blackboard.minionData.gameObject.transform.position);
            blackboard.minionData.indexX = temporaryIndex.x;
            blackboard.minionData.indexY = temporaryIndex.y;
            blackboard.minionData.indexPos = index;

            return NodeState.Success;
        }

        return NodeState.Failure;
    }
}