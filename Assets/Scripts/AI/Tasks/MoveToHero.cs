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


        Vector3 pos = Vector3.zero;
        Vector3 tmp = Vector3.zero;
        blackboard.minionData.mapManager.RemoveEnemyOnTile(
            new Vector2Int(blackboard.minionData.indexX, blackboard.minionData.indexY), blackboard.minionData);
        var localPosition = blackboard.minionData.transform.localPosition;

        blackboard.minionData.indexX += (blackboard.dir == DirectionToMove.Right) ? 1 :
            (blackboard.dir == DirectionToMove.Left) ? -1 : 0;
        blackboard.minionData.indexY += (blackboard.dir == DirectionToMove.Up) ? 1 :
            (blackboard.dir == DirectionToMove.Down) ? -1 : 0;

        blackboard.minionData.mapManager.GetWorldPosFromTilePos(blackboard.minionData.indexX,
            blackboard.minionData.indexY, out pos);
        //tmp.z = localPosition.z % 1;
        //tmp.x = localPosition.x % 1;

        pos += tmp;
        blackboard.minionData.mapManager.AddMinionOnTile(
            new Vector2Int(blackboard.minionData.indexX, blackboard.minionData.indexY), blackboard.minionData);
        //blackboard.minionData.Move(pos,0.5f);
        return NodeState.Success;
    }
}