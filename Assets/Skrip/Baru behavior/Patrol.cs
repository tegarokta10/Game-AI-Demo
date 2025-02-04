using UnityEngine;

public class Patrol : INode
{
    private ZombieBT zombieBT;

    public Patrol(ZombieBT zombieBT)
    {
        this.zombieBT = zombieBT;
    }


    public NodeState Evaluate()
    {
        // Ambil waypoint saat ini
        Transform currentWaypoint = zombieBT.waypoints[zombieBT.currentWaypointIndex];

        // Bergerak menuju waypoint
        zombieBT.MoveTo(currentWaypoint.position, zombieBT.patrolSpeed);

        // Periksa jika sudah mencapai waypoint
        if (Vector3.Distance(zombieBT.transform.position, currentWaypoint.position) < 1.0f)
        {
            // Pindah ke waypoint berikutnya
            zombieBT.currentWaypointIndex = (zombieBT.currentWaypointIndex + 1) % zombieBT.waypoints.Count;
        }

        // Patrol berhasil dijalankan
        return NodeState.RUNNING;
    }
}
