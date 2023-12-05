using BehaviourTree;
using UnityEngine;

public class MoveToHer : Node
{
    private MinionBlackboard blackboard;
    
    public MoveToHer(MinionBlackboard _blackboard)
    {
        blackboard = _blackboard;
    }
    
    public override NodeState Evaluate(Node root)
    {
        blackboard.dir = PathFinding.BFSFindPath(new Vector2Int(blackboard.minionData.indexX, blackboard.minionData.indexY), MapManager.Instance.mapArray, Personnalities.MoveToHero);
        return blackboard.dir switch
        {
            DirectionToMove.Error => NodeState.Failure,
            DirectionToMove.None => NodeState.Failure,
            _ => NodeState.Success
        };
    }
}
