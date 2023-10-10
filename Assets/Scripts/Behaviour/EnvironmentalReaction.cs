using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnvironmentalReaction : MonoBehaviour
{
    public GameObject catchPoint;
    public UnityEvent onImpactEvent;
    private float variation;
    private float previous;
    public float threshold = -200f;
    private Rigidbody _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = catchPoint.GetComponent<Rigidbody>();
    }
    
    void FixedUpdate()
    {
        variation = (_rigidbody.velocity.magnitude - previous) / Time.fixedDeltaTime;
        previous = _rigidbody.velocity.magnitude;
        
        if (variation < threshold)
        {
            //print("INVOKE!!");
            onImpactEvent?.Invoke();
        }
        
    }
        
}