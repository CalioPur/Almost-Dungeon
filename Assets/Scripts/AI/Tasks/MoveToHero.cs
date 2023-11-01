using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
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
        blackboard.minionData.mapManager.RemoveMinionOnTile(new Vector2Int(blackboard.minionData.indexMinionX, blackboard.minionData.indexMinionY), blackboard.minionData);
        switch (blackboard.dir)
        {
            case DirectionToMove.Up:
                blackboard.minionData.mapManager.GetWorldPosFromTilePos(blackboard.minionData.indexMinionX, blackboard.minionData.indexMinionY + 1, out pos);
                blackboard.minionData.indexMinionY++;
                break;
            case DirectionToMove.Down:
                blackboard.minionData.mapManager.GetWorldPosFromTilePos(blackboard.minionData.indexMinionX, blackboard.minionData.indexMinionY - 1, out pos);
                blackboard.minionData.indexMinionY--;
                break;
            case DirectionToMove.Left:
                blackboard.minionData.mapManager.GetWorldPosFromTilePos(blackboard.minionData.indexMinionX - 1, blackboard.minionData.indexMinionY, out pos);
                blackboard.minionData.indexMinionX--;
                break;
            case DirectionToMove.Right:
                blackboard.minionData.mapManager.GetWorldPosFromTilePos(blackboard.minionData.indexMinionX + 1, blackboard.minionData.indexMinionY, out pos);
                blackboard.minionData.indexMinionX++;
                break;
        }
        blackboard.minionData.mapManager.AddMinionOnTile(new Vector2Int(blackboard.minionData.indexMinionX, blackboard.minionData.indexMinionY), blackboard.minionData);
        blackboard.minionData.Move(pos);
        return NodeState.Success;
    }
}
