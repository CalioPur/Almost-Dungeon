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

        if (blackboard.heroPosition.y == y && blackboard.heroPosition.x == x)
        {
            blackboard.dir = DirectionToMove.None;
            return NodeState.Success;
        }
        return NodeState.Failure;
    }
}
