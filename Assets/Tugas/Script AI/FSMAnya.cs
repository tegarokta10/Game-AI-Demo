using UnityEngine;
using UnityEngine.UI;

public class FSMAnya : MonoBehaviour
{
    enum State { Idle, AvoidObstacle, InSafeZone, GameOver }
    State currentState = State.Idle;

    [SerializeField]
    private Rigidbody2D rb;

    public float jumpforce = 5f;
    public float rayDistance = 5f;
    public LayerMask obstacleLayer;
    public GameObject pointAtas;
    public GameObject pointBawah;
    public float spreadAngle = 45f;
    public int numberOfRays = 5;
    public Text scoreTxt;

    private float score = 0;
    private bool hitObstacle = false;

    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        SetState(State.Idle);
    }

    void Update()
    {
        scoreTxt.text = "Score: " + score;

        switch (currentState)
        {
            case State.Idle:
                HandleIdle();
                break;
            case State.AvoidObstacle:
                HandleAvoidObstacle();
                break;
            case State.InSafeZone:
                HandleInSafeZone();
                break;
            case State.GameOver:
                HandleGameOver();
                break;
        }
    }

    void CastRays()
    {
        for (int i = 0; i < numberOfRays; i++)
        {
            // Hitung sudut untuk setiap Raycast
            float angle = (-spreadAngle / 2) + ((spreadAngle / (numberOfRays - 1)) * i);
            Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.right; // Arah Raycast

            // Debug.DrawRay akan menggambar ray di Scene View
            Debug.DrawRay(transform.position, direction * rayDistance, Color.red);

            // Raycast untuk deteksi
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayDistance, obstacleLayer);
            if (hit.collider != null)
            {
                // Jika rintangan terdeteksi, ganti warna garis menjadi hijau
                Debug.DrawRay(transform.position, direction * hit.distance, Color.green);
                Debug.Log("Obstacle detected at angle: " + angle + " with distance: " + hit.distance);
            }
        }
    }

    void SetState(State newState)
    {
        currentState = newState;
    }

    void HandleIdle()
    {
        hitObstacle = CheckForObstacles();
        if (hitObstacle)
        {
            SetState(State.AvoidObstacle);
        }
    }

    void HandleAvoidObstacle()
    {
        hitObstacle = CheckForObstacles();
        AdjustMovementToSafeZone();

        if (!hitObstacle)
        {
            SetState(State.Idle);
        }
    }

    void HandleInSafeZone()
    {
        // Menunggu keluar dari zona aman
    }

    void HandleGameOver()
    {
        rb.velocity = Vector2.zero;
        // Tambahkan logika akhir game di sini
    }

    bool CheckForObstacles()
    {
        for (int i = 0; i < numberOfRays; i++)
        {
            float angle = (-spreadAngle / 2) + ((spreadAngle / (numberOfRays - 1)) * i);
            Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.right;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayDistance, obstacleLayer);

            Debug.DrawRay(transform.position, direction * rayDistance, Color.red);

            if (hit.collider != null && hit.distance < rayDistance)
            {
                return true;
            }
        }
        return false;
    }

    void AdjustMovementToSafeZone()
    {
        Vector2 pointAtasPos = pointAtas.transform.position;
        Vector2 pointBawahPos = pointBawah.transform.position;

        float safeZoneCenterY = (pointAtasPos.y + pointBawahPos.y) / 2;
        float distanceToSafeZone = Mathf.Abs(transform.position.y - safeZoneCenterY);

        if (distanceToSafeZone > 0.1f)
        {
            if (transform.position.y < safeZoneCenterY)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpforce);
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, -jumpforce);
            }
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("point"))
        {
            score++;
            SetState(State.InSafeZone);
        }
        if (collision.gameObject.CompareTag("tembok"))
        {
            SetState(State.GameOver);
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("point"))
        {
            SetState(State.Idle);
        }
    }
}
