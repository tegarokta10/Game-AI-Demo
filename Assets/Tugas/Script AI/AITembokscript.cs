using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITembokscript : MonoBehaviour
{
    float nilairandom;
    void Start()
    {
        // Posisi acak di antara batas atas dan bawah yang bisa dilewati
        nilairandom = Random.Range(-3f, 1.7f); 
        transform.position = new Vector2(transform.position.x, nilairandom);
    }

    void Update()
    {
        // Gerakkan tembok ke kiri
        transform.position = Vector2.MoveTowards(transform.position, Vector2.left * 100, Time.deltaTime * 5);
    }
}
