using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

public class FakeGrab : MonoBehaviour
{
    private DateTime lastPress;
    private Vector3 previosPosition;
    private Quaternion previosRotation;
    [SerializeField] private GameObject controllerGameObject;
    // Start is called before the first frame update
    void Start()
    {
        lastPress = DateTime.UtcNow;

    }

    void FixedUpdate()
    {
        var leftHandedControllers = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Left, leftHandedControllers);
        var leftController = leftHandedControllers.FirstOrDefault();


       // Debug.Log(_leftController.name);
        if (leftController.isValid)
        {  
            leftController.TryGetFeatureValue(CommonUsages.gripButton, out bool gripButtonPressed);
            if (gripButtonPressed)
            {
                leftController.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 pos);
                TimeSpan span = DateTime.UtcNow - lastPress;
                
                if (span.TotalMilliseconds > 500)
                {
                    var distance = Vector3.Distance(controllerGameObject.transform.position, transform.position);
                    if (distance > 0.15f) return;
                    previosPosition = pos;
                    lastPress = DateTime.UtcNow;
                    return;
                }

                var deltaInputPos = pos - previosPosition;

                var lowerGuard = Mathf.Clamp(-0.16f - transform.localPosition.z, -0.16f, 0);
                var upperGuard = -transform.localPosition.z;

                var zTranslation = Mathf.Clamp(deltaInputPos.z, lowerGuard, upperGuard);
                Vector3 translation = new Vector3 { x = 0, y = 0, z = zTranslation };              
                transform.Translate(translation, Space.Self);

                leftController.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation);
                var inputAngle = GetRotation(rotation.eulerAngles) * 0.01f;
                var currentAngle = GetRotation(transform.localRotation.eulerAngles);

                var predictAngle = inputAngle + currentAngle;
                if(predictAngle >= 90f || predictAngle <= -90f)
                {
                    inputAngle = 0f;
                }

                transform.Rotate(Vector3.forward, inputAngle, Space.Self);

                // set variables for next iteration
                lastPress = DateTime.UtcNow;
                previosPosition = pos;
            }
        }
    }

    private float GetRotation(Vector3 euler)
    {
        float degrees = euler.z;
        if (degrees > 180)
        {
            degrees = -(360f - degrees);
        }

        return degrees;
    }
}
