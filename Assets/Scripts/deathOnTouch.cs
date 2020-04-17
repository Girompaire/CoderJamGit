using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathOnTouch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.GetComponent<PlayerController>().GetComponent<Rigidbody2D>().velocity *= 0;
        collision.gameObject.GetComponent<PlayerController>().transform.position = collision.gameObject.GetComponent<PlayerController>().firstPosition;
    }
}
