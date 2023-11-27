using BehaviourTree;

public class HeroIsInSight : Node
{
    private MinionBlackboard blackboard;

    public HeroIsInSight(MinionBlackboard blackboard)
    {
        this.blackboard = blackboard;
    }

    public override NodeState Evaluate(Node root)
    {
        int x = blackboard.minionData.indexX;
        int y = blackboard.minionData.indexY;

        if (blackboard.heroPosition.y == y && blackboard.heroPosition.x == x)
        {
            blackboard.dir = DirectionToMove.None;
            return NodeState.Success;
        }

        if (blackboard.heroPosition.x == blackboard.minionData.indexX)
            if (blackboard.heroPosition.y > blackboard.minionData.indexY)
                while (y <= blackboard.heroPosition.y &&
                       blackboard.minionData.mapManager.GetTileDataAtPosition(x, y).hasDoorUp &&
                       blackboard.minionData.mapManager.GetTileDataAtPosition(x, y).PiecePlaced)
                {
                    y++;
                    if (blackboard.heroPosition.y == y)
                    {
                        blackboard.dir = DirectionToMove.Up;
                        return NodeState.Success;
                    }
                }
            else
                while (y >= blackboard.heroPosition.y &&
                       blackboard.minionData.mapManager.GetTileDataAtPosition(x, y).hasDoorDown &&
                       blackboard.minionData.mapManager.GetTileDataAtPosition(x, y).PiecePlaced)
                {
                    y--;
                    if (blackboard.heroPosition.y == y)
                    {
                        blackboard.dir = DirectionToMove.Down;
                        return NodeState.Success;
                    }
                }

        if (blackboard.heroPosition.y == blackboard.minionData.indexY)
            if (blackboard.heroPosition.x > blackboard.minionData.indexX)
                while (x <= blackboard.heroPosition.x &&
                       blackboard.minionData.mapManager.GetTileDataAtPosition(x, y).hasDoorRight &&
                       blackboard.minionData.mapManager.GetTileDataAtPosition(x, y).PiecePlaced)
                {
                    x++;
                    if (blackboard.heroPosition.x == x)
                    {
                        blackboard.dir = DirectionToMove.Right;
                        return NodeState.Success;
                    }
                }
            else
                while (x >= blackboard.heroPosition.x &&
                       blackboard.minionData.mapManager.GetTileDataAtPosition(x, y).hasDoorLeft &&
                       blackboard.minionData.mapManager.GetTileDataAtPosition(x, y).PiecePlaced)
                {
                    x--;
                    if (blackboard.heroPosition.x == x)
                    {
                        blackboard.dir = DirectionToMove.Left;
                        return NodeState.Success;
                    }
                }

        blackboard.dir = DirectionToMove.Error;
        return NodeState.Failure;
    }
}