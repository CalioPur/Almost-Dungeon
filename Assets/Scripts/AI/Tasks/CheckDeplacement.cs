using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
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
        int x = blackboard.minionData.indexMinionX;
        int y = blackboard.minionData.indexMinionY;
        switch (d)
        {
            case DirectionToMove.Up:
                while(y<=blackboard.heroPosition.y && map.GetTileDataAtPosition(x,y).hasDoorUp)
                {
                    y++;
                    if(blackboard.heroPosition.y == y)
                    {
                        return true;
                    }
                }
                break;
            case DirectionToMove.Right:
                while(x<=blackboard.heroPosition.x && map.GetTileDataAtPosition(x,y).hasDoorRight)
                {
                    x++;
                    if(blackboard.heroPosition.x == x)
                    {
                        return true;
                    }
                }
                break;
            case DirectionToMove.Down:
                while(y>=blackboard.heroPosition.y && map.GetTileDataAtPosition(x,y).hasDoorDown)
                {
                    y--;
                    if(blackboard.heroPosition.y == y)
                    {
                        return true;
                    }
                }
                break;
            case DirectionToMove.Left:
                while (x >= blackboard.heroPosition.x && map.GetTileDataAtPosition(x, y).hasDoorLeft)
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
        if (blackboard.heroPosition.x == blackboard.minionData.indexMinionX)
        {
            if (blackboard.heroPosition.y == blackboard.minionData.indexMinionY)
            {
                blackboard.dir = DirectionToMove.None;
                return NodeState.Success;
            }
            else if (blackboard.heroPosition.y > blackboard.minionData.indexMinionY)
            {
                if (IsInSight(DirectionToMove.Up, blackboard.minionData.mapManager))
                {
                    blackboard.dir = DirectionToMove.Up;
                    return NodeState.Success;
                }
                
            }
            else if (blackboard.heroPosition.y < blackboard.minionData.indexMinionY)
            {
                if (IsInSight(DirectionToMove.Down, blackboard.minionData.mapManager))
                {
                    blackboard.dir = DirectionToMove.Down;
                    return NodeState.Success;
                }
            }
        }
        else if (blackboard.heroPosition.y == blackboard.minionData.indexMinionX)
        {
            if(blackboard.heroPosition.x>blackboard.minionData.indexMinionX)
            {
                if (IsInSight(DirectionToMove.Right, blackboard.minionData.mapManager))
                {
                    blackboard.dir = DirectionToMove.Right;
                    return NodeState.Success;
                }
            }
            else if(blackboard.heroPosition.x<blackboard.minionData.indexMinionX)
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
