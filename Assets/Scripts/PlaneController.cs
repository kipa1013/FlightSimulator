using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;


public class PlaneController : MonoBehaviour
{
    [Header("Plane Stats")]
    public float throttleIncremnet = 0.1f;
    public float maxThrust = 120000f;
    public AnimationCurve PowerCurve;
    public float responsivness = 10f;

    private float throttle;
    private float roll;
    private float pitch;
    private float yaw;
    private readonly float wingSpan = 11f; //36f;
    private readonly float wingArea = 16.17f; //174f;
    private Vector3 airDynamicForces;
    private Vector3 velocitySmooth = Vector3.zero;

    private float aspectRatio
    {
        get
        {
            return Mathf.Pow(wingSpan, 2) / wingArea;
        }
    }

    public float AngleOfAttack
    {
        get
        {
            float degrees = rb.rotation.eulerAngles.x;
            if (degrees > 180)
            {
                return 360f - degrees;
            }
            else if (degrees < 180)
            {
                return -degrees;
            }

            return degrees;
        }
    }


    private float responseModifer
    {
        get
        {
            return (rb.mass / 10f) * responsivness;
        }
    }

    Rigidbody rb;
    private PlaneInput planeInput;
    [SerializeField] TextMeshProUGUI hud;

    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = Mathf.Epsilon;

        planeInput = new PlaneInput();
        planeInput.Plane.Enable();
    }
    private void HandleInputs()
    {
        var steeringInput = planeInput.Plane.Steering.ReadValue<Vector3>();
        roll = steeringInput.x;
        pitch = steeringInput.y;
        yaw = steeringInput.z;

        var thrustInput = planeInput.Plane.Thrust.ReadValue<float>();

        if (thrustInput > 0) throttle += throttleIncremnet;
        else if (thrustInput < 0) throttle -= throttleIncremnet;

        throttle = Mathf.Clamp(throttle, 0f, 100f);

    }

    private void UpdateHUD()
    {
        hud.text = "Throttle: " + throttle.ToString("F0") + " %\n"
            + "Airspeed: " + (rb.velocity.magnitude * 1.94384f).ToString("F0") + " knot/s\n"
            + "Altitude: " + (transform.position.y * 3.28084f).ToString("F0") + " Feet\n"
            + "AoA: " + AngleOfAttack.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInputs();
        UpdateHUD();
    }

    private void CalculateSteeringTorque()
    {
        var pitchTorque = Vector3.right * pitch;
        var rollTorque = -Vector3.forward * roll;
        var yawTorque = Vector3.up * yaw;

        var torque = (pitchTorque + rollTorque + yawTorque) * responseModifer * 30f;

        rb.AddRelativeTorque(torque);
    }

    //https://stackoverflow.com/questions/49716989/unity-aircraft-physics
    //uses approx.of lifting line theory
    private void calculateForces()
    {
        // α * 2 * PI * (AR / AR + 2)
        var inducedLift = AngleOfAttack * Mathf.Deg2Rad * (aspectRatio / (aspectRatio + 2f)) * 2f * Mathf.PI;

        // CL ^ 2 / (AR * PI)
        var inducedDrag = (inducedLift * inducedLift) / (aspectRatio * Mathf.PI);

        // V ^ 2 * R * 0.5 * A
        var pressure = rb.velocity.sqrMagnitude * 1.2754f * 0.5f * wingArea;

        var lift = Mathf.Clamp(inducedLift * pressure, 0 , 10000);

        var drag = Mathf.Clamp((0.021f + inducedDrag) * pressure, 0, float.MaxValue);

        // *flip sign(s) if necessary*
        var dragDirection = rb.velocity.normalized;
        var liftDirection = Vector3.Cross(dragDirection, transform.right);


        var liftForce =  lift * liftDirection;
        var dragForce = dragDirection * drag;

        rb.AddForce(liftForce - dragForce);
        var thrust = maxThrust * PowerCurve.Evaluate((throttle / 100));
        rb.AddForce(transform.forward * thrust);
    }

    private void FixedUpdate()
    {
        calculateForces();
        CalculateSteeringTorque();
    }
}
