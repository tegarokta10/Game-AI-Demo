using UnityEngine;

public class Character : MonoBehaviour
{
    public float moveSpeed = 5.0f; // Kecepatan pergerakan NPC
    public float obstacleAvoidanceDistance = 3.0f; // Jarak deteksi rintangan
    public float smoothTime = 0.2f; // Waktu interpolasi untuk perubahan arah
    public string obstacleTag = "Obstacle"; // Tag untuk objek rintangan

    private Vector3 currentDirection;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        // Inisialisasi arah awal NPC
        currentDirection = transform.forward;
    }

    void Update()
    {
        // Tentukan arah gerakan berdasarkan deteksi rintangan
        Vector3 targetDirection = currentDirection; // Default arah jika tidak ada rintangan

        // Cek rintangan di depan
        if (IsObstacleInDirection(transform.position, transform.forward))
        {
            if (!IsObstacleInDirection(transform.position, transform.right))
            {
                targetDirection = transform.right; // Menghindar ke kanan
            }
            else if (!IsObstacleInDirection(transform.position, -transform.right))
            {
                targetDirection = -transform.right; // Menghindar ke kiri
            }
            else
            {
                targetDirection = -transform.forward; // Jika tidak bisa ke kanan atau kiri, mundur
            }
        }
        // Cek rintangan di kanan
        else if (IsObstacleInDirection(transform.position, transform.right))
        {
            targetDirection = -transform.right; // Menghindar ke kiri
        }
        // Cek rintangan di kiri
        else if (IsObstacleInDirection(transform.position, -transform.right))
        {
            targetDirection = transform.right; // Menghindar ke kanan
        }
        // Cek rintangan di belakang
        else if (IsObstacleInDirection(transform.position, -transform.forward))
        {
            targetDirection = transform.forward; // Menghindar ke depan
        }

        // Interpolasi untuk menghaluskan perubahan arah
        currentDirection = Vector3.SmoothDamp(currentDirection, targetDirection, ref velocity, smoothTime);

        // Gerakkan NPC sesuai arah yang dihaluskan
        transform.position += currentDirection * moveSpeed * Time.deltaTime;

        // Rotasi NPC untuk menghadap arah gerakan
        transform.rotation = Quaternion.LookRotation(currentDirection);
    }

    bool IsObstacleInDirection(Vector3 origin, Vector3 direction)
    {
        RaycastHit hit;
        // Raycast untuk mendeteksi rintangan
        if (Physics.Raycast(origin, direction, out hit, obstacleAvoidanceDistance))
        {
            // Cek apakah objek yang terdeteksi memiliki tag "Obstacle"
            if (hit.collider.CompareTag(obstacleTag))
            {
                return true;
            }
        }
        return false;
    }

    void OnDrawGizmos()
    {
        // Visualisasi raycast di Scene View
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * obstacleAvoidanceDistance);
        Gizmos.DrawRay(transform.position, transform.right * obstacleAvoidanceDistance);
        Gizmos.DrawRay(transform.position, -transform.right * obstacleAvoidanceDistance);
        Gizmos.DrawRay(transform.position, -transform.forward * obstacleAvoidanceDistance);
    }
}
