using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class GetDirectionOfNearestEnemyInSight : Node
{
    HeroBlackboard blackboard;

    public GetDirectionOfNearestEnemyInSight(HeroBlackboard _blackboard)
    {
        blackboard = _blackboard;
    }

    public override NodeState Evaluate(Node root)
    {
        Vector2Int indexes = blackboard.hero.GetIndexHeroPos();
        Vector2Int MapSize = blackboard.hero.mapManager.GetSizeDungeon();

        int minDistance = int.MaxValue;

        MapManager mapManager = blackboard.hero.mapManager;

        int distance = 0;
        for (int x = indexes.x; x < MapSize.x; x++)
        {
            TileData tileData = blackboard.hero.mapManager.GetTileDataAtPosition(x, indexes.y);
            if (!tileData.PiecePlaced) break;
            if (mapManager.GetNbMonstersOnPos(new Vector2Int(x, indexes.y)) > 0)
            {
                if (distance < minDistance)
                {
                    blackboard.directionToMove = DirectionToMove.Right;
                    minDistance = distance;
                }
                break;
            }

            if (!tileData.hasDoorRight) break;
            distance++;
        }
        
        distance = 0;
        for (int x = indexes.x; x >= 0; x--)
        {
            TileData tileData = blackboard.hero.mapManager.GetTileDataAtPosition(x, indexes.y);
            if (!tileData.PiecePlaced) break;
            if (mapManager.GetNbMonstersOnPos(new Vector2Int(x, indexes.y)) > 0)
            {
                if (distance < minDistance)
                {
                    blackboard.directionToMove = DirectionToMove.Left;
                    minDistance = distance;
                }
                break;
            }

            if (!tileData.hasDoorLeft) break;
            distance++;
        }
        
        distance = 0;
        for (int y = indexes.y; y < MapSize.y; y++)
        {
            TileData tileData = blackboard.hero.mapManager.GetTileDataAtPosition(indexes.x, y);
            if (!tileData.PiecePlaced) break;
            if (!tileData) break;
            
            if (mapManager.GetNbMonstersOnPos(new Vector2Int(indexes.x, y)) > 0)
            {
                if (distance < minDistance)
                {
                    blackboard.directionToMove = DirectionToMove.Up;
                    minDistance = distance;
                }
                break;
            }

            if (!tileData.hasDoorUp) break;
            distance++;
        }
        
        distance = 0;
        for (int y = indexes.y; y >= 0; y--)
        {
            TileData tileData = blackboard.hero.mapManager.GetTileDataAtPosition(indexes.x, y);
            if (!tileData.PiecePlaced) break;
            if (!tileData.isConnectedToPath) break;

            if (mapManager.GetNbMonstersOnPos(new Vector2Int(indexes.x, y)) > 0)
            {
                if (distance < minDistance)
                {
                    blackboard.directionToMove = DirectionToMove.Down;
                    minDistance = distance;
                }
                break;
            }

            if (!tileData.hasDoorDown) break;
            distance++;
        }
        if (minDistance == int.MaxValue)
        {
            blackboard.directionToMove = DirectionToMove.None;
            return NodeState.Failure;
        }
        return NodeState.Success;
    }
}