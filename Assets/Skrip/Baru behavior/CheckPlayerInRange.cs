public class CheckPlayerInRange : INode
{
    private ZombieBT zombieBT;
    private float detectionRange;

    public CheckPlayerInRange(ZombieBT zombieBT, float detectionRange)
    {
        this.zombieBT = zombieBT;
        this.detectionRange = detectionRange;
    }

    public NodeState Evaluate()
    {
        // Periksa apakah target ada (tidak null)
        if (zombieBT.target == null)
        {
            return NodeState.FAILURE; // Jika tidak ada target, kembalikan Failure
        }

        // Periksa apakah target ada dalam jangkauan
        if (zombieBT.IsPlayerInRange(detectionRange))
        {
            return NodeState.SUCCESS; // Jika target berada dalam jangkauan, kembalikan Success
        }

        return NodeState.FAILURE; // Jika target tidak dalam jangkauan, kembalikan Failure
    }
}
