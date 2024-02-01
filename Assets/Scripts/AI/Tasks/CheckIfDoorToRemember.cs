using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class CheckIfDoorToRemember : Node
{
    private HeroBlackboard _blackboard;
    
    public CheckIfDoorToRemember(HeroBlackboard blackboard)
    {
        _blackboard = blackboard;
    }
    
    public override NodeState Evaluate(Node root)
    {
        
        Vector2Int heroPos = _blackboard.hero.GetIndexHeroPos();
        
        TileData tileData = _blackboard.hero.mapManager.GetTileDataAtPosition(heroPos.x, heroPos.y);
        
        List<DoorLockedData> doors = tileData._instance.doorLocked;
        
        if (doors.Count > 0 && !_blackboard.DoorSaw.Contains(heroPos))
        {
            _blackboard.DoorSaw.Add(heroPos);
        }
        // Debug.Log("nb door saw: " + _blackboard.DoorSaw.Count);
        
        return NodeState.Success;
    }
}
