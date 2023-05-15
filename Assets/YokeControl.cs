using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class YokeControl : MonoBehaviour
{
    // XR Input Devices
    

    // Right Hand
    public GameObject rightHand;
    private Transform rightHandOriginalParent;
    private bool rightHandOnYoke = false;
    public Transform rightSnap;

    // Left Hand
    public GameObject leftHand;
    private Transform leftHandOriginalParent;
    private bool leftHandOnYoke = false;
    public Transform leftSnap;

    public Transform directionalObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("PlayerHand"))
        {
            // Place Right Hand
            InputDevice rightHandDev = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            bool triggerValue;
            if (rightHandOnYoke && rightHandDev.TryGetFeatureValue(CommonUsages.triggerButton, out triggerValue) && triggerValue)
            {
                PlaceHandOnWheel(ref rightHand, ref rightSnap, ref rightHandOriginalParent, ref rightHandOnYoke);
            }

            // Place Left Hand
            InputDevice leftHandDev = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            if (leftHandOnYoke && leftHandDev.TryGetFeatureValue(CommonUsages.triggerButton, out triggerValue) && triggerValue)
            {
                PlaceHandOnWheel(ref leftHand, ref leftSnap, ref leftHandOriginalParent, ref leftHandOnYoke);
            }
        }
    }

    private void ReleaseHandsFromWheel() {
        InputDevice rightHandDev = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        bool triggerValue;
        if (rightHandOnYoke && rightHandDev.TryGetFeatureValue(CommonUsages.triggerButton, out triggerValue) && !triggerValue)
        {
            rightHand.transform.parent = rightHandOriginalParent;
            rightHand.transform.position = rightHandOriginalParent.position;
            rightHandOnYoke = false;
        }
        InputDevice leftHandDev = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        if (leftHandOnYoke && leftHandDev.TryGetFeatureValue(CommonUsages.triggerButton, out triggerValue) && !triggerValue)
        {
            leftHand.transform.parent = leftHandOriginalParent;
            leftHand.transform.position = leftHandOriginalParent.position;
            leftHandOnYoke = false;
        }
    }

    private void PlaceHandOnWheel(ref GameObject hand, ref Transform snap, ref Transform originalParent, ref bool handOnYoke) {
        originalParent = hand.transform.parent;
        hand.transform.parent = snap.transform;
        hand.transform.position = snap.transform.position;

        handOnYoke = true;
    }

    private void ConvertHandRotationToYokeMovement() {
        if (rightHandOnYoke && !leftHandOnYoke)
        {
            Quaternion newRot = Quaternion.Euler(0, 0, rightHandOriginalParent.transform.rotation.eulerAngles.z);
            directionalObject.rotation = newRot;
            transform.parent = directionalObject;
        }
        else if (!rightHandOnYoke && leftHandOnYoke)
        {
            Quaternion newRot = Quaternion.Euler(0, 0, leftHandOriginalParent.transform.rotation.eulerAngles.z);
            directionalObject.rotation = newRot;
            transform.parent = directionalObject;
        }
        else if (rightHandOnYoke && leftHandOnYoke)
        {
            Quaternion newRotLeft = Quaternion.Euler(0, 0, leftHandOriginalParent.transform.rotation.eulerAngles.z);
            Quaternion newRotRight = Quaternion.Euler(0, 0, rightHandOriginalParent.transform.rotation.eulerAngles.z);
            Quaternion finalRot = Quaternion.Slerp(newRotLeft, newRotRight, 1.0f / 2.0f);
            directionalObject.rotation = finalRot;
            transform.parent = directionalObject;
        }


    }
}
