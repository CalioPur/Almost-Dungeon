using BehaviourTree;
using Node = BehaviourTree.Node;

public class CheckifFrontOfHero : Node
{
    MinionBlackboard blackboard;
    
    public CheckifFrontOfHero(MinionBlackboard _blackboard)
    {
        blackboard = _blackboard;
    }
    
    public override NodeState Evaluate(Node root)
    {
        int x = blackboard.minionData.indexX;
        int y = blackboard.minionData.indexY;

        if (blackboard.heroPosition.y == y && blackboard.heroPosition.x == x
            ||
            ( y + 1 <= blackboard.minionData.mapManager.height &&
              blackboard.heroPosition.y == y + 1 && blackboard.heroPosition.x == x &&
              blackboard.minionData.mapManager.GetTileDataAtPosition(x, y).hasDoorUp)
            ||
            (y - 1 >= 0 && blackboard.heroPosition.y == y - 1 && blackboard.heroPosition.x == x &&
             blackboard.minionData.mapManager.GetTileDataAtPosition(x, y).hasDoorDown)
            ||
            (x + 1 <= blackboard.minionData.mapManager.width && 
             blackboard.heroPosition.y == y && blackboard.heroPosition.x == x + 1 &&
             blackboard.minionData.mapManager.GetTileDataAtPosition(x, y).hasDoorRight)
            ||
            (x - 1 >= 0 && blackboard.heroPosition.y == y && blackboard.heroPosition.x == x - 1 &&
             blackboard.minionData.mapManager.GetTileDataAtPosition(x, y).hasDoorLeft)
           )
        {
            blackboard.dir = DirectionToMove.None;
            return NodeState.Success;
        }
        return NodeState.Failure;
    }
}
