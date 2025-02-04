public enum NodeState
{
    SUCCESS,
    FAILURE,
    RUNNING
}

public interface INode
{
    NodeState Evaluate();
}
