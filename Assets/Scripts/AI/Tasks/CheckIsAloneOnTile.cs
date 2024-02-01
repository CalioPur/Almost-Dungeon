using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class CheckIsAloneOnTile : Node
{
    private MinionBlackboard minionBlackboard;
    
    public CheckIsAloneOnTile(MinionBlackboard minionBlackboard)
    {
        this.minionBlackboard = minionBlackboard;
    }
    
    public override NodeState Evaluate(Node root)
    {
        Vector2Int position = new Vector2Int(minionBlackboard.minionData.indexX, minionBlackboard.minionData.indexY);
        Vector2Int heroPos = GameManager.Instance.GetHeroPos();
        if (minionBlackboard.minionData.mapManager.GetMonstersOnPos(position, out List<TrapData> traps))
        {
            if (traps.Count > 0)
            {
                return NodeState.Failure;
            }
        }
        
        if (heroPos.x == position.x && heroPos.y == position.y)
        {
            return NodeState.Failure;
        }
        
        return NodeState.Success;
    }
}
