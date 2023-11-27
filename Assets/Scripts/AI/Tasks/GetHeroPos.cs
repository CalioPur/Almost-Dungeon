using BehaviourTree;

public class GetHeroPos : Node
{
    
    private MinionBlackboard blackboard;
    
    
    public GetHeroPos(MinionBlackboard blackboard)
    {
        this.blackboard = blackboard;
    }
    
    public override NodeState Evaluate(Node root)
    {
        blackboard.minionData.GetHeroPos();
        return NodeState.Success;
    }

}
