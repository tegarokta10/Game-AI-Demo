using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5.0f;  // Kecepatan peluru
    public float detectionRadius = 5.0f;  // Radius deteksi musuh
    public string enemyTag = "Enemy";  // Tag musuh

    public Vector3 direction;  // Arah gerakan peluru (diatur dari Player)

    void Start()
    {
        // Jika direction belum diatur, default ke kanan
        if (direction == Vector3.zero)
            direction = transform.right;
    }

    void Update()
    {
        // Gerakkan peluru ke arah yang ditentukan
        transform.Translate(direction * speed * Time.deltaTime);

        // Deteksi musuh di sekitar peluru
        DetectEnemy();
    }

    void DetectEnemy()
    {
        // Cari semua collider di sekitar peluru
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(enemyTag))
            {
                // Jika menemukan musuh, hancurkan musuh dan peluru
                Destroy(hitCollider.gameObject);
                Destroy(gameObject);
                return;  // Keluar dari loop setelah menemukan musuh pertama
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Gambar radius deteksi di Scene View untuk debug
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}