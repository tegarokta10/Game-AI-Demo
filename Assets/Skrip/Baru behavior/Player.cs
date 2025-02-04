using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float health = 100.0f;
    public GameObject gameOverUI;  // UI untuk Game Over
    public GameObject winUI;  // UI untuk Menang
    public Button restartButton;  // Tombol Restart yang dapat diatur melalui Editor Unity
    public float obstacleAvoidanceDistance = 5.0f;
    public string obstacleTag = "Obs";

    [SerializeField] private Rigidbody rb;  // Rigidbody untuk mengontrol gerakan pemain

    private float lastClickTime = 0f;  // Waktu klik terakhir
    private float doubleClickThreshold = 0.3f;  // Waktu maksimum antara klik untuk dianggap double-click
    private float originalSpeed;  // Menyimpan kecepatan asli untuk reset

    public GameObject bulletPrefab;  // Prefab peluru
    public float bulletCooldown = 1.0f;  // Waktu antara penembakan peluru
    private float lastBulletTime = 0f;  // Waktu terakhir peluru ditembakkan

    void Start()
    {
        rb = GetComponent<Rigidbody>();  // Ambil referensi Rigidbody
        originalSpeed = moveSpeed;  // Simpan kecepatan awal

        // Pastikan UI Game Over dan Win tidak ditampilkan di awal
        gameOverUI.SetActive(false);
        winUI.SetActive(false);

        // Sembunyikan tombol restart di awal
        restartButton.gameObject.SetActive(false);

        // Menambahkan listener untuk tombol restart
        restartButton.onClick.AddListener(RestartGame);
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * obstacleAvoidanceDistance, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, obstacleAvoidanceDistance))
        {
            if (hit.collider.CompareTag("Obs"))
            {
                Debug.Log("Obstacle detected!");
            }
        }

        // Jika health <= 0, pemain tidak bisa bergerak
        if (health <= 0)
            return;

        // Deteksi double-click untuk meningkatkan kecepatan
        if (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical"))
        {
            float currentTime = Time.time;
            if (currentTime - lastClickTime <= doubleClickThreshold)
            {
                // Double-click terdeteksi
                moveSpeed = originalSpeed * 2;  // Tingkatkan kecepatan
                Invoke("ResetSpeed", 1.0f);  // Reset kecepatan setelah 1 detik
            }
            lastClickTime = currentTime;
        }

        // Gerakan pemain menggunakan Rigidbody
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized * moveSpeed;
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        // Update rotasi player sesuai arah gerakan
        if (movement.magnitude > 0)
        {
            transform.rotation = Quaternion.LookRotation(movement);
        }

        // Deteksi input untuk menembakkan peluru
        if (Time.time - lastBulletTime >= bulletCooldown)
        {
            ShootBullet();
            lastBulletTime = Time.time;
        }
    }

    // Metode untuk menerima damage
    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"Player Health After Damage: {health}");

        if (health <= 0)
        {
            health = 0;
            Die();  // Jika health habis, panggil fungsi Die
        }
    }

    void ShootBullet()
    {
        // Pastikan bulletPrefab sudah diassign
        if (bulletPrefab == null)
        {
            Debug.LogError("bulletPrefab is not assigned in the Inspector!");
            return;
        }

        // Buat instance peluru dari prefab
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // Atur arah peluru sesuai dengan arah player saat ini
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.direction = transform.forward; // Gunakan forward player sebagai arah peluru
        }
    }

    // Fungsi untuk memproses kematian player
    void Die()
    {
        Debug.Log("Player has died!");

        // Menampilkan UI Game Over
        gameOverUI.SetActive(true);
        restartButton.gameObject.SetActive(true);  // Menampilkan tombol restart saat Game Over

        // Menonaktifkan kontrol pemain
        this.enabled = false;

        // Menonaktifkan tampilan visual player (opsional)
        GetComponent<Renderer>().enabled = false;
    }


    // Fungsi untuk memproses kemenangan player
    void WinGame()
    {
        Debug.Log("Player wins the game!");

        // Menampilkan UI Menang
        winUI.SetActive(true);
        restartButton.gameObject.SetActive(true);  // Menampilkan tombol restart saat menang

        // Menonaktifkan kontrol pemain
        this.enabled = false;
    }

    // Fungsi untuk memulai ulang game
    public void RestartGame()
    {
        // Memuat ulang scene "Steering Behavior"
        SceneManager.LoadScene("Steering Behavior");
    }

    // Deteksi ketika player mencapai goal
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with: " + other.name);  // Cek nama objek yang terdeteksi

        if (other.CompareTag("Goal"))  // Pastikan target memiliki tag "Goal"
        {
            WinGame();
        }
    }

    // Fungsi untuk mereset kecepatan ke nilai asli
    void ResetSpeed()
    {
        moveSpeed = originalSpeed;  // Kembalikan kecepatan ke nilai asli
    }

    public bool IsObstacleInDirection(Vector3 origin, Vector3 direction)
    {
        Debug.DrawRay(origin, direction * obstacleAvoidanceDistance, Color.red);  // Menampilkan raycast di Scene View untuk debug
        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, obstacleAvoidanceDistance))
        {
            // Jika raycast mengenai objek dengan tag "Obstacle", tampilkan log
            if (hit.collider.CompareTag(obstacleTag))
            {
                Debug.Log($"Obstacle detected in direction {direction} at distance: {hit.distance}");
                return true;  // Jika ada Obstacle, kembalikan true
            }
        }

        return false;  // Tidak ada Obstacle terdeteksi
    }
}   