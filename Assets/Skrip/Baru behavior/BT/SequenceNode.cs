using System.Collections.Generic;

public class SequenceNode : INode
{
    private List<INode> children;

    public SequenceNode(IEnumerable<INode> nodes)
    {
        children = new List<INode>(nodes);
    }

    public NodeState Evaluate()
    {
        foreach (var child in children)
        {
            NodeState state = child.Evaluate();
            if (state == NodeState.FAILURE)
            {
                return NodeState.FAILURE;
            }
            if (state == NodeState.RUNNING)
            {
                return NodeState.RUNNING;
            }
        }
        return NodeState.SUCCESS;
    }
}
