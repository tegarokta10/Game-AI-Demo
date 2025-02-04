using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 5.0f;  // Kecepatan pergerakan NPC
    public float rotationSpeed = 700.0f;  // Kecepatan rotasi NPC

    void Update()
    {
        // Pergerakan NPC ke arah depan
        MoveForward();

        // Rotasi NPC jika diperlukan (misalnya, berdasarkan input atau logika lain)
        RotateNPC();
    }

    void MoveForward()
    {
        // Gerakkan NPC ke arah depan
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    void RotateNPC()
    {
        // Rotasi NPC berdasarkan input atau logika lain
        // Misalnya, rotasi berdasarkan arah mouse atau input pengguna
        // (Jika tidak perlu rotasi tambahan, Anda bisa mengabaikan fungsi ini)
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput).normalized;
        if (direction.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
