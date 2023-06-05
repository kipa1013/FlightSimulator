using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringActions : MonoBehaviour
{
    [SerializeField] private SteeringWheel steeringLeaver;
    private PlaneController planeController;
    [SerializeField] private SteeringWheel _steeringWheel;
    private void Awake()
    {
        planeController = GetComponentInParent<PlaneController>();
        Debug.Log("FOUND");
    }

    public void setThrust()
    {
        
        planeController.throttle = Mathf.Clamp(steeringLeaver.GetRotationNormalized()*500, 0f, 100f);
        Debug.Log(planeController.throttle);
        return;
    }

    public void setRoll()
    {
        planeController.roll = _steeringWheel.GetRotationNormalized();
        
    }
    
}
