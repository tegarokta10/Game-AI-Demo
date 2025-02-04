using System.Collections.Generic;

public class SelectorNode : INode
{
    private List<INode> children;

    public SelectorNode(IEnumerable<INode> nodes)
    {
        children = new List<INode>(nodes);
    }

    public NodeState Evaluate()
    {
        foreach (var child in children)
        {
            NodeState state = child.Evaluate();
            if (state == NodeState.SUCCESS || state == NodeState.RUNNING)
            {
                return state;
            }
        }
        return NodeState.FAILURE;
    }
}

