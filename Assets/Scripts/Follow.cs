using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public GameObject followTraget;

    // Update is called once per frame
    void Update()
    {
        transform.position = followTraget.transform.position;
    }
}
