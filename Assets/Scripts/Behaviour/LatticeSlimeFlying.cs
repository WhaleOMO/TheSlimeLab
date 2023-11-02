using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LatticeSlimeFlying : MonoBehaviour
{
    public GameObject[] controlPoints;
    private IEnumerator _floatingCoroutine;

    //public float degreesPerSecond = 15.0f;
    public float amplitude = 1f;
    public float frequency = 1f;
    
    // Position Storage Variables
    private Vector3[] posOffset = new Vector3[9];
    private Vector3[] tempPos = new Vector3[9];

    // Start is called before the first frame update
    void Start()
    {
        int i = 0;
        foreach (var controlPoint in controlPoints)
        {
            var rb = controlPoint.GetComponent<Rigidbody>();
            rb.useGravity = false;
            posOffset[i] = controlPoint.transform.position;
            i++;
        }

    }
    
    private void Update()
    {
        VerticalFloating();
    }
    
    private void VerticalFloating()
    {
        int i = 0;
        foreach (var controlPoint in controlPoints)
        {
            
            tempPos[i] = posOffset[i];
            tempPos[i].y += Mathf.Sin (Time.fixedTime * Mathf.PI * frequency) * amplitude;

            controlPoint.transform.position = tempPos[i];  
            i++;
        }
    }
    

}
