using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawnerscript : MonoBehaviour
{
    float time = 0; // Butuh waktu berapa lama
    float timer = 1.5f; // Setiap berapa detik sekali
    public GameObject tembok; // Untuk mengeluarkan objek si tembok

    void Update()
    {
        if (time <= 0) // Jika waktunya <= 0 maka tembok keluar
        {
            Instantiate(tembok, transform.position, Quaternion.identity); // Mengeluarkan tembok
            time = timer; // Reset waktu sesuai timer
        }
        else
        {
            time -= Time.deltaTime; // Hitung mundur waktu
        }
    }
}
