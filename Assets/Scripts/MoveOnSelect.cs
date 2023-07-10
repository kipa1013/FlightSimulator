using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnSelect : MonoBehaviour
{
    [SerializeField] private float X;
    [SerializeField]
    private float Y;
    [SerializeField]
    private float Z;
    [SerializeField][Range(0,1)]//TODO: Look for discrete Values here 
    private float Speed = 0.1f;

    private Vector3 toMove;
    private Vector3 target;

    private bool move = false;
    private bool ret;
    
    public bool ToggleBool;
    // Start is called before the first fr ame update
    void Start()
    {
        target = new Vector3(X, Y, Z);
    }

    public void Move()
    {
        toMove = (this.transform.localPosition - target) * Speed;
        //move = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (move)
        {
            transform.localPosition = toMove * Time.deltaTime;
        }

        if (Vector3.Distance(this.transform.localPosition, target) < 0.01)
        {
            move = false;
        }
        
    }

    private void FixedUpdate()
    {
        float deltaZ = 0.1f;
        if (ToggleBool)
        {
            deltaZ = -deltaZ;
        }
        var lowerGuard = Mathf.Clamp(-0.1f - transform.localPosition.z, -0.1f, 0);
        var upperGuard = -transform.localPosition.z;

        
        var zTranslation = Mathf.Clamp(0.01f, lowerGuard, upperGuard);
        Vector3 translation = new Vector3 { x = 0, y = 0, z = zTranslation };              
        transform.Translate(translation, Space.Self);
    }

    public void Toggle()
    {
        ToggleBool = !ToggleBool;
    }
}
