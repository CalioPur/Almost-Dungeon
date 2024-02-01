using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class CheckIsAloneOnTile : Node
{
    MinionBlackboard minionBlackboard;
    
    public CheckIsAloneOnTile(MinionBlackboard minionBlackboard)
    {
        this.minionBlackboard = minionBlackboard;
    }
    
    public override NodeState Evaluate(Node root)
    {
        Vector2Int position = new Vector2Int(minionBlackboard.minionData.indexX, minionBlackboard.minionData.indexY);
        if (minionBlackboard.minionData.mapManager.GetMonstersOnPos(position, out List<TrapData> traps))
        {
            if (traps.Count > 0)
            {
                return NodeState.Failure;
            }
        }
        
        if (minionBlackboard.heroPosition.x == position.x && minionBlackboard.heroPosition.y == position.y)
        {
            return NodeState.Failure;
        }
        
        return NodeState.Success;
    }
}
