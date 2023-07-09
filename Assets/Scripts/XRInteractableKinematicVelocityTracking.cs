using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

public class XRInteractableKinematicVelocityTracking : XRGrabInteractable
{
    private Rigidbody _rb;

    [SerializeField] private AirplaneController controller;
    [SerializeField] private Rigidbody referenceBody;
    [SerializeField] private Transform anchor;
    [FormerlySerializedAs("maxPositionZ")] [SerializeField] private float minPositionz;
    
    protected override void Awake()
    {
        base.Awake();
        _rb = GetComponent<Rigidbody>();
        SetupRigidbodyGrab(_rb);

    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        float pose = interactorsSelecting[0].transform.position.z;
        float target = Mathf.Clamp(pose, anchor.position.z-minPositionz, anchor.position.z );
        Vector3 position = new Vector3 { x = anchor.position.x, y = anchor.position.y, z = target };
        _rb.MovePosition(position);

        var rotation = interactorsSelecting[0].transform.rotation;
        var zRotation = rotation.z;

        var angle = 180 * zRotation;

        _rb.MoveRotation(Quaternion.AngleAxis(angle, Vector3.forward));


        var pitchInput = (target - anchor.position.z) / (minPositionz - anchor.position.z);
        controller.Pitch = pitchInput;
        controller.Roll = zRotation;

    }
}

