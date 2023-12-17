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

        if (blackboard.heroPosition.y == y && blackboard.heroPosition.x == x
            ||
            ( y + 1 <= blackboard.minionData.mapManager.height - 2 &&
                 blackboard.heroPosition.y == y + 1 && blackboard.heroPosition.x == x &&
                 blackboard.minionData.mapManager.GetTileDataAtPosition(x, y).hasDoorUp)
            ||
            (y - 1 >= 0 && blackboard.heroPosition.y == y - 1 && blackboard.heroPosition.x == x &&
             blackboard.minionData.mapManager.GetTileDataAtPosition(x, y).hasDoorDown)
            ||
            (x + 1 <= blackboard.minionData.mapManager.width - 2 && 
             blackboard.heroPosition.y == y && blackboard.heroPosition.x == x + 1 &&
             blackboard.minionData.mapManager.GetTileDataAtPosition(x, y).hasDoorRight)
            ||
            (x - 1 >= 0 && blackboard.heroPosition.y == y && blackboard.heroPosition.x == x - 1 &&
             blackboard.minionData.mapManager.GetTileDataAtPosition(x, y).hasDoorLeft)
            )
        {
            blackboard.dir = DirectionToMove.None;
            if (blackboard.firstTimeSeeHero)
            {
                blackboard.firstTimeSeeHero = false;
                blackboard.minionData.PlayEmote(EmoteType.Detected);
            }
            return NodeState.Success;
        }

        if (blackboard.heroPosition.x == blackboard.minionData.indexX)
            if (blackboard.heroPosition.y > blackboard.minionData.indexY)
                while (y + 1 < blackboard.heroPosition.y &&
                       blackboard.minionData.mapManager.GetTileDataAtPosition(x, y).hasDoorUp &&
                       blackboard.minionData.mapManager.GetTileDataAtPosition(x, y).PiecePlaced)
                {
                    y++;
                    if (blackboard.heroPosition.y == y + 1 )
                    {
                        if (blackboard.firstTimeSeeHero)
                        {
                            blackboard.firstTimeSeeHero = false;
                            blackboard.minionData.PlayEmote(EmoteType.Detected);
                        }
                        blackboard.dir = DirectionToMove.Up;
                        return NodeState.Success;
                    }
                }
            else
                while (y - 1 >= blackboard.heroPosition.y &&
                       blackboard.minionData.mapManager.GetTileDataAtPosition(x, y).hasDoorDown &&
                       blackboard.minionData.mapManager.GetTileDataAtPosition(x, y).PiecePlaced)
                {
                    y--;
                    if (blackboard.heroPosition.y == y - 1)
                    {
                        if (blackboard.firstTimeSeeHero)
                        {
                            blackboard.firstTimeSeeHero = false;
                            blackboard.minionData.PlayEmote(EmoteType.Detected);
                        }
                        blackboard.dir = DirectionToMove.Down;
                        return NodeState.Success;
                    }
                }

        if (blackboard.heroPosition.y == blackboard.minionData.indexY)
            if (blackboard.heroPosition.x > blackboard.minionData.indexX)
                while (x + 1  <= blackboard.heroPosition.x &&
                       blackboard.minionData.mapManager.GetTileDataAtPosition(x, y).hasDoorRight &&
                       blackboard.minionData.mapManager.GetTileDataAtPosition(x, y).PiecePlaced)
                {
                    x++;
                    if (blackboard.heroPosition.x == x + 1 )
                    {
                        if (blackboard.firstTimeSeeHero)
                        {
                            blackboard.firstTimeSeeHero = false;
                            blackboard.minionData.PlayEmote(EmoteType.Detected);
                        }
                        blackboard.dir = DirectionToMove.Right;
                        return NodeState.Success;
                    }
                }
            else
                while (x - 1 >= blackboard.heroPosition.x &&
                       blackboard.minionData.mapManager.GetTileDataAtPosition(x, y).hasDoorLeft &&
                       blackboard.minionData.mapManager.GetTileDataAtPosition(x, y).PiecePlaced)
                {
                    x--;
                    if (blackboard.heroPosition.x == x - 1)
                    {
                        if (blackboard.firstTimeSeeHero)
                        {
                            blackboard.firstTimeSeeHero = false;
                            blackboard.minionData.PlayEmote(EmoteType.Detected);
                        }
                        blackboard.dir = DirectionToMove.Left;
                        return NodeState.Success;
                    }
                }
        if (!blackboard.firstTimeSeeHero)
        {
            blackboard.firstTimeSeeHero = true;
        }

        blackboard.dir = DirectionToMove.Error;
        return NodeState.Failure;
    }
}