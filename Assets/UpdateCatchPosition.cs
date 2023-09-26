using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateCatchPosition : MonoBehaviour
{
    public GameObject[] controlPoints;
    public GameObject catchPoint;


    private Vector3 catchPosition;
    public static bool isGrab = false;


    // Start is called before the first frame update
    void Start()
    {
        catchPosition = catchPoint.GetComponent<Rigidbody>().position;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (isGrab)
        {
            Vector3 newPosition = catchPoint.GetComponent<Rigidbody>().position;
            foreach (var controlPoint in controlPoints)
            {
                var rb = controlPoint.GetComponent<Rigidbody>();
                rb.velocity = CalculateUpdateVelocity(catchPosition, newPosition);
                //print(CalculateLanuchVelocity());
            }
        }
        catchPosition = catchPoint.GetComponent<Rigidbody>().position;


    }

    Vector3 CalculateUpdateVelocity(Vector3 oldPosition, Vector3 NewPosition)
    {
        Vector3 displacement = NewPosition - oldPosition;
        Vector3 velocity = displacement / Time.deltaTime;
        return velocity;
    }

    public static void OnGrab()
    {
        isGrab = !isGrab;
        print("isgrab");
    }

}
