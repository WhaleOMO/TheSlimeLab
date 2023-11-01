using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LatticeSlimeFlying : MonoBehaviour
{
    public GameObject[] controlPoints;
    private IEnumerator _floatingCoroutine;
    
    public float degreesPerSecond = 15.0f;
    public float amplitude = 0.5f;
    public float frequency = 1f;
    
    // Position Storage Variables
    private Vector3[] posOffset = new Vector3[8];
    private Vector3[] tempPos = new Vector3[8];
    
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

   void Update()
    {
        VerticalFloating();
    }

   // ReSharper disable Unity.PerformanceAnalysis
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
