using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RingInteraction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Collider2D collider = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
