using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSteeringInteractions : MonoBehaviour
{
    [SerializeField] private Transform airplane;

    private Transform oldTransform;
    // Start is called before the first frame update
    void Start()
    {
        oldTransform = airplane;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 toMove = airplane.position - oldTransform.position;
        transform.Translate(toMove);
    }
}
