using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTrail : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject trial1, trial2, trial3;
    float radius = 2f, radspeed = 0.005f;
    float radCounter = 0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        radCounter += radspeed;
        trial1.transform.position = transform.position + new Vector3(radius * Mathf.Cos(radCounter), 0f, radius * Mathf.Sin(radCounter));
        trial2.transform.position = transform.position + new Vector3(radius * Mathf.Cos(radCounter + Mathf.Deg2Rad * 120f), 0f, radius * Mathf.Sin(radCounter + Mathf.Deg2Rad * 120f));
        trial3.transform.position = transform.position + new Vector3(radius * Mathf.Cos(radCounter + Mathf.Deg2Rad * 240f), 0f, radius * Mathf.Sin(radCounter + Mathf.Deg2Rad * 240f));

    }
}
