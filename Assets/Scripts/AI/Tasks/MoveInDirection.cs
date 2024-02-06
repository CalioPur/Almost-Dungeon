using System;
using BehaviourTree;
using UnityEngine;

public class MoveInDirection : Node
{
    private MinionBlackboard blackboard;

    public MoveInDirection(MinionBlackboard blackboard)
    {
        this.blackboard = blackboard;
    }

    public override NodeState Evaluate(Node root)
    {
        if (blackboard.dir is DirectionToMove.Error)
            return NodeState.Failure;
        if (blackboard.dir is DirectionToMove.None)
            return NodeState.Failure;
        Vector2Int heroPos = GameManager.Instance.GetHeroPos();
        switch (blackboard.dir)
        {
            case DirectionToMove.Left:
                if (blackboard.minionData.indexX + 1 == heroPos.x &&
                    blackboard.minionData.indexY == heroPos.y)
                    return NodeState.Success;
                break;
            case DirectionToMove.Right:
                if (blackboard.minionData.indexX - 1 == heroPos.x &&
                    blackboard.minionData.indexY == heroPos.y)
                    return NodeState.Success;
                break;
            case DirectionToMove.Up:
                if (blackboard.minionData.indexX == heroPos.x &&
                    blackboard.minionData.indexY + 1 == heroPos.y)
                    return NodeState.Success;
                break;
            case DirectionToMove.Down:
                if (blackboard.minionData.indexX == heroPos.x &&
                    blackboard.minionData.indexY - 1 == heroPos.y)
                    return NodeState.Success;
                break;
        }

        Vector2Int temporaryIndex = new Vector2Int(blackboard.minionData.indexX, blackboard.minionData.indexY);
        temporaryIndex.x += (blackboard.dir == DirectionToMove.Right) ? 1 :
            (blackboard.dir == DirectionToMove.Left) ? -1 : 0;
        temporaryIndex.y += (blackboard.dir == DirectionToMove.Up) ? 1 :
            (blackboard.dir == DirectionToMove.Down) ? -1 : 0;
        int index = -1;
        bool isValidPos = blackboard.minionData.mapManager.AddMinionOnTile(
            new Vector2Int(temporaryIndex.x, temporaryIndex.y), blackboard.minionData);
        
        if (!isValidPos) return NodeState.Failure;
        
        blackboard.minionData.mapManager.RemoveEnemyOnTile(
            new Vector2Int(blackboard.minionData.indexX, blackboard.minionData.indexY), blackboard.minionData,
            blackboard.minionData.gameObject.transform.position);
        blackboard.minionData.indexX = temporaryIndex.x;
        blackboard.minionData.indexY = temporaryIndex.y;
        TileData tileData = MapManager.Instance.GetTileDataAtPosition(temporaryIndex.x, temporaryIndex.y);
        blackboard.minionData.Move(tileData.transform, Vector3.zero, 0.3f);
        return NodeState.Success;
    }
}