using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class LatticeSlimeMoving : MonoBehaviour
{
    public float h = 1;
    public float gravity = -18;
    public bool isGrounded;

    public int nSteps = 20;
    public float max = 45;
    public float creepForce = 100;
    
    public GameObject groundChecker;

    public GameObject[] controlPoints;

    public UnityEvent onCreepEvent;
    public UnityEvent onJumpEvent;

    private IEnumerator _movingCoroutine;

    private void OnEnable()
    {
        _movingCoroutine = Movement();
        StartCoroutine(_movingCoroutine);
    }

    void ToggleControlPointsRbKinematic(bool isKinematic)
    {
        foreach (var controlPoint in controlPoints)
        {
            var rb = controlPoint.GetComponent<Rigidbody>();
            if (isKinematic)
            {
                rb.angularDrag *= 0.5f;
            }
            else
            {
                rb.angularDrag *= 2f;
            }
            rb.freezeRotation = true;
        }
    }

    void RotateStep(Vector3 up, float offset)
    {
        foreach (var controlPoint in controlPoints)
        {
            controlPoint.transform.Rotate(up, offset);
        }
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    IEnumerator Movement()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            isGrounded = Mathf.Abs(groundChecker.GetComponent<Rigidbody>().velocity.y) < 0.1f;
            if (isGrounded)
            {
                float offset = Mathf.Max(20f, Random.Range(0, max));
                //SlimeSound.instance.PlayJumpSound(_landClip); 
                ToggleControlPointsRbKinematic(true);

                for (int i = 0; i < nSteps; ++i)
                {
                    float step = offset / nSteps;

                    RotateStep(Vector3.up, step);

                    yield return new WaitForSeconds(0.01f);
                }

                ToggleControlPointsRbKinematic(false);

                // Decide whether to launch or creep  
                bool b = Random.value < 0.5f;
                // print(b);
                
                if (b)
                {
                    // creep for 4 times
                    var rb0 = controlPoints[0].GetComponent<Rigidbody>();
                    var rb3 = controlPoints[3].GetComponent<Rigidbody>();
                    var rb4 = controlPoints[4].GetComponent<Rigidbody>();
                    var rb7 = controlPoints[7].GetComponent<Rigidbody>();
                    for (int i = 0; i < 4; i++)
                    {
                        onCreepEvent?.Invoke();
                        //rb0.AddForce((-1) * controlPoints[0].transform.right * creepForce, ForceMode.Acceleration);
                        //rb3.AddForce((-1) * controlPoints[3].transform.right * creepForce, ForceMode.Acceleration);
                        rb4.AddForce(controlPoints[4].transform.right * ((-1) * creepForce), ForceMode.Acceleration);
                        rb7.AddForce(controlPoints[7].transform.right * ((-1) * creepForce), ForceMode.Acceleration);
                        yield return new WaitForSeconds(1f);
                    }
                }
                else
                {
                    yield return new WaitForSeconds(Random.Range(0.5f, 2f));
                    Launch();
                    onJumpEvent?.Invoke();
                }
            }
        }

    }

    void Launch()
    {
        Physics.gravity = Vector3.up * gravity;
        foreach (var controlPoint in controlPoints)
        {
            var rb = controlPoint.GetComponent<Rigidbody>();
            rb.velocity = CalculateLaunchVelocity(rb);
        }
    }

    Vector3 CalculateLaunchVelocity(Rigidbody rb)
    {
        Vector3 newpos = rb.position + rb.transform.right * (-1.5f);

        float displacementY = newpos.y - rb.position.y;
        Vector3 displacementXZ = new Vector3(newpos.x - rb.position.x, 0, newpos.z - rb.position.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * h / gravity) * Mathf.Sqrt(2 * (displacementY - h) / gravity));

        return velocityXZ + velocityY;
    }

    private void OnDisable()
    {
        StopCoroutine(_movingCoroutine);
    }
}
