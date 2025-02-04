using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnerscript : MonoBehaviour
{
    float time = 0; // butuh waktu berapa
    float timer = 1.5f; //setiap berapa detik sekali
    public GameObject tembok; //untuk mengeliuarkan objek si temboknya 

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (time<=0) // jika time nya kuran dari sama dengan 0 maka temboknya keluar 
        {
            Instantiate(tembok, transform.position, Quaternion.identity); //Meneluarkan tembok
            time = timer; //artinya waktu keluar sesuai dengan timernya
        }
        else
        {
            time -= Time.deltaTime; // jika waktunya waktunya tidak lebih dari <0 maka keluarlah temboknya
        }
    }
}
