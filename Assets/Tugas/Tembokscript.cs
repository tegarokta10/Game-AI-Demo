using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tembokscript : MonoBehaviour
{
    float nilairandom;
    void Start()
    {
        nilairandom = Random.Range(-3f, 1.7f); //Nilai f digunakan untuk desimal pada f loat
        transform.position = new Vector2(transform.position.x, nilairandom);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position,Vector2.left * 100,Time.deltaTime * 5 );
    }
}
