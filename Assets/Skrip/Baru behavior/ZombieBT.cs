using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieBT : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();
    public Transform target;
    public float patrolSpeed = 3.0f;
    public float chaseSpeed = 6.0f;
    public float detectionRange = 10.0f;
    public float attackRange = 1.5f;
    public float attackDamage = 2.0f;
    public float returnToPatrolDistance = 15.0f;
    public float obstacleAvoidanceDistance = 5.0f; // Jarak untuk menghindari rintangan
    public string obstacleTag = "Obstacle"; // Tag untuk rintangan
    public int numberOfRays = 17;  // Jumlah raycast yang ditembakkan di setiap arah
    public float angle = 90.0f; // Sudut distribusi ray di sekitar NPC

    public int currentWaypointIndex = 0;
    private NewBehaviorTree behaviorTree;
    private Vector3 currentDirection;

    void Start()
    {
        if (waypoints == null || waypoints.Count == 0)
        {
            Debug.LogError("Waypoints list is empty! Zombie cannot patrol.");
            return;
        }

        currentWaypointIndex = Mathf.Clamp(currentWaypointIndex, 0, waypoints.Count - 1);
        behaviorTree = new NewBehaviorTree();

        var root = new SelectorNode(new INode[] {
            new SequenceNode(new INode[] {
                new CheckPlayerInRange(this, detectionRange),
                new ChasePlayer(this)
            }),
            new SequenceNode(new INode[] {
                new CheckFarFromWaypoint(this, returnToPatrolDistance),
                new Patrol(this)
            }),
            new Patrol(this)
        });

        behaviorTree.SetRootNode(root);
        currentDirection = transform.forward; // Inisialisasi arah zombie.
    }

    void Update()
    {
        if (behaviorTree != null)
        {
            behaviorTree.Tick();
        }

        if (target != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, target.position);

            // Jika player dalam jarak serangan, serang player
            if (distanceToPlayer <= attackRange)
            {
                AttackPlayer();
            }
        }

        // Hapus pemanggilan ShowRaycastInAllDirections di Update
        // Cukup tampilkan ray di editor menggunakan Gizmos
    }

    // Fungsi untuk menggambar raycast di semua arah, di editor Gizmos
    void OnDrawGizmos()
    {
        // Loop untuk menggambar ray yang ditembakkan dari NPC
        for (int i = 0; i < numberOfRays; i++)
        {
            // Ambil rotasi objek saat ini (NPC)
            var rotation = this.transform.rotation;

            // Modifikasi rotasi untuk ray ke-i, menggunakan sudut yang berbeda-beda
            var rotationMod = Quaternion.AngleAxis((i / (float)numberOfRays) * angle - (angle / 2), this.transform.up);

            // Hitung arah ray berdasarkan rotasi objek dan modifikasi rotasi
            var direction = rotation * rotationMod * Vector3.forward;

            // Gambar ray di editor Unity untuk memberikan visualisasi
            Gizmos.color = Color.red;
            Gizmos.DrawRay(this.transform.position, direction * obstacleAvoidanceDistance);  // Hanya untuk editor
        }
    }

    public bool IsPlayerInRange(float range)
    {
        if (target == null) return false;
        return Vector3.Distance(transform.position, target.position) <= range;
    }


    public void MoveTo(Vector3 destination, float speed)
    {
        if (destination == Vector3.zero) return;

        // Periksa rintangan di depan zombie
        if (IsObstacleAround())
        {
            // Jika ada rintangan, cari arah penghindaran
            Vector3 avoidanceDirection = GetAvoidanceDirection();
            transform.position += avoidanceDirection * speed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(avoidanceDirection);
            return;
        }

        // Jika tidak ada rintangan, bergerak ke tujuan
        Vector3 direction = (destination - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    public bool IsObstacleInDirection(Vector3 origin, Vector3 direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, obstacleAvoidanceDistance))
        {
            if (hit.collider.CompareTag(obstacleTag))
            {
                return true;
            }
        }
        return false;
    }

    // Fungsi untuk mendeteksi rintangan di sekitar zombie
    public bool IsObstacleAround()
    {
        for (int i = 0; i < numberOfRays; i++)
        {
            var rotation = this.transform.rotation;
            var rotationMod = Quaternion.AngleAxis((i / (float)numberOfRays) * angle * 2 - angle, this.transform.up);
            var direction = rotation * rotationMod * Vector3.forward;

            if (IsObstacleInDirection(transform.position, direction))
            {
                return true;  // Ada rintangan, zombie harus menghindar
            }
        }

        return false;  // Tidak ada rintangan, zombie bisa lanjut bergerak
    }

    // Fungsi untuk menampilkan raycast, bisa digunakan di editor atau debug
    void ShowRaycastInAllDirectionsRuntime()
    {
        // Mendapatkan posisi zombie
        Vector3 origin = transform.position;

        // Loop untuk menggambar ray yang ditembakkan berdasarkan angle dan jumlah ray
        for (int i = 0; i < numberOfRays; i++)
        {
            // Hitung sudut rotasi untuk setiap ray berdasarkan jumlah dan angle
            float rotationAngle = (i / (float)numberOfRays) * angle - (angle / 2);  // Membagi sudut angle

            // Membuat rotasi berdasarkan sudut yang dihitung
            Quaternion rotationMod = Quaternion.AngleAxis(rotationAngle, this.transform.up);

            // Arah raycast berdasarkan rotasi
            Vector3 direction = rotationMod * transform.forward;

            // Melakukan raycast dan menggambar ray di Scene
            Debug.DrawRay(origin, direction * obstacleAvoidanceDistance, Color.red);  // Untuk runtime
        }
    }

    // Fungsi untuk mencari arah penghindaran (kanan, kiri, atau mundur)
    private Vector3 GetAvoidanceDirection()
    {
        float rotationStep = 100 * Time.deltaTime; // Kecepatan rotasi zombie untuk mencari jalur penghindaran
        Vector3 rightDirection = Quaternion.Euler(0, rotationStep, 0) * currentDirection;
        Vector3 leftDirection = Quaternion.Euler(0, -rotationStep, 0) * currentDirection;

        if (!IsObstacleInDirection(transform.position, rightDirection))
        {
            currentDirection = rightDirection;
        }
        else if (!IsObstacleInDirection(transform.position, leftDirection))
        {
            currentDirection = leftDirection;
        }
        else
        {
            currentDirection = -transform.forward;
        }

        return currentDirection;  // Arah penghindaran yang dipilih
    }

    public void AttackPlayer()
    {
        Player playerScript = target.GetComponent<Player>();
        if (playerScript != null)
        {
            playerScript.TakeDamage(attackDamage);
            Debug.Log("Zombie attacks Player!");
        }
    }
}
