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
        if (blackboard.dir == DirectionToMove.None)
            return NodeState.Success;
        
        blackboard.minionData.mapManager.RemoveEnemyOnTile(
            new Vector2Int(blackboard.minionData.indexX, blackboard.minionData.indexY), blackboard.minionData);
        var localPosition = blackboard.minionData.transform.localPosition;

        blackboard.minionData.indexX += (blackboard.dir == DirectionToMove.Right) ? 1 :
            (blackboard.dir == DirectionToMove.Left) ? -1 : 0;
        blackboard.minionData.indexY += (blackboard.dir == DirectionToMove.Up) ? 1 :
            (blackboard.dir == DirectionToMove.Down) ? -1 : 0;

        blackboard.minionData.mapManager.GetWorldPosFromTilePos(
            new Vector2Int(blackboard.minionData.indexX, blackboard.minionData.indexY), out Vector3 pos);
        blackboard.minionData.mapManager.AddMinionOnTile(
            new Vector2Int(blackboard.minionData.indexX, blackboard.minionData.indexY), blackboard.minionData);
        return NodeState.Success;
    }
}