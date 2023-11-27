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
        Vector2Int MapSize = blackboard.hero.mapManager.GetSizeDungeon();

        MapManager mapManager = blackboard.hero.mapManager;

        for (int x = indexes.x; x < MapSize.x; x++)
        {
            TileData tileData = blackboard.hero.mapManager.GetTileDataAtPosition(x, indexes.y);
            if (!tileData.PiecePlaced) break;
            if (mapManager.GetNbMonstersOnPos(new Vector2Int(x, indexes.y)) > 0)
            {
                blackboard.Targets = tileData.enemies;
                return NodeState.Success;
            }
            if (!tileData.hasDoorRight) break;
            
        }
        
        indexes = blackboard.hero.GetIndexHeroPos();
        
        for (int x = indexes.x; x >= 0; x--)
        {
            TileData tileData = blackboard.hero.mapManager.GetTileDataAtPosition(x, indexes.y);
            if (!tileData.PiecePlaced) break;
            if (mapManager.GetNbMonstersOnPos(new Vector2Int(x, indexes.y)) > 0)
            {
                blackboard.Targets = tileData.enemies;
                return NodeState.Success;
            }
            if (!tileData.hasDoorLeft) break;
        }
        
        indexes = blackboard.hero.GetIndexHeroPos();
        
        for (int y = indexes.y; y < MapSize.y; y++)
        {
            TileData tileData = blackboard.hero.mapManager.GetTileDataAtPosition(indexes.x, y);
            if (!tileData.PiecePlaced) break;
            if (!tileData) break;
            
            if (mapManager.GetNbMonstersOnPos(new Vector2Int(indexes.x, y)) > 0)
            {
                blackboard.Targets = tileData.enemies;
                return NodeState.Success;
            }
            if (!tileData.hasDoorUp) break;
        }
        
        indexes = blackboard.hero.GetIndexHeroPos();
        
        for (int y = indexes.y; y >= 0; y--)
        {
            TileData tileData = blackboard.hero.mapManager.GetTileDataAtPosition(indexes.x, y);
            if (!tileData.PiecePlaced) break;
            if (!tileData.isConnectedToPath) break;

            if (mapManager.GetNbMonstersOnPos(new Vector2Int(indexes.x, y)) > 0)
            {
                blackboard.Targets = tileData.enemies;
                return NodeState.Success;
            }
            if (!tileData.hasDoorDown) break;
        }
        
        return NodeState.Failure;
    }
}