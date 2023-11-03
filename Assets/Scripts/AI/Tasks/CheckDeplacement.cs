using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class CheckDeplacement : Node
{
    private MinionBlackboard blackboard;

    public CheckDeplacement(MinionBlackboard blackboard)
    {
        this.blackboard = blackboard;
        
    }


    private bool IsInSight(DirectionToMove d, MapManager map)
    {
        
        int x = blackboard.minionData.indexX;
        int y = blackboard.minionData.indexY;
        switch (d)
        {
            case DirectionToMove.Up:
                while(y<=blackboard.heroPosition.y && map.GetTileDataAtPosition(x,y).hasDoorUp && map.GetTileDataAtPosition(x,y).PiecePlaced)
                {
                    y++;
                    if(blackboard.heroPosition.y == y)
                    {
                        return true;
                    }
                }
                break;
            case DirectionToMove.Right:
                while(x<=blackboard.heroPosition.x && map.GetTileDataAtPosition(x,y).hasDoorRight && map.GetTileDataAtPosition(x,y).PiecePlaced)
                {
                    x++;
                    if(blackboard.heroPosition.x == x)
                    {
                        return true;
                    }
                }
                break;
            case DirectionToMove.Down:
                while(y>=blackboard.heroPosition.y && map.GetTileDataAtPosition(x,y).hasDoorDown && map.GetTileDataAtPosition(x,y).PiecePlaced)
                {
                    y--;
                    if(blackboard.heroPosition.y == y)
                    {
                        return true;
                    }
                }
                break;
            case DirectionToMove.Left:
                while (x >= blackboard.heroPosition.x && map.GetTileDataAtPosition(x, y).hasDoorLeft && map.GetTileDataAtPosition(x,y).PiecePlaced)
                {
                    x--;
                    if(blackboard.heroPosition.x == x)
                    {
                        return true;
                    }
                }
                break;
        }
        
        return false;
    }
    public override NodeState Evaluate(Node root)
    {

        if (blackboard.heroPosition.x == blackboard.minionData.indexX)
        {
            if (blackboard.heroPosition.y == blackboard.minionData.indexY)
            {
                blackboard.dir = DirectionToMove.None;
                return NodeState.Success;
            }
            else if (blackboard.heroPosition.y > blackboard.minionData.indexY)
            {
                if (IsInSight(DirectionToMove.Up, blackboard.minionData.mapManager))
                {
                    blackboard.dir = DirectionToMove.Up;
                    return NodeState.Success;
                }
                
            }
            else if (blackboard.heroPosition.y < blackboard.minionData.indexY)
            {
                if (IsInSight(DirectionToMove.Down, blackboard.minionData.mapManager))
                {
                    blackboard.dir = DirectionToMove.Down;
                    return NodeState.Success;
                }
            }
        }
        else if (blackboard.heroPosition.y == blackboard.minionData.indexY)
        {
            if(blackboard.heroPosition.x>blackboard.minionData.indexX)
            {
                if (IsInSight(DirectionToMove.Right, blackboard.minionData.mapManager))
                {
                    blackboard.dir = DirectionToMove.Right;
                    return NodeState.Success;
                }
            }
            else if(blackboard.heroPosition.x<blackboard.minionData.indexX)
            {
                if (IsInSight(DirectionToMove.Left, blackboard.minionData.mapManager))
                {
                    blackboard.dir = DirectionToMove.Left;
                    return NodeState.Success;
                }
            }
        }
        return NodeState.Failure;
    }
}
