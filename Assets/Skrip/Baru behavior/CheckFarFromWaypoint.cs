using UnityEngine;

public class CheckFarFromWaypoint : INode
{
    private ZombieBT zombieBT;
    private float distanceThreshold;

    // Perbarui konstruktor untuk menerima dua parameter
    public CheckFarFromWaypoint(ZombieBT zombieBT, float distanceThreshold)
    {
        this.zombieBT = zombieBT;
        this.distanceThreshold = distanceThreshold;
    }

    public NodeState Evaluate()
    {
        if (zombieBT.waypoints == null || zombieBT.waypoints.Count == 0)
        {
            Debug.LogWarning("Waypoints list is empty or null!");
            return NodeState.FAILURE;
        }

        if (zombieBT.currentWaypointIndex < 0 || zombieBT.currentWaypointIndex >= zombieBT.waypoints.Count)
        {
            Debug.LogWarning($"Invalid waypoint index: {zombieBT.currentWaypointIndex}");
            return NodeState.FAILURE;
        }

        Transform currentWaypoint = zombieBT.waypoints[zombieBT.currentWaypointIndex];
        float distanceToWaypoint = Vector3.Distance(zombieBT.transform.position, currentWaypoint.position);

        if (distanceToWaypoint > distanceThreshold)
        {
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }

}
