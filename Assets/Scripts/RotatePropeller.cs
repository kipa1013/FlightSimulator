using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePropeller : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(Vector3.forward * 5f);
    }
}
