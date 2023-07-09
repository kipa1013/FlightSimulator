using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AirplaneController : MonoBehaviour
{
    [SerializeField]
    List<AeroSurface> controlSurfaces = null;
    [SerializeField]
    List<WheelCollider> wheels = null;
    [SerializeField]
    float rollControlSensitivity = 0.2f;
    [SerializeField]
    float pitchControlSensitivity = 0.2f;
    [SerializeField]
    float yawControlSensitivity = 0.2f;

    [Range(-1, 1)]
    public float Pitch;
    [Range(-1, 1)]
    public float Yaw;
    [Range(-1, 1)]
    public float Roll;
    [Range(0, 1)]
    public float Flap;

    [FormerlySerializedAs("thrustPercent")] public float ThrustPercent;
    [FormerlySerializedAs("brakesTorque")] public float BrakesTorque;

    private PlaneInput planeInput;
    private AudioSource engineSound;
    AircraftPhysics aircraftPhysics;
    Rigidbody rb;

    private void Start()
    {
        aircraftPhysics = GetComponent<AircraftPhysics>();
        rb = GetComponent<Rigidbody>();

    }

    private void Awake()
    {
        planeInput = new PlaneInput();
        planeInput.Plane.Enable();
        engineSound = GetComponent<AudioSource>();
    }
    private void Update()
    {
        //var steeringInput = planeInput.Plane.Steering.ReadValue<Vector3>();
        //Roll = steeringInput.x;
        //Pitch = steeringInput.y;
        //Yaw = steeringInput.z;

        //var thrustInput = planeInput.Plane.Thrust.ReadValue<float>();

        //if (thrustInput > 0) ThrustPercent += 0.1f;
        //else if (thrustInput < 0) ThrustPercent -= 0.1f;
        //ThrustPercent = Mathf.Clamp01(ThrustPercent);

        //if (planeInput.Plane.Flaps.triggered)
        //{
        //    Flap = Flap > 0 ? 0 : 0.3f;
        //}

        //if (planeInput.Plane.Break.triggered)
        //{
        //    BrakesTorque = BrakesTorque > 0 ? 0 : 100f;
        //}
        engineSound.volume = ThrustPercent;
        //engineSound.pitch = Mathf.Clamp01(rb.velocity.magnitude / 100) / 3;
    }

    private void FixedUpdate()
    {
        SetControlSurfecesAngles(Pitch, Roll, Yaw, Flap);
        aircraftPhysics.SetThrustPercent(ThrustPercent);
        foreach (var wheel in wheels)
        {
            wheel.brakeTorque = BrakesTorque;
            // small torque to wake up wheel collider
            wheel.motorTorque = 0.01f;
        }
    }

    public void SetControlSurfecesAngles(float pitch, float roll, float yaw, float flap)
    {
        foreach (var surface in controlSurfaces)
        {
            if (surface == null || !surface.IsControlSurface) continue;
            switch (surface.InputType)
            {
                case ControlInputType.Pitch:
                    surface.SetFlapAngle(pitch * pitchControlSensitivity * surface.InputMultiplyer);
                    break;
                case ControlInputType.Roll:
                    surface.SetFlapAngle(roll * rollControlSensitivity * surface.InputMultiplyer);
                    break;
                case ControlInputType.Yaw:
                    surface.SetFlapAngle(yaw * yawControlSensitivity * surface.InputMultiplyer);
                    break;
                case ControlInputType.Flap:
                    surface.SetFlapAngle(Flap * surface.InputMultiplyer);
                    break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            SetControlSurfecesAngles(Pitch, Roll, Yaw, Flap);
    }

    public void ToggleFlap()
    {
        Flap = Flap > 0 ? 0 : 0.3f;
    }
}
