using UnityEngine;

public class ChasePlayer : INode
{
    private ZombieBT zombie;

    public ChasePlayer(ZombieBT zombie)
    {
        this.zombie = zombie;
    }

    public NodeState Evaluate()
    {
        if (zombie.target == null) return NodeState.FAILURE;

        zombie.MoveTo(zombie.target.position, zombie.chaseSpeed);

        if (zombie.IsPlayerInRange(zombie.attackRange))
        {
            return NodeState.SUCCESS; // Beralih ke attack
        }

        return NodeState.RUNNING;
    }
}
