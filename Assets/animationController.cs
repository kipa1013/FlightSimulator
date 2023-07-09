using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void ToggleFlaps()
    {
        bool flaps = animator.GetBool("turnOn");
        animator.SetBool("turnOn", !flaps);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
