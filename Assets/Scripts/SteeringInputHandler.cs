using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringInputHandler : MonoBehaviour
{ 

    [SerializeField]
    private AirplaneController _airplaneController;
    [SerializeField]
    private SteeringWheel steeringWheel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float rotation = steeringWheel.GetRotationNormalized();
        _airplaneController.Roll =Mathf.Clamp(rotation,-1,1);
        
    }
}
