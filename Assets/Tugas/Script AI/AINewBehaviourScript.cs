using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AINewBehaviourScript : MonoBehaviour
{
    private Rigidbody2D rb; // Inisialisasi Rigidbody2D
    public float jumpForce = 5f; // Kekuatan lompat
    public float rayDistance = 5f; // Jarak Raycast untuk deteksi tembok
    public LayerMask obstacleLayer; // Layer tembok
    public Text scoreTxt;

    private float score = 0; // Skor pemain
    private AIState currentState; // State saat ini
    private bool isAlive = true; // Status hidup/mati

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        TransitionToState(AIState.Idle); // Mulai dari state Idle
    }

    void Update()
    {
        if (!isAlive) return; // Jika mati, hentikan update
        currentState = UpdateState(currentState); // Perbarui state sesuai logika
    }

    private AIState UpdateState(AIState state)
    {
        switch (state)
        {
            case AIState.Idle:
                return HandleIdle();

            case AIState.Navigate:
                return HandleNavigate();

            case AIState.GameOver:
                HandleGameOver();
                break;
        }
        return state;
    }

    private void TransitionToState(AIState newState)
    {
        currentState = newState;
        Debug.Log($"Transisi ke state: {newState}");
    }

    private AIState HandleIdle()
    {
        // Memulai game saat karakter menerima input untuk mulai
        Debug.Log("Menunggu untuk mulai...");
        return AIState.Navigate; // Otomatis masuk ke Navigate untuk AI
    }

    private AIState HandleNavigate()
    {
        // Deteksi rintangan di depan
        if (CheckForObstacles())
        {
            Debug.Log("Melompat untuk menghindari rintangan...");
            Jump(); // Lompat jika ada rintangan
        }
        scoreTxt.text = "Score: " + score;
        return AIState.Navigate; // Tetap di state Navigate
    }

    private void HandleGameOver()
    {
        Debug.Log("Game Over!"); // Tampilan Game Over
        isAlive = false; // Matikan karakter
    }

    private bool CheckForObstacles()
    {
        // Raycast untuk mendeteksi rintangan di depan
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, rayDistance, obstacleLayer);
        Debug.DrawRay(transform.position, Vector2.right * rayDistance, Color.red); // Debug Ray
        return hit.collider != null; // True jika ada rintangan
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce); // Lompat
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("point"))
        {
            score++; // Tambah skor saat melewati poin
            Debug.Log("Skor: " + score);
        }
        else if (collision.CompareTag("tembok"))
        {
            TransitionToState(AIState.GameOver); // Masuk state GameOver
        }
    }
}

public enum AIState
{
    Idle,       // State menunggu
    Navigate,   // State bergerak maju
    GameOver    // State mati
}
