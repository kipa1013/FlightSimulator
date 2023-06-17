using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CockpitController : MonoBehaviour
{
    public GameObject bg;
    public GameObject bgOuterRing;
    public GameObject plane;
    public float pitch;
    public float roll;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        roll = plane.transform.rotation.eulerAngles.z;
        bgOuterRing.transform.localRotation = Quaternion.Euler(0f, roll + 180, 0f);

        //Mesh mesh = bg.GetComponent<MeshFilter>().mesh;
        //Vector2[] uv = mesh.uv;
        //string uvs = "";
        //foreach (Vector2 vector in uv)
        //{
        //    uvs += vector.ToString() + " ";
        //}
        //Debug.Log(uvs);

        const float baseVal = 0.15f;
        const float offsetPerDeg = 0.00385f;
        float yOffset = baseVal + pitch * offsetPerDeg;

        MeshRenderer renderer = bg.GetComponent<MeshRenderer>();
        Material material = renderer.material;
        Vector2 defaultYOffset = material.mainTextureOffset;
        material.mainTextureOffset = new Vector2(defaultYOffset.x, yOffset);
    }
}
