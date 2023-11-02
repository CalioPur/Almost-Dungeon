using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;
using UnityEngine.EventSystems;

public class CheckDirectionToMove : Node
{
    private HeroBlackboard blackboard;
    
    private DirectionToMove oldDirectionToMove = DirectionToMove.None;

    public CheckDirectionToMove(HeroBlackboard _blackboard)
    {
        blackboard = _blackboard;
    }
    
    private DirectionToMove ReverseDirection(DirectionToMove direction)
    {
        switch (direction)
        {
            case DirectionToMove.Up:
                return DirectionToMove.Down;
            case DirectionToMove.Down:
                return DirectionToMove.Up;
            case DirectionToMove.Left:
                return DirectionToMove.Right;
            case DirectionToMove.Right:
                return DirectionToMove.Left;
            default:
                return DirectionToMove.None;
        }
    }

    public override NodeState Evaluate(Node root)
    {
        blackboard.directionToMove = DirectionToMove.None;
        
        List<DirectionToMove> possibleDirections = new List<DirectionToMove>();
        TileData currentTile = blackboard.hero.mapManager.GetTileDataAtPosition(blackboard.hero.indexHeroX, blackboard.hero.indexHeroY);
        TileData NextData = null;
        
        if (currentTile == null)
        {
            blackboard.hero.OutOfMap();
            return NodeState.Success;
        }
        
        if (currentTile.hasDoorRight)
        {
            NextData =
                blackboard.hero.mapManager.GetTileDataAtPosition(blackboard.hero.indexHeroX + 1,
                    blackboard.hero.indexHeroY);
            if (NextData == null || !NextData.PiecePlaced)
            {
                blackboard.directionToMove = DirectionToMove.Right;
                return NodeState.Success;
            }
            
            if (NextData.hasDoorLeft)
            {
                possibleDirections.Add(DirectionToMove.Right);
            }
        }
        if (currentTile.hasDoorLeft)
        {
            NextData =
                blackboard.hero.mapManager.GetTileDataAtPosition(blackboard.hero.indexHeroX - 1,
                    blackboard.hero.indexHeroY);
            if (NextData == null || !NextData.PiecePlaced)
            {
                blackboard.directionToMove = DirectionToMove.Left;
                return NodeState.Success;
            }

            if (NextData.hasDoorRight)
            {
                possibleDirections.Add(DirectionToMove.Left);
            }
        }
        if (currentTile.hasDoorUp)
        {
            NextData =
                blackboard.hero.mapManager.GetTileDataAtPosition(blackboard.hero.indexHeroX, blackboard.hero.indexHeroY + 1);
            if (NextData == null || !NextData.PiecePlaced)
            {
                blackboard.directionToMove = DirectionToMove.Up;
                return NodeState.Success;
            }

            if (NextData.hasDoorDown)
            {
                possibleDirections.Add(DirectionToMove.Up);
            }
        }
        if (currentTile.hasDoorDown)
        {
            NextData =
                blackboard.hero.mapManager.GetTileDataAtPosition(blackboard.hero.indexHeroX, blackboard.hero.indexHeroY - 1);
            if (NextData == null || !NextData.PiecePlaced)
            {
                blackboard.directionToMove = DirectionToMove.Down;
                return NodeState.Success;
            }

            if (NextData.hasDoorUp)
            {
                possibleDirections.Add(DirectionToMove.Down);
            }
        }
        
        if (possibleDirections.Count == 0)
        {
            blackboard.hero.OutOfMap();
            return NodeState.Success;
        }
        else
        {// il prend une direction au hasard MAIS il ne peut pas prendre la direction inverse de celle qu'il a pris avant si il y a d'autres directions possibles
            if (possibleDirections.Contains(ReverseDirection(oldDirectionToMove)) && possibleDirections.Count > 1)
            {
                possibleDirections.Remove(ReverseDirection(oldDirectionToMove));
            }
            
            blackboard.directionToMove = possibleDirections[Random.Range(0, possibleDirections.Count)];
            oldDirectionToMove = blackboard.directionToMove;
            return NodeState.Success;
        }
    }
}