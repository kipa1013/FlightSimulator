using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CockpitController : MonoBehaviour
{
    // Flight Stats input fields:
    private float maxThrust = 120000f;
    private float lift = 280f;
    private float throttle;
    private float lastThrust = 0f;
    private float roll;
    private float pitch;
    private float yaw;
    private Vector3 velocity;
    private Vector3 position;

    public float MaxThrust { get => maxThrust; set => maxThrust = value; }
    public float Lift { get => lift; set => lift = value; }
    public float Throttle { get => throttle; set => throttle = value; }
    public float LastThrust { get => lastThrust; set => lastThrust = value; }
    public float Roll { get => roll; set => roll = value; }
    public float Pitch { get => pitch; set => pitch = value; }
    public float Yaw { get => yaw; set => yaw = value; }
    public Vector3 Velocity { get => velocity; set => velocity = value; }
    public Vector3 Position { get => position; set => position = value; }

    // CockpitController fields:
    private float airspeedInKnots;
    private float heightInFeet;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        airspeedInKnots = velocity.magnitude * 1.94384f;
        heightInFeet = position.y * 3.28084f;
    }
}
