using BehaviourTree;
using UnityEngine;
using Node = BehaviourTree.Node;

public class CheckifFrontOfHero : Node
{
    private MinionBlackboard blackboard;
    
    public CheckifFrontOfHero(MinionBlackboard _blackboard)
    {
        blackboard = _blackboard;
    }
    
    public override NodeState Evaluate(Node root)
    {
        int x = blackboard.minionData.indexX;
        int y = blackboard.minionData.indexY;
        Vector2Int heroPos = GameManager.Instance.GetHeroPos();

        if (heroPos.y == y && heroPos.x == x
            ||
            ( y + 1 <= blackboard.minionData.mapManager.height &&
              heroPos.y == y + 1 && heroPos.x == x &&
              blackboard.minionData.mapManager.GetTileDataAtPosition(x, y).hasDoorUp)
            ||
            (y - 1 >= 0 && heroPos.y == y - 1 && heroPos.x == x &&
             blackboard.minionData.mapManager.GetTileDataAtPosition(x, y).hasDoorDown)
            ||
            (x + 1 <= blackboard.minionData.mapManager.width && 
             heroPos.y == y && heroPos.x == x + 1 &&
             blackboard.minionData.mapManager.GetTileDataAtPosition(x, y).hasDoorRight)
            ||
            (x - 1 >= 0 && heroPos.y == y && heroPos.x == x - 1 &&
             blackboard.minionData.mapManager.GetTileDataAtPosition(x, y).hasDoorLeft)
           )
        {
            blackboard.dir = DirectionToMove.None;
            return NodeState.Success;
        }
        return NodeState.Failure;
    }
}
