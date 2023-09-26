using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class LatticeSlimeMoving : MonoBehaviour
{
    public Rigidbody rb;

    public float h = 1;
    public float gravity = -18;
    public bool isGrounded;

    public int nSteps = 20;
    public float max = 45;


    public GameObject groundChecker;

    public GameObject[] controlPoints;

    private IEnumerator _movingCoroutine;

    private void OnEnable()
    {
        _movingCoroutine = Wait();
        StartCoroutine(_movingCoroutine);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
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
    
    IEnumerator Wait()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            isGrounded = Mathf.Abs(groundChecker.GetComponent<Rigidbody>().velocity.y) < 0.1f;
            if (isGrounded)
            {
                float offset = Mathf.Max(20f, Random.Range(0, max));
                
                ToggleControlPointsRbKinematic(true);
                
                for (int i = 0; i < nSteps; ++i)
                {
                    float step = offset / nSteps;
                    
                    RotateStep(Vector3.up, step);
                    
                    yield return new WaitForSeconds(0.01f);
                }
                
                ToggleControlPointsRbKinematic(false);
                
                yield return new WaitForSeconds(Random.Range(1.5f,2f));
                Lanuch();
            }
        }
    }

    void RotateStep(Vector3 up, float offset)
    {
        foreach (var controlPoint in controlPoints)
        {
            controlPoint.transform.Rotate(up, offset);
        }
    }


    void Lanuch()
    {
        Physics.gravity = Vector3.up * gravity;
        foreach (var controlPoint in controlPoints)
        {
            var rb = controlPoint.GetComponent<Rigidbody>();
            rb.velocity = CalculateLanuchVelocity(rb);
            //print(CalculateLanuchVelocity());
        }
    }

    Vector3 CalculateLanuchVelocity(Rigidbody rb)
    {
        Vector3 newpos = rb.position + rb.transform.right * (-2f);

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
