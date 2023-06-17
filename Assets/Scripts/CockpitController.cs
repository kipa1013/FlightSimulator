using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CockpitController : MonoBehaviour
{
    public GameObject bg;
    public GameObject plane;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       bg.transform.localEulerAngles.Set(0f, -plane.transform.localEulerAngles.z, 0f);
    }
}
