using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPower : MonoBehaviour
{
    private int power;
    public GameObject square;

    private void Awake()
    {
        power = Random.Range(1,5);
        if (power == 1)
        {
            this.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1); 
        }
        if (power == 2)
        {
            this.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 1);
        }
        if (power == 3)
        {
            this.GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 1);
        }
        if (power == 4)
        {
            this.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

     

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (power == 1)
            {
                deathOnTouch(collision.gameObject);
                Destroy(this.gameObject);
            }
            if (power == 2)
            {
                swapPlayer(collision.gameObject);
                Destroy(this.gameObject);
            }
            if (power == 3)
            {
                tpTop(collision.gameObject);
                Destroy(this.gameObject);
            }
            if (power == 4)
            {
                teleportToOtherPlayer(collision.gameObject);
                Destroy(this.gameObject);
            }
        }
    }

    private void deathOnTouch(GameObject cible)
    {
        cible.GetComponent<PlayerController>().GetComponent<Rigidbody2D>().velocity *= 0;
        cible.GetComponent<PlayerController>().transform.position = cible.GetComponent<PlayerController>().firstPosition;
    }

    private void swapPlayer(GameObject cible)
    { 
        GameObject[] otherPlayer;
        otherPlayer = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject item in otherPlayer)
        {
            if (item.GetComponent<PlayerController>().id != cible.gameObject.GetComponent<PlayerController>().id)
            {
                Transform cibleLastPosition = cible.transform;

                cible.transform.position = item.transform.position;
                item.transform.position = cibleLastPosition.transform.position;

            }
        }
    }

    private void tpTop(GameObject cible)
    {
        GameObject forme = Instantiate(square, cible.transform.position + new Vector3(0,10,0) , Quaternion.identity);
        forme.GetComponent<SpriteRenderer>().color = cible.GetComponent<PlayerController>().colorPlayer;
        cible.transform.position = cible.transform.position + new Vector3(0, 11, 0);
    }

    private void teleportToOtherPlayer(GameObject cible)
    {
        GameObject[] otherPlayer;
        otherPlayer = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject item in otherPlayer)
        {
            if (item.GetComponent<PlayerController>().id != cible.gameObject.GetComponent<PlayerController>().id)
            {
                cible.transform.position = item.transform.position;
            }
        }
    }
}

