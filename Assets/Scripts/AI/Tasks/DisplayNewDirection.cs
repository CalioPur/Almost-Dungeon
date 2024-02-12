using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class DisplayNewDirection : Node
{
    private HeroBlackboard BB;
    
    public DisplayNewDirection(HeroBlackboard blackboard)
    {
        BB = blackboard;
    }

    public override NodeState Evaluate(Node root)
    {
        //erase all arrows
        BB.downArrow.SetActive(false);
        BB.leftArrow.SetActive(false);
        BB.rightArrow.SetActive(false);
        BB.upArrow.SetActive(false);
        
        TileData tile;
        BB.hero.mapManager.GetTile(BB.hero.GetIndexHeroPos(), out tile);
        
        if(tile.hasDoorDown)
            BB.downArrow.SetActive(true);
        if(tile.hasDoorLeft)
            BB.leftArrow.SetActive(true);
        if(tile.hasDoorRight)
            BB.rightArrow.SetActive(true);
        if(tile.hasDoorUp)
            BB.upArrow.SetActive(true);
        
        if (BB.directionToMove == DirectionToMove.Up)
            BB.downArrow.SetActive(false);
        else if (BB.directionToMove == DirectionToMove.Right)
            BB.leftArrow.SetActive(false);
        else if (BB.directionToMove == DirectionToMove.Down) 
            BB.upArrow.SetActive(false);
        else if (BB.directionToMove == DirectionToMove.Left)
            BB.rightArrow.SetActive(false);
        return NodeState.Success;
    }
}
