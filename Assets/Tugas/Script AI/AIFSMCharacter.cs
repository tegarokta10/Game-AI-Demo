using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIFSMCharacter : MonoBehaviour
{
    // Enum untuk mendefinisikan state FSM
    private enum AIState { Idle, Moving, AvoidingObstacle, SafeZone }

    private AIState currentState; // State saat ini
    Rigidbody2D Rb;
    public float jumpforce = 5f;
    public float rayDistance = 5f;
    public LayerMask obstacleLayer;
    public Color rayColor = Color.red;
    public GameObject pointAtas;
    public GameObject pointBawah;
    public float safeJumpDistance = 3f;
    public int numberOfRays = 5;
    public float spreadAngle = 45f;
    public Text scoreTxt;
    public float adjustmentSpeed = 2f;
    private float score;
    private bool inSafeZone = false;

    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        currentState = AIState.Idle; // Mulai dari state Idle
    }

    void Update()
    {
        // Menampilkan skor
        scoreTxt.text = "Score: " + score;

        // Jalankan logika berdasarkan state saat ini
        switch (currentState)
        {
            case AIState.Idle:
                HandleIdleState();
                break;
            case AIState.Moving:
                HandleMovingState();
                break;
            case AIState.AvoidingObstacle:
                HandleAvoidingObstacleState();
                break;
            case AIState.SafeZone:
                HandleSafeZoneState();
                break;
        }
    }

    // Logika untuk state Idle
    private void HandleIdleState()
    {
        Debug.Log("AI sedang Idle.");
        if (!inSafeZone)
        {
            currentState = AIState.Moving; // Berpindah ke state Moving jika tidak di safe zone
        }
    }

    // Logika untuk state Moving
    private void HandleMovingState()
    {
        Debug.Log("AI sedang bergerak.");
        bool hitObstacle = CheckForObstacles();

        if (hitObstacle)
        {
            currentState = AIState.AvoidingObstacle; // Berpindah ke state AvoidingObstacle jika ada rintangan
        }
        else
        {
            AdjustMovementToSafeZone(); // Jika tidak ada rintangan, tetap menuju safe zone
        }
    }

    // Logika untuk state AvoidingObstacle
    private void HandleAvoidingObstacleState()
    {
        Debug.Log("AI sedang menghindari rintangan.");
        AdjustMovementToSafeZone();
        currentState = AIState.Moving; // Setelah menghindar, kembali ke state Moving
    }

    // Logika untuk state SafeZone
    private void HandleSafeZoneState()
    {
        Debug.Log("AI berada di safe zone.");
        // Jika keluar dari safe zone, kembali ke Moving
        if (!inSafeZone)
        {
            currentState = AIState.Moving;
        }
    }

    // Fungsi untuk memancarkan beberapa Raycast
    void CastRays()
    {
        for (int i = 0; i < numberOfRays; i++)
        {
            float angle = (-spreadAngle / 2) + ((spreadAngle / (numberOfRays - 1)) * i);
            Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.right;
            Debug.DrawRay(transform.position, direction * rayDistance, rayColor);
        }
    }

    // Fungsi untuk mendeteksi rintangan
    bool CheckForObstacles()
    {
        for (int i = 0; i < numberOfRays; i++)
        {
            float angle = (-spreadAngle / 2) + ((spreadAngle / (numberOfRays - 1)) * i);
            Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.right;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayDistance, obstacleLayer);

            if (hit.collider != null)
            {
                Debug.Log("Rintangan terdeteksi: " + hit.collider.name);
                return true;
            }
        }
        return false;
    }

    // Fungsi untuk menyesuaikan gerakan menuju safe zone
    void AdjustMovementToSafeZone()
    {
        Vector2 pointAtasPos = pointAtas.transform.position;
        Vector2 pointBawahPos = pointBawah.transform.position;

        float safeZoneCenterY = (pointAtasPos.y + pointBawahPos.y) / 2;

        if (transform.position.y < safeZoneCenterY)
        {
            Rb.velocity = new Vector2(Rb.velocity.x, jumpforce);
        }
        else if (transform.position.y > safeZoneCenterY)
        {
            Rb.velocity = new Vector2(Rb.velocity.x, -jumpforce);
        }
    }

    // Trigger untuk memasuki safe zone
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "point")
        {
            score++;
            inSafeZone = true;
            currentState = AIState.SafeZone; // Berpindah ke state SafeZone
            Debug.Log("Masuk safe zone.");
        }
        if (collision.gameObject.tag == "tembok")
        {
            Destroy(gameObject); // Game over jika terkena tembok
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "point")
        {
            inSafeZone = false; // Keluar dari safe zone
            Debug.Log("Keluar dari safe zone.");
        }
    }
}
