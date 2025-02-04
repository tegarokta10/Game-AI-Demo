public class NewBehaviorTree
{
    private INode rootNode;

    public void SetRootNode(INode root)
    {
        rootNode = root;
    }

    public void Tick()
    {
        if (rootNode != null)
        {
            rootNode.Evaluate();
        }
    }
}
