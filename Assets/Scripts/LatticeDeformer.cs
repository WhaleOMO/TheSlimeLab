using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

public class LatticeDeformer : MonoBehaviour
{
    // The mesh to be deformed
    public Mesh targetMesh;
    public GameObject b0, b1, b2, b3, t0, t1, t2, t3;
    
    [Tooltip("Strength of spring")]
    public float spring = 100f;
    [Tooltip("Higher the value the faster the spring oscillation stops")]
    public float damper = 0.2f;
    [Header("Other Settings")]
    public float colliderSize = 0.35f;
    public float rigidbodyMass = 1f;

    private Vector3[] _initialVerts;
    // trilinear uvw value for each vertex
    // calculate once, used every frame to lerp vertex positions based on lattice points' positions...
    private Vector3[] _vertexUVWs;           
    
    [ContextMenu("InitializeCubeLattice")]
    private void InitCubeLattice()
    {
        var boundingBox = GetComponent<MeshRenderer>().bounds;
        var parent = gameObject;
        Vector3 minPos = boundingBox.min;
        Vector3 edge = 2.0f * boundingBox.extents;
        b0 = CreateLatticePoint(parent, minPos, "b0");
        b1 = CreateLatticePoint(parent, minPos + new Vector3(edge.x, 0, 0), "b1");
        b2 = CreateLatticePoint(parent, minPos + new Vector3(edge.x, 0, edge.z), "b2");
        b3 = CreateLatticePoint(parent, minPos + new Vector3(0, 0, edge.z), "b3");

        t0 = CreateLatticePoint(parent, minPos + new Vector3(0, edge.y, 0), "t0");
        t1 = CreateLatticePoint(parent, minPos + new Vector3(edge.x, edge.y, 0), "t1");
        t2 = CreateLatticePoint(parent, minPos + new Vector3(edge.x, edge.y, edge.z), "t2");
        t3 = CreateLatticePoint(parent, minPos + new Vector3(0, edge.y, edge.z), "t3");
    }
    
    private GameObject CreateLatticePoint(GameObject parent, Vector3 position, string n)
    {
        GameObject go = new GameObject(n)
        {
            transform =
            {
                position = position,
                parent = parent.transform
            },
                
            layer = LayerMask.NameToLayer("Bones")
        };
        
        return go;
    }
    

    private void OnEnable()
    {
        targetMesh = GetComponent<MeshFilter>().mesh;
    }

    private void Start()
    {
        Softbody.Init(Softbody.ColliderShape.Box, colliderSize, rigidbodyMass, spring, damper, RigidbodyConstraints.FreezeRotation);
        
        // Colliders
        Softbody.AddCollider(ref b0); Softbody.AddCollider(ref b1); Softbody.AddCollider(ref b2); Softbody.AddCollider(ref b3);
        Softbody.AddCollider(ref t0); Softbody.AddCollider(ref t1); Softbody.AddCollider(ref t2); Softbody.AddCollider(ref t3);
        
        // Bottom
        Softbody.AddSpring(ref b0, ref b1);
        Softbody.AddSpring(ref b1, ref b2);
        Softbody.AddSpring(ref b2, ref b3);
        Softbody.AddSpring(ref b3, ref b0);
        // Top
        Softbody.AddSpring(ref t0, ref t1);
        Softbody.AddSpring(ref t1, ref t2);
        Softbody.AddSpring(ref t2, ref t3);
        Softbody.AddSpring(ref t3, ref t0);
        // Side
        Softbody.AddSpring(ref b0, ref t0);
        Softbody.AddSpring(ref b1, ref t1);
        Softbody.AddSpring(ref b2, ref t2);
        Softbody.AddSpring(ref b3, ref t3);
        // Cross
        Softbody.AddSpring(ref b0, ref t2);
        Softbody.AddSpring(ref b1, ref t3);
        Softbody.AddSpring(ref b2, ref t0);
        Softbody.AddSpring(ref b3, ref t1);
        
        // Precalculate weight for each vertex (UVW)
        
        Bounds localBounds = targetMesh.bounds;
        Vector3 extents = localBounds.extents;
        // Get the center of the Bounds
        Vector3 center = localBounds.center;
        // Calculate the eight corners
        Vector3[] corners = new Vector3[8];
        corners[0] = center + new Vector3(-extents.x, -extents.y, -extents.z);      // bottom left
        corners[1] = center + new Vector3(extents.x, -extents.y, -extents.z);       // top right
        corners[2] = center + new Vector3(extents.x, -extents.y, extents.z);        // 
        corners[3] = center + new Vector3(-extents.x, -extents.y, extents.z);
        corners[4] = center + new Vector3(-extents.x, extents.y, -extents.z);
        corners[5] = center + new Vector3(extents.x, extents.y, -extents.z);
        corners[6] = center + new Vector3(extents.x, extents.y, extents.z);
        corners[7] = center + new Vector3(-extents.x, extents.y, extents.z);

        Vector3[] gridPoints =
        {
            corners[0], corners[1], corners[3], corners[2],
            corners[4], corners[5], corners[7], corners[6]
        };

        _initialVerts = targetMesh.vertices;
        _vertexUVWs = new Vector3[targetMesh.vertices.Length];
        for (int i = 0; i < _vertexUVWs.Length; i++)
        {
            _vertexUVWs[i] = CalculateTrilinearWeights(gridPoints, targetMesh.vertices[i]);
        }
    }

    private Vector3 CalculateTrilinearWeights(Vector3[] gridPoints, Vector3 targetPoint)
    {
        // First, calculate the normalized coordinates (u, v, w) within the grid.
        float u = (targetPoint.x - gridPoints[0].x) / (gridPoints[1].x - gridPoints[0].x);
        float v = (targetPoint.y - gridPoints[0].y) / (gridPoints[3].y - gridPoints[0].y);
        float w = (targetPoint.z - gridPoints[0].z) / (gridPoints[4].z - gridPoints[0].z);
        return new Vector3(u, v, w);
    }
    

    private Vector3[] GetDeformedVertMesh(Vector3[] latticePositions)
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            // Interpolate along the x-axis (bottom face).
            float bottomInterpolationA = Lerp(gridPoints[0].z, gridPoints[1].z, u);
            float bottomInterpolationB = Lerp(gridPoints[3].z, gridPoints[2].z, u);
            float bottomInterpolation = Lerp(bottomInterpolationA, bottomInterpolationB, v);

            // Interpolate along the x-axis (top face).
            float topInterpolationA = Lerp(gridPoints[4].z, gridPoints[5].z, u);
            float topInterpolationB = Lerp(gridPoints[7].z, gridPoints[6].z, u);
            float topInterpolation = Lerp(topInterpolationA, topInterpolationB, v);
        }
        
        // Trilinear Interpolation
        // 1. Bilinear Bottom xzPlane Pos P1
        
        // 2. Bilinear Top xzPlane Pos P2
        
        // 3. Lerp P1 and P2 with y
        
    }
    
    private void Update()
    {
        //Vector3[] latticePositions = new []{}
        //targetMesh.vertices = GetDeformedVertMesh();
    }
    
    private void OnDrawGizmosSelected()
    {
        Debug.DrawLine(b0.transform.position, b1.transform.position);
        Debug.DrawLine(b1.transform.position, b2.transform.position);
        Debug.DrawLine(b2.transform.position, b3.transform.position);
        Debug.DrawLine(b3.transform.position, b0.transform.position);
            
        Debug.DrawLine(t0.transform.position, t1.transform.position);
        Debug.DrawLine(t1.transform.position, t2.transform.position);
        Debug.DrawLine(t2.transform.position, t3.transform.position);
        Debug.DrawLine(t3.transform.position, t0.transform.position);
            
        Debug.DrawLine(b0.transform.position, t0.transform.position);
        Debug.DrawLine(b1.transform.position, t1.transform.position);
        Debug.DrawLine(b2.transform.position, t2.transform.position);
        Debug.DrawLine(b3.transform.position, t3.transform.position);
        
        Debug.DrawLine(b0.transform.position, t2.transform.position);
        Debug.DrawLine(b1.transform.position, t3.transform.position);
        Debug.DrawLine(b2.transform.position, t0.transform.position);
        Debug.DrawLine(b3.transform.position, t1.transform.position);
    }

}
