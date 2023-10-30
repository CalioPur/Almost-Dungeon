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

    public override NodeState Evaluate(Node root)
    {
        blackboard.directionToMove = DirectionToMove.None;
        
        List<DirectionToMove> possibleDirections = new List<DirectionToMove>();
        //check dans quelle direction le joueur peut aller
        
        TileData currentTile = blackboard.hero.mapManager.GetTileDataAtPosition(blackboard.hero.indexHeroX, blackboard.hero.indexHeroY);
        TileData NextData = null;
        if (currentTile == null)
        {
            blackboard.hero.OutOfMap();
            return NodeState.Failure;
        }
        
        if (currentTile.hasDoorRight && blackboard.hero.indexHeroX + 1 < blackboard.hero.mapManager.width - 2)
        {
            NextData =
                blackboard.hero.mapManager.GetTileDataAtPosition(blackboard.hero.indexHeroX + 1,
                    blackboard.hero.indexHeroY);
            if (!NextData.PiecePlaced)
            {
                blackboard.directionToMove = DirectionToMove.Right;
                return NodeState.Success;
            }
            
            if (NextData.hasDoorLeft)
            {
                possibleDirections.Add(DirectionToMove.Right);
            }
        }
        if (currentTile.hasDoorLeft && blackboard.hero.indexHeroX - 1 > 0)
        {
            NextData =
                blackboard.hero.mapManager.GetTileDataAtPosition(blackboard.hero.indexHeroX - 1,
                    blackboard.hero.indexHeroY);
            if (!NextData.PiecePlaced)
            {
                blackboard.directionToMove = DirectionToMove.Left;
                return NodeState.Success;
            }

            if (NextData.hasDoorRight)
            {
                possibleDirections.Add(DirectionToMove.Left);
            }
        }
        if (currentTile.hasDoorUp && blackboard.hero.indexHeroY + 1 < blackboard.hero.mapManager.height - 2)
        {
            NextData =
                blackboard.hero.mapManager.GetTileDataAtPosition(blackboard.hero.indexHeroX, blackboard.hero.indexHeroY + 1);
            if (!NextData.PiecePlaced)
            {
                blackboard.directionToMove = DirectionToMove.Up;
                return NodeState.Success;
            }

            if (NextData.hasDoorDown)
            {
                possibleDirections.Add(DirectionToMove.Up);
            }
        }
        if (currentTile.hasDoorDown && blackboard.hero.indexHeroY - 1 > 0)
        {
            NextData =
                blackboard.hero.mapManager.GetTileDataAtPosition(blackboard.hero.indexHeroX, blackboard.hero.indexHeroY - 1);
            if (!NextData.PiecePlaced)
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
            return NodeState.Failure;
        }
        else
        {// il prend une direction au hasard MAIS il ne peut pas prendre la direction inverse de celle qu'il a pris avant si il y a d'autres directions possibles
            if (possibleDirections.Contains(oldDirectionToMove) && possibleDirections.Count > 1)
            {
                possibleDirections.Remove(oldDirectionToMove);
            }
            
            
            blackboard.directionToMove = possibleDirections[Random.Range(0, possibleDirections.Count)];
            oldDirectionToMove = blackboard.directionToMove;
            return NodeState.Success;
        }
    }
}