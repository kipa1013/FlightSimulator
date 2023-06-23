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
    [SerializeField]
    Text displayText = null;

    [FormerlySerializedAs("thrustPercent")] public float ThrustPercent;
    [FormerlySerializedAs("brakesTorque")] public float BrakesTorque;

    private PlaneInput planeInput;
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
    }
    private void Update()
    {
        var steeringInput = planeInput.Plane.Steering.ReadValue<Vector3>();
        //Roll = steeringInput.x;
        Pitch = steeringInput.y;
        Yaw = steeringInput.z;

        var thrustInput = planeInput.Plane.Thrust.ReadValue<float>();

        if (thrustInput > 0) ThrustPercent += 0.1f;
        else if (thrustInput < 0) ThrustPercent -= 0.1f;
        ThrustPercent = Mathf.Clamp01(ThrustPercent);

        if (planeInput.Plane.Flaps.triggered)
        {
            Flap = Flap > 0 ? 0 : 0.3f;
        }

        if (planeInput.Plane.Break.triggered)
        {
            BrakesTorque = BrakesTorque > 0 ? 0 : 100f;
        }

        displayText.text = "V: " + ((int)rb.velocity.magnitude).ToString("D3") + " m/s\n";
        displayText.text += "A: " + ((int)transform.position.y).ToString("D4") + " m\n";
        displayText.text += "T: " + (int)(ThrustPercent * 100) + "%\n";
        displayText.text += BrakesTorque > 0 ? "B: ON\n" : "B: OFF\n";
        displayText.text += Flap > 0 ? "F: ON" : "F: OFF";
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
}
