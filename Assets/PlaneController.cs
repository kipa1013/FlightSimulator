using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlaneController : MonoBehaviour
{
    [Header("Plane Stats")]
    public float throttleIncremnet = 0.1f;
    public float maxThrust = 120000f;
    public AnimationCurve PowerCurve;
    public float responsivness = 10f;
    public float lift = 280f;
    public float speedDragFactor = 0.1f;

    private float _beginningDrag;
    private float _beginningAngularDrag;
    private float throttle;
    private float lastThrust = 0f;
    private float roll;
    private float pitch;
    private float yaw;

    private float responseModifer
    {
        get
        {
            return (rb.mass / 10f) * responsivness;
        }
    }

    Rigidbody rb;
    [SerializeField] TextMeshProUGUI hud;
    [SerializeField] private CockpitController cockpitController;

    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _beginningDrag = rb.drag;
        _beginningAngularDrag = rb.angularDrag;
    }

    private void SendPlaneStatsToCockpitController()
    {
        cockpitController.MaxThrust = maxThrust;
        cockpitController.Lift = lift;
        cockpitController.Throttle = throttle;
        cockpitController.LastThrust = lastThrust;
        cockpitController.Roll = roll;
        cockpitController.Pitch = pitch;
        cockpitController.Yaw = yaw;
        cockpitController.Velocity = rb.velocity;
        cockpitController.Position = transform.position;
    }

    private void HandleInputs()
    {
        roll = Input.GetAxis("Roll");
        pitch = Input.GetAxis("Pitch");
        yaw = Input.GetAxis("Yaw");

        if (Input.GetKey(KeyCode.Space)) throttle += throttleIncremnet;
        else if (Input.GetKey(KeyCode.LeftControl)) throttle -= throttleIncremnet;

        throttle = Mathf.Clamp(throttle, 0f, 100f);

    }

    private void UpdateHUD()
    {
        hud.text = "Throttle: " + throttle.ToString("F0") + " %\n"
            + "Airspeed: " + (rb.velocity.magnitude * 1.94384f).ToString("F0") + " knot/s\n"
            + "Altitude: " + (transform.position.y * 3.28084f).ToString("F0") + " Feet";
    }

    // Update is called once per frame
    void Update()
    {
        HandleInputs();
        UpdateHUD();
        SendPlaneStatsToCockpitController();
    }

    private void UpdateThrust()
    {

        var thrust = maxThrust * PowerCurve.Evaluate((throttle / 100));
        float finalThrust;
        if (thrust < lastThrust)
        {
            finalThrust = Mathf.Max(lastThrust - 400, thrust);
        }
        else
        {
            finalThrust = Mathf.Min(lastThrust + 500, thrust);
        }

        
        lastThrust = finalThrust;
        rb.AddForce(transform.forward * finalThrust);
    }
    private void CalculateDrag()
    {
        //speed drag

        Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
        var ForwardSpeed = Mathf.Max(0f, localVelocity.z);
        float speedDrag = ForwardSpeed * speedDragFactor;

        rb.drag = _beginningDrag + speedDrag;
        rb.angularDrag = _beginningAngularDrag * ForwardSpeed;
    }

    private void FixedUpdate()
    {
        UpdateThrust();
        CalculateDrag();
        rb.AddForce(Vector3.up * rb.velocity.magnitude * lift);
        rb.AddTorque(transform.up * yaw * responseModifer);
        rb.AddTorque(transform.right * pitch * responseModifer);
        rb.AddTorque(-transform.forward * roll * responseModifer);
    }
}
