using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;

public class SlimeMoving : MonoBehaviour
{
    public GameObject boneRoot;     // The center bone
    public GameObject boneSet;
    public Rigidbody rb;

    public float h = 1;
    public float gravity = -18;
    public bool isGrounded;

    public float max = 60;
    public ArrayList bones = new ArrayList();
    float angleInTotal = 0f;
    Vector3 bx, by, bz;
    Vector3 bx2, by2, bz2;

    Vector3 rRoot, rx, rx2, ry, ry2, rz, rz2;
    
    // Start is called before the first frame update
    void Start()
    {
        bx = boneRoot.transform.position - gameObject.GetComponent<BoneSphere>().x.transform.position;
        bx2 = boneRoot.transform.position - gameObject.GetComponent<BoneSphere>().x2.transform.position;
        by = boneRoot.transform.position - gameObject.GetComponent<BoneSphere>().y.transform.position;
        by2 = boneRoot.transform.position - gameObject.GetComponent<BoneSphere>().y2.transform.position;
        bz = boneRoot.transform.position - gameObject.GetComponent<BoneSphere>().z.transform.position;
        bz2 = boneRoot.transform.position - gameObject.GetComponent<BoneSphere>().z2.transform.position;

        rRoot = gameObject.GetComponent<BoneSphere>().root.transform.rotation.eulerAngles;
        rx = gameObject.GetComponent<BoneSphere>().x.transform.rotation.eulerAngles;
        rx2 = gameObject.GetComponent<BoneSphere>().x2.transform.rotation.eulerAngles;
        ry = gameObject.GetComponent<BoneSphere>().y.transform.rotation.eulerAngles;
        ry2 = gameObject.GetComponent<BoneSphere>().y2.transform.rotation.eulerAngles;
        rz = gameObject.GetComponent<BoneSphere>().z.transform.rotation.eulerAngles;
        rz2 = gameObject.GetComponent<BoneSphere>().z2.transform.rotation.eulerAngles;

        rb = GetComponent<Rigidbody>();
        StartCoroutine(Wait());
    }
    IEnumerator Wait()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (isGrounded)
            {
                float offset = Random.Range(-max, max);
                float maxDis = 8.0f;
                int nSteps = 20;
                
                rb.freezeRotation = false;
                rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                
                // if there is a obstacle in front of the slime
                if (Physics.Raycast(rb.position, -transform.right, maxDis))
                {
                    offset = Random.Range(-135f,-180f);
                    nSteps = 80;
                }

                for (int i = 0; i < nSteps; ++i)
                {
                    float step = offset / nSteps;
                    angleInTotal += step;
                    
                    rb.transform.Rotate(Vector3.up, step);
                    
                    Vector3 nextAngle = boneSet.transform.localRotation.eulerAngles;
                    nextAngle.y += step/2;
                    boneSet.transform.localRotation = Quaternion.Euler(nextAngle);
                    
                    yield return new WaitForSeconds(0.01f);
                }
                rb.freezeRotation = true;
                yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
                Lanuch();
            }
        }
    }
    
    void Lanuch()
    {
        Physics.gravity = Vector3.up * gravity;
        rb.useGravity = true;
        rb.velocity = CalculateLanuchVelocity();
    }

    Vector3 CalculateLanuchVelocity()
    {
        
        Vector3 newpos = rb.position + transform.right * (-0.5f);

        float displacementY = newpos.y - rb.position.y;
        Vector3 displacementXZ = new Vector3(newpos.x - rb.position.x, 0, newpos.z - rb.position.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * h / gravity) * Mathf.Sqrt(2 * (displacementY - h) / gravity));

        return velocityXZ + velocityY;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void FixedUpdate()
    {
        if (!isGrounded)
        {
            SychronizeNodes(0.2f);
        }
        
    }
    private void SychronizeNodes(float bias)
    {
        // Debug.Log(angleInTotal);
        Matrix4x4 rotateMat = Matrix4x4.identity;
        rotateMat.m00 = Mathf.Cos(Mathf.Deg2Rad * angleInTotal);
        rotateMat.m02 = Mathf.Sin(Mathf.Deg2Rad * angleInTotal);
        rotateMat.m20 = -Mathf.Sin(Mathf.Deg2Rad * angleInTotal);
        rotateMat.m22 = Mathf.Cos(Mathf.Deg2Rad * angleInTotal);
        boneRoot.transform.position = rb.position + new Vector3(0, bias, 0);
        var boneSphere = gameObject.GetComponent<BoneSphere>();
        boneSphere.x.transform.position = boneRoot.transform.position - rotateMat.MultiplyPoint(bx);
        boneSphere.x2.transform.position = boneRoot.transform.position - rotateMat.MultiplyPoint(bx2);
        boneSphere.y.transform.position = boneRoot.transform.position - rotateMat.MultiplyPoint(by);
        boneSphere.y2.transform.position = boneRoot.transform.position - rotateMat.MultiplyPoint(by2);
        boneSphere.z.transform.position = boneRoot.transform.position - rotateMat.MultiplyPoint(bz);
        boneSphere.z2.transform.position = boneRoot.transform.position - rotateMat.MultiplyPoint(bz2);
        
        boneSphere.root.transform.rotation = Quaternion.Euler(new Vector3(rRoot.x, rRoot.y + angleInTotal, rRoot.z));
        boneSphere.x.transform.rotation = Quaternion.Euler(new Vector3(rx.x, rx.y + angleInTotal, rx.z));
        boneSphere.x2.transform.rotation = Quaternion.Euler(new Vector3(rx2.x, rx2.y + angleInTotal, rx2.z));
        boneSphere.y.transform.rotation = Quaternion.Euler(new Vector3(ry.x, ry.y + angleInTotal, ry.z));
        boneSphere.y2.transform.rotation = Quaternion.Euler(new Vector3(ry2.x, ry2.y + angleInTotal, ry2.z));
        boneSphere.z.transform.rotation = Quaternion.Euler(new Vector3(rz.x, rz.y + angleInTotal, rz.z));
        boneSphere.z2.transform.rotation = Quaternion.Euler(new Vector3(rz2.x, rz2.y + angleInTotal, rz2.z));

    }
}