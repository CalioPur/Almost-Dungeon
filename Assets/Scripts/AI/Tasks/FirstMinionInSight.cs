using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class FirstMinionInSight : Node
{
    private HeroBlackboard blackboard;

    public FirstMinionInSight(HeroBlackboard _blackboard)
    {
        blackboard = _blackboard;
    }

    public override NodeState Evaluate(Node root)
    {
        Vector2Int indexes = blackboard.hero.GetIndexHeroPos();

        MapManager mapManager = blackboard.hero.mapManager;

        for (int x = indexes.x; x < mapManager.width; x++)
        {
            TileData tileData = blackboard.hero.mapManager.GetTileDataAtPosition(x, indexes.y);

            if (!tileData.isConnectedToPath) break;

            if (tileData.enemies.Count > 0)
            {
                blackboard.Targets = tileData.enemies;
                return NodeState.Success;
            }
        }
        
        for (int x = indexes.x; x >= 0; x--)
        {
            TileData tileData = blackboard.hero.mapManager.GetTileDataAtPosition(x, indexes.y);

            if (!tileData.isConnectedToPath) break;

            if (tileData.enemies.Count > 0)
            {
                blackboard.Targets = tileData.enemies;
                return NodeState.Success;
            }
        }
        
        for (int y = indexes.y; y < mapManager.height; y++)
        {
            TileData tileData = blackboard.hero.mapManager.GetTileDataAtPosition(indexes.x, y);

            if (!tileData.isConnectedToPath) break;

            if (tileData.enemies.Count > 0)
            {
                blackboard.Targets = tileData.enemies;
                return NodeState.Success;
            }
        }
        
        for (int y = indexes.y; y >= 0; y--)
        {
            TileData tileData = blackboard.hero.mapManager.GetTileDataAtPosition(indexes.x, y);

            if (!tileData.isConnectedToPath) break;

            if (tileData.enemies.Count > 0)
            {
                blackboard.Targets = tileData.enemies;
                return NodeState.Success;
            }
        }
        
        return NodeState.Failure;
    }
}