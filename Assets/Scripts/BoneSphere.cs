using System;
using UnityEngine;

public class BoneSphere : MonoBehaviour
{

    [Header("Bones")]
    public GameObject root = null;
    public GameObject x = null;
    public GameObject x2 = null;
    public GameObject y = null;
    public GameObject y2 = null;
    public GameObject z = null;
    public GameObject z2 = null;
    [Header("Spring Joint Settings")]
    [Tooltip("Strength of spring")]
    public float Spring = 100f;
    [Tooltip("Higher the value the faster the spring oscillation stops")]
    public float Damper = 0.2f;
    [Header("Other Settings")]
    public Softbody.ColliderShape Shape = Softbody.ColliderShape.Box;
    public float ColliderSize = 0.0035f;
    public float RigidbodyMass = 1f;
    public float CenterBoneMass = 10f;

    public bool debugBones;
    
    private void Start()
    {
        Softbody.Init(Shape, ColliderSize, RigidbodyMass, Spring, Damper, RigidbodyConstraints.FreezeRotation);

        Softbody.AddCollider(ref root, Shape, ColliderSize, CenterBoneMass);
        Softbody.AddCollider(ref x);
        Softbody.AddCollider(ref x2);
        Softbody.AddCollider(ref y);
        Softbody.AddCollider(ref y2);
        Softbody.AddCollider(ref z);
        Softbody.AddCollider(ref z2);

        Softbody.AddSpring(ref x, ref root);
        Softbody.AddSpring(ref x2, ref root);
        Softbody.AddSpring(ref y, ref root);
        Softbody.AddSpring(ref y2, ref root);
        Softbody.AddSpring(ref z, ref root);
        Softbody.AddSpring(ref z2, ref root);
    }

    private void OnDrawGizmos()
    {
        if (!debugBones)
        {
            return;   
        }
        // Visualize "Bone-spring" connections
        Vector3 boneCenter = root.transform.position;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(boneCenter, x.transform.position);
        Gizmos.DrawLine(boneCenter, x2.transform.position);
        Gizmos.DrawLine(boneCenter, y.transform.position);
        Gizmos.DrawLine(boneCenter, y2.transform.position);
        Gizmos.DrawLine(boneCenter, z.transform.position);
        Gizmos.DrawLine(boneCenter, z2.transform.position);
    }
}