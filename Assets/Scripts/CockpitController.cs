using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CockpitController : MonoBehaviour
{
    public GameObject bg;
    public GameObject bgOuterRing;
    public GameObject plane;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Plane rotation: " + plane.transform.rotation.eulerAngles);
        Debug.Log("Ring rotation: " + bgOuterRing.transform.localRotation.eulerAngles);

        //bgOuterRing.transform.localEulerAngles.Set(0f, -plane.transform.localEulerAngles.z, 0f);
        //bgOuterRing.transform.localRotation = Quaternion.Euler(0f, plane.transform.eulerAngles.z, 0f);
        bgOuterRing.transform.localRotation = Quaternion.Euler(0f, plane.transform.rotation.eulerAngles.z + 180, 0f);
    }
}
