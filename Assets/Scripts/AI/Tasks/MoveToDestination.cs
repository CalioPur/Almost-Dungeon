using BehaviourTree;
using UnityEngine;

public class MoveToDestination : Node
{
    private HeroBlackboard blackboard;
    
    public MoveToDestination(HeroBlackboard _blackboard)
    {
        blackboard = _blackboard;
    }
    
    public override NodeState Evaluate(Node root)
    {
        if (blackboard.directionToMove == DirectionToMove.None)
            return NodeState.Failure;

        switch (blackboard.directionToMove)
        {
            case DirectionToMove.Up:
                blackboard.hero.AddIndexY(1);
                break;
            case DirectionToMove.Down:
                blackboard.hero.AddIndexY(-1);
                break;
            case DirectionToMove.Left:
                blackboard.hero.AddIndexX(-1);
                break;
            case DirectionToMove.Right:
                blackboard.hero.AddIndexX(1);
                break;
        }
        Vector3 pos = Vector3.zero;
        blackboard.hero.mapManager.GetWorldPosFromTilePos(blackboard.hero.GetIndexHeroPos(), out pos);
        TileData tile;
        if (blackboard.hero.mapManager.GetTile(blackboard.hero.GetIndexHeroPos(), out tile))
        {
            tile.IsVisited = true;
        }
        
        if(isHeroNextToExit()) blackboard.hero.emotesManager.PlayEmote(EmoteType.NextToExit);

        blackboard.hero.Move(tile.transform, Vector3.zero, 0.5f);
        return NodeState.Success;
    }
    private bool isHeroNextToExit()
    {
        blackboard.hero.mapManager.GetTile(blackboard.hero.GetIndexHeroPos(), out TileData tile);
        if (tile == null)
            return false;
        if (tile.hasDoorDown)
        {
            if (blackboard.hero.GetIndexHeroPos().y - 1 < 0) return true;
            if (!blackboard.hero.mapManager
                    .mapArray[blackboard.hero.GetIndexHeroPos().x, blackboard.hero.GetIndexHeroPos().y - 1]
                    .isConnectedToPath) return true;
        }
        if (tile.hasDoorUp)
        {
            if (blackboard.hero.GetIndexHeroPos().y + 1 >= blackboard.hero.mapManager.mapArray.GetLength(1)) return true;
            if (!blackboard.hero.mapManager
                    .mapArray[blackboard.hero.GetIndexHeroPos().x, blackboard.hero.GetIndexHeroPos().y + 1]
                    .isConnectedToPath) return true;
        }
        if (tile.hasDoorLeft)
        {
            if (blackboard.hero.GetIndexHeroPos().x - 1 < 0) return true;
            if (!blackboard.hero.mapManager
                    .mapArray[blackboard.hero.GetIndexHeroPos().x - 1, blackboard.hero.GetIndexHeroPos().y]
                    .isConnectedToPath) return true;
        }

        if (!tile.hasDoorRight) return false;
        if (blackboard.hero.GetIndexHeroPos().x + 1 >= blackboard.hero.mapManager.mapArray.GetLength(0)) return true;
        return !blackboard.hero.mapManager
            .mapArray[blackboard.hero.GetIndexHeroPos().x + 1, blackboard.hero.GetIndexHeroPos().y]
            .isConnectedToPath;
    }
    
}
