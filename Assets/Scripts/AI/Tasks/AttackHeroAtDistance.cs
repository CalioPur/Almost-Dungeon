using BehaviourTree;
using UnityEditor.Experimental.GraphView;
using Node = BehaviourTree.Node;

public class AttackHeroAtDistance : Node
{
    MinionBlackboard blackboard;
    
    public AttackHeroAtDistance(MinionBlackboard blackboard)
    {
        this.blackboard = blackboard;
    }


    public override NodeState Evaluate(Node root)
    {
        if (blackboard.dir == DirectionToMove.Error)
        {
            return NodeState.Failure;
        }
        
        //attack hero here
        
        return NodeState.Success;
    }
}
