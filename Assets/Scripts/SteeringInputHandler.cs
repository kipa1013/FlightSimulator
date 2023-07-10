using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

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
        _airplaneController.Roll = -Mathf.Clamp(GetRotationNormalized(yolk),-1,1);
        var yokePos = yolk.transform.localPosition.z / 0.16f;
        var transformYoke = yokePos + 0.5f;
        _airplaneController.Pitch = Mathf.Clamp(transformYoke * 2, -1, 1);
        SetThrustAndBrake();
        //_airplaneController.Yaw =

        // Setup VR Controllers
        var controllers = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller, controllers);
        var leftController = controllers.FirstOrDefault(c => c.characteristics.HasFlag(InputDeviceCharacteristics.Left));
        var rightController = controllers.FirstOrDefault(c => c.characteristics.HasFlag(InputDeviceCharacteristics.Right));
        Debug.Log(controllers);

        if(leftController.isValid && rightController.isValid)
        {
            leftController.TryGetFeatureValue(CommonUsages.triggerButton, out var leftTrigger);
            rightController.TryGetFeatureValue(CommonUsages.triggerButton, out var rightTrigger);
            if (leftTrigger && rightTrigger)
            {
                _airplaneController.BrakesTorque = 100f;
                Debug.Log("BREAKING");
                _airplaneController.Yaw = 0f;
            }
            else
            {
                _airplaneController.BrakesTorque = 0f;
                _airplaneController.Yaw = leftTrigger ? 1 : rightTrigger ? -1 : 0;
            }
        }
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

        var normalized = degrees/90;
        //Debug.Log(normalized);
        return normalized;

    }

    public void SetThrustAndBrake()
    {
        if (thrust.transform.localPosition.y > 0.01)
        {
            //_airplaneController.BrakesTorque = 0f;
            _airplaneController.ThrustPercent = 1f;
            //Mathf.Clamp01(Mathf.Abs(thrust.transform.localPosition.z*10));
        }
        else
        {
            _airplaneController.ThrustPercent = 0f;
            //_airplaneController.BrakesTorque = 100f;
        }
        
    }
}
