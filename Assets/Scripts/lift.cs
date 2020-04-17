using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lift : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<PlayerController>().myGravity = -5;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<PlayerController>().myGravity = 20;

    }
}
