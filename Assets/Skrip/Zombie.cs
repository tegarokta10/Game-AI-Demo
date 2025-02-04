using UnityEngine;

public class Zombie : MonoBehaviour
{
    public Transform target;  // Referensi ke target yang akan dikejar oleh NPC.
    public float chaseSpeed = 5.0f; // Kecepatan pengejaran NPC.
    public float stopDistance = 1.0f; // Jarak minimum di mana NPC akan berhenti mengejar target.
    public string obstacleTag = "Obstacle"; // Tag untuk objek rintangan
    public float obstacleAvoidanceDistance = 3.0f; // Jarak deteksi rintangan
    public int numberOfRays = 17; // Jumlah ray yang ditembakkan untuk deteksi rintangan
    public float angle = 90.0f; // Sudut distribusi ray di sekitar NPC

    private bool isChasing = true; // Menentukan apakah NPC sedang mengejar target atau tidak.
    private Vector3 currentDirection; // Menyimpan arah saat ini NPC.

    void Start()
    {
        currentDirection = transform.forward; // Inisialisasi arah NPC.
    }

    void Update()
    {
        if (target == null)
        {
            if (isChasing)
            {
                StopChase();
            }
            return;
        }

        if (isChasing)
        {
            // Dapatkan arah menuju target.
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            // Cek apakah ada rintangan di arah tujuan.
            if (IsObstacleInDirection(transform.position, directionToTarget))
            {
                // Logika menghindar jika ada rintangan di depan.
                AvoidObstacle();
            }
            else
            {
                // Jika tidak ada rintangan, bergerak ke arah target.
                currentDirection = directionToTarget; // Simpan arah saat ini.
                MoveInDirection(currentDirection);
            }

            // Cek jarak antara NPC dan target.
            float distance = Vector3.Distance(transform.position, target.position);

            // Jika jarak cukup dekat.
            if (distance < stopDistance)
            {
                StopChase();
                AttackTarget();
            }
        }
    }

    void MoveInDirection(Vector3 direction)
    {
        transform.position += direction * chaseSpeed * Time.deltaTime; // Gerakkan NPC menuju arah yang ditentukan.
    }

    void AvoidObstacle()
    {
        // Logika untuk menghindari rintangan
        float rotationStep = 100 * Time.deltaTime; // Kecepatan rotasi NPC untuk mencari jalur
        Vector3 rightDirection = Quaternion.Euler(0, rotationStep, 0) * currentDirection;
        Vector3 leftDirection = Quaternion.Euler(0, -rotationStep, 0) * currentDirection;

        // Coba putar ke kanan jika tidak ada rintangan
        if (!IsObstacleInDirection(transform.position, rightDirection))
        {
            currentDirection = rightDirection;
        }
        // Jika ada rintangan ke kanan, coba putar ke kiri
        else if (!IsObstacleInDirection(transform.position, leftDirection))
        {
            currentDirection = leftDirection;
        }
        // Jika tidak bisa ke kanan atau kiri, coba mundur
        else
        {
            currentDirection = -transform.forward;
        }

        // Bergerak dalam arah yang ditentukan
        MoveInDirection(currentDirection);
    }

    void StopChase()
    {
        Debug.Log("Chase stopped.");
        isChasing = false;
        chaseSpeed = 0f;
    }

    void AttackTarget()
    {
        Debug.Log("Attacking target!");

        if (target != null)
        {
            Destroy(target.gameObject);
            Debug.Log("NPC2 caught and destroyed!");
            target = null;
        }
    }

    bool IsObstacleInDirection(Vector3 origin, Vector3 direction)
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

    // Fungsi OnDrawGizmos yang menggambar ray di Scene View Unity
    void OnDrawGizmos()
    {
        // Loop untuk menggambar ray yang ditembakkan dari NPC
        for (int i = 0; i < numberOfRays; i++)
        {
            // Ambil rotasi objek saat ini (NPC)
            var rotation = this.transform.rotation;

            // Modifikasi rotasi untuk ray ke-i, menggunakan sudut yang berbeda-beda
            var rotationMod = Quaternion.AngleAxis((i / ((float)numberOfRays)) * angle * 2 - angle, this.transform.up);

            // Hitung arah ray berdasarkan rotasi objek dan modifikasi rotasi
            var direction = rotation * rotationMod * Vector3.forward;

            // Gambar ray di editor Unity untuk memberikan visualisasi
            Gizmos.color = Color.red;
            Gizmos.DrawRay(this.transform.position, direction * obstacleAvoidanceDistance);
        }
    }
}
