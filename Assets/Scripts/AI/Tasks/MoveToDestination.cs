using System.Collections.Generic;
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
        TileData tile;
        if (blackboard.hero.mapManager.GetTile(blackboard.hero.GetIndexHeroPos(), out tile))
        {
            tile.IsVisited = true;
        }
        
        blackboard.hero.Move(tile.transform, Vector3.zero, 0.5f);
        List<TileData> aaa = ALaid1.GetTilesInLineOfSight(blackboard.hero.GetIndexHeroPos(), blackboard.hero.mapManager.getMapArray());
        
        return NodeState.Success;
    }
}
