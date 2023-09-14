using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;

public class SlimeMoving : MonoBehaviour
{
    public GameObject boneRoot;
    public Rigidbody rb;

    public float h = 1;
    public float gravity = -18;
    public bool isGrounded;

    public int nSteps = 20;
    public float max = 45;
    public ArrayList bones = new ArrayList();
    Vector3 bx, by, bz;
    Vector3 bx2, by2, bz2;
    // Start is called before the first frame update
    void Start()
    {
        bx = boneRoot.transform.position - gameObject.GetComponent<BoneSphere>().x.transform.position;
        bx2 = boneRoot.transform.position - gameObject.GetComponent<BoneSphere>().x2.transform.position;
        by = boneRoot.transform.position - gameObject.GetComponent<BoneSphere>().y.transform.position;
        by2 = boneRoot.transform.position - gameObject.GetComponent<BoneSphere>().y2.transform.position;
        bz = boneRoot.transform.position - gameObject.GetComponent<BoneSphere>().z.transform.position;
        bz2 = boneRoot.transform.position - gameObject.GetComponent<BoneSphere>().z2.transform.position;
        rb = GetComponent<Rigidbody>();
        StartCoroutine(Wait());
        //rb.useGravity = false;
    }
    IEnumerator Wait()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (isGrounded)
            {
                float offset = Random.Range(-max, max);

                rb.freezeRotation = false;
                rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                
                for (int i = 0; i < nSteps; ++i)
                {
                    Quaternion newQuaternion = new Quaternion();
                    newQuaternion.Set(rb.rotation.x, rb.rotation.y + offset/nSteps, rb.rotation.z, 1);
                    newQuaternion = newQuaternion.normalized;

                    RotateFixedDir(offset, nSteps, newQuaternion);
                    yield return new WaitForSeconds(0.01f);
                }

                rb.freezeRotation = true;
                yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
                Lanuch();
            }
        }
    }

    void RotateFixedDir(float offset, int numSteps, Quaternion targetQuaternion)
    {
        float step = offset/numSteps;
        rb.rotation = Quaternion.RotateTowards(rb.rotation, targetQuaternion, step);
    }


    void Lanuch()
    {
        Physics.gravity = Vector3.up * gravity;
        rb.useGravity = true;
        rb.velocity = CalculateLanuchVelocity();
    }

    Vector3 CalculateLanuchVelocity()
    {
        //Vector3 offset = new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));
        
        // Vector3 newpos = rb.position + offset;
        
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
            // TODO: Control bones when on air
            boneRoot.transform.position = rb.position + new Vector3(0,0.2f,0);
            gameObject.GetComponent<BoneSphere>().x.transform.position = boneRoot.transform.position - bx;
            gameObject.GetComponent<BoneSphere>().x2.transform.position = boneRoot.transform.position - bx2;
            gameObject.GetComponent<BoneSphere>().y.transform.position = boneRoot.transform.position - by;
            gameObject.GetComponent<BoneSphere>().y2.transform.position = boneRoot.transform.position - by2;
            gameObject.GetComponent<BoneSphere>().z.transform.position = boneRoot.transform.position - bz;
            gameObject.GetComponent<BoneSphere>().z2.transform.position = boneRoot.transform.position - bz2;
        }
        
        // boneRoot.transform.rotation = rb.rotation;
    }
}