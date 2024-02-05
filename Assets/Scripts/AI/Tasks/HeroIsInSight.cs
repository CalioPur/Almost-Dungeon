using BehaviourTree;
using UnityEngine;

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
            if (blackboard.firstTimeSeeHero)
            {
                blackboard.firstTimeSeeHero = false;
                blackboard.minionData.PlayEmote(EmoteType.Detected);
            }
            return NodeState.Success;
        }

        if (heroPos.x == blackboard.minionData.indexX)
            if (heroPos.y > blackboard.minionData.indexY)
                while (y < heroPos.y &&
                       blackboard.minionData.mapManager.GetTileDataAtPosition(x, y).hasDoorUp &&
                       blackboard.minionData.mapManager.GetTileDataAtPosition(x, y).PiecePlaced)
                {
                    y++;
                    if (heroPos.y == y)
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
                while (y >= heroPos.y &&
                       blackboard.minionData.mapManager.GetTileDataAtPosition(x, y).hasDoorDown &&
                       blackboard.minionData.mapManager.GetTileDataAtPosition(x, y).PiecePlaced)
                {
                    y--;
                    if (heroPos.y == y)
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

        if (heroPos.y == blackboard.minionData.indexY)
            if (heroPos.x > blackboard.minionData.indexX)
                while (x  <= heroPos.x &&
                       blackboard.minionData.mapManager.GetTileDataAtPosition(x, y).hasDoorRight &&
                       blackboard.minionData.mapManager.GetTileDataAtPosition(x, y).PiecePlaced)
                {
                    x++;
                    if (heroPos.x == x)
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
                while (x >= heroPos.x &&
                       blackboard.minionData.mapManager.GetTileDataAtPosition(x, y).hasDoorLeft &&
                       blackboard.minionData.mapManager.GetTileDataAtPosition(x, y).PiecePlaced)
                {
                    x--;
                    if (heroPos.x == x)
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