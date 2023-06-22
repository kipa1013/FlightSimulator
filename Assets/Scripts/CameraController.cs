using UnityEngine;



public class CameraController : MonoBehaviour
{
    [SerializeField]
    public Transform pov;
    public float speed;


    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, pov.position, Time.deltaTime * speed);
    }
}
