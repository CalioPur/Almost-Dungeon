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
        blackboard.minionData.mapManager.RemoveEnemyOnTile(new Vector2Int(blackboard.minionData.indexX, blackboard.minionData.indexY), blackboard.minionData);
        var localPosition = blackboard.minionData.transform.localPosition;
        switch (blackboard.dir)
        {
            case DirectionToMove.Up:
                blackboard.minionData.mapManager.GetWorldPosFromTilePos(blackboard.minionData.indexX, blackboard.minionData.indexY + 1, out pos);
                blackboard.minionData.indexY++;
                tmp.z = localPosition.z%1;
                tmp.x = localPosition.x%1;
                break;
            case DirectionToMove.Down:
                blackboard.minionData.mapManager.GetWorldPosFromTilePos(blackboard.minionData.indexX, blackboard.minionData.indexY - 1, out pos);
                blackboard.minionData.indexY--;
                tmp.z = localPosition.z%1;
                tmp.x = localPosition.x%1;
                break;
            case DirectionToMove.Left:
                blackboard.minionData.mapManager.GetWorldPosFromTilePos(blackboard.minionData.indexX - 1, blackboard.minionData.indexY, out pos);
                blackboard.minionData.indexX--;
                tmp.z = localPosition.z%1;
                tmp.x = localPosition.x%1;
                break;
            case DirectionToMove.Right:
                blackboard.minionData.mapManager.GetWorldPosFromTilePos(blackboard.minionData.indexX + 1, blackboard.minionData.indexY, out pos);
                blackboard.minionData.indexX++;
                tmp.z = localPosition.z%1;
                tmp.x = localPosition.x%1;
                break;
        }

        tmp.x -= 0.5f;
        tmp.z-=0.5f;
        Debug.Log(tmp);
        pos += tmp;
        blackboard.minionData.mapManager.AddMinionOnTile(new Vector2Int(blackboard.minionData.indexX, blackboard.minionData.indexY), blackboard.minionData);
        blackboard.minionData.Move(pos);
        return NodeState.Success;
    }
}
