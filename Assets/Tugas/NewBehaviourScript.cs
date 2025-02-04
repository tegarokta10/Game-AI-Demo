using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    Rigidbody2D Rb; // Inisialisasi 
    public float jumpforce; //variabel yang di gunakan untuk membuat loncat"
    // Start is called before the first frame update
    float score;
    public Text scoreTxt;

    void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreTxt.text ="Score: " + score;


        if (Input.GetMouseButtonDown(0))
            {
                Rb.velocity = Vector2.up * jumpforce; // velocity = fungsi loncat dari rb
            }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag=="point" )
        {
            score++;
        }
        if (collision.gameObject.tag=="tembok")
        {
            Destroy(gameObject);
        }
    }
}
