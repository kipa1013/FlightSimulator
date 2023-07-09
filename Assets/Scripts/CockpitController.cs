using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CockpitController : MonoBehaviour
{
    public GameObject plane;
    public float pitch;
    public float roll;
    //public float yaw;
    public float knots;
    public float feet;
    public float heading;

    public GameObject attitudeIndBg;
    public GameObject attitudeIndBgOutRing;
    public GameObject airSpeedIndNeedle;
    public GameObject altShortNeedle;
    public GameObject altLongNeedle;
    public GameObject headingMarkings;

    /* Airspeed Indicator structures */
    private class AirspeedArea
    {
        public int from;
        public int to;
        public float angleFrom;
        public float angleTo;

        public AirspeedArea(int from, int to, float angleFrom, float angleTo)
        {
            this.from = from;
            this.to = to;
            this.angleFrom = angleFrom;
            this.angleTo = angleTo;
        }
    }
    private AirspeedArea[] airspeedAreas =
    {
        new AirspeedArea(0, 40, -90f, -120.7f),
        new AirspeedArea(40, 50, -120.7f, -140.6f),
        new AirspeedArea(50, 60, -140.6f, -162.8f),
        new AirspeedArea(60, 70, -162.8f, -184.4f),
        new AirspeedArea(70, 80, -184.4f, -206.5f),
        new AirspeedArea(80, 90, -206.5f, -229.4f),
        new AirspeedArea(90, 100, -229.4f, -252.9f),
        new AirspeedArea(100, 110, -252.9f, -274.8f),
        new AirspeedArea(110, 120, -274.8f, -295.1f),
        new AirspeedArea(120, 130, -295.1f, -311.1f),
        new AirspeedArea(130, 140, -311.1f, -326f),
        new AirspeedArea(140, 150, -326f, -341.5f),
        new AirspeedArea(150, 160, -341.5f, -356.1f),
        new AirspeedArea(160, 170, -356.1f, -368.9f),
        new AirspeedArea(170, 180, -368.9f, -380.8f),
        new AirspeedArea(180, 190, -380.8f, -394f),
        new AirspeedArea(190, 200, -394f, -407.9f),
    };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /* Attitude Indicator */
        roll = plane.transform.rotation.eulerAngles.z;
        attitudeIndBgOutRing.transform.localRotation = Quaternion.Euler(0f, roll + 180, 0f);

        pitch = plane.transform.rotation.eulerAngles.x;
        if (pitch >= 180f)
        {
            pitch = pitch - 360f;
        }

        const float baseVal = 0.15f;
        const float offsetPerDeg = 0.00385f;
        float yOffset = baseVal - pitch * offsetPerDeg;

        MeshRenderer renderer = attitudeIndBg.GetComponent<MeshRenderer>();
        Material material = renderer.material;
        Vector2 defaultYOffset = material.mainTextureOffset;
        material.mainTextureOffset = new Vector2(defaultYOffset.x, yOffset);

        /* Airspeed Indicator */
        Rigidbody rb = plane.GetComponent<Rigidbody>();
        knots = rb.velocity.magnitude * 1.94384f;
        float angle = -90f;
        foreach (AirspeedArea area in airspeedAreas)
        {
            if (area.from < knots && knots <= area.to)
            {
                angle = Mathf.Lerp(area.angleFrom, area.angleTo, (knots - area.from) / (area.to - area.from));
            }
        }
        airSpeedIndNeedle.transform.localRotation = Quaternion.Euler(0f, 0f, angle);

        /* Altimeter */
        feet = plane.transform.position.y * 3.28084f;
        int feetThousands = (int)(feet / 1000) * 1000;
        int feetHundreds = (int)(feet % 1000);
        float angleThousands;
        float angleHundreds;

        angleThousands = -90f - Mathf.Lerp(0f, 360f, feet / 12000f);
        angleHundreds = -90f - Mathf.Lerp(0f, 360f, feetHundreds / 1000f);

        altShortNeedle.transform.localRotation = Quaternion.Euler(0f, 0f, angleThousands);
        altLongNeedle.transform.localRotation = Quaternion.Euler(0f, 0f, angleHundreds);

        /* Heading Indicator */
        heading = -plane.transform.rotation.eulerAngles.y;
        headingMarkings.transform.localRotation = Quaternion.Euler(0f, 0f, -90f - heading);
    }
}
