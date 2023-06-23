using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringInputHandler : MonoBehaviour
{

    [SerializeField] 
    private AirplaneController _airplaneController;
    [SerializeField]
    private GameObject yolk;
    [SerializeField]
    private GameObject thrust;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _airplaneController.Roll =Mathf.Clamp(GetRotationNormalized(yolk),-1,1);
        _airplaneController.Pitch = Mathf.Clamp(yolk.transform.localPosition.z*10,-1,1);
        SetThrustAndBrake();
        //_airplaneController.Yaw =
    }
    public float GetRotationNormalized(GameObject reference)
    {
        /*Debug.Log("Local Rotation:"+wheelTransform.localRotation);
        Debug.Log("global Rotation:"+this.wheelTransform.rotation);
        Debug.Log("global EulerAngles:"+this.wheelTransform.eulerAngles);
        Debug.Log("local EulerAngles:"+this.wheelTransform.localEulerAngles);*/
        float degrees = reference.transform.localEulerAngles.z;
        if (degrees > 180)
        {
            degrees= -(360f - degrees);
        }

        var normalized = degrees/180;
        //Debug.Log(normalized);
        return normalized;

    }

    public void SetThrustAndBrake()
    {
        if (thrust.transform.localPosition.z < 0)
        {
            _airplaneController.BrakesTorque = 0f;
            _airplaneController.ThrustPercent = Mathf.Clamp01(Mathf.Abs(thrust.transform.localPosition.z*10));
        }
        else
        {
            _airplaneController.ThrustPercent = 0f;
            _airplaneController.BrakesTorque = 100f;
        }
        
    }
}
