using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

public class LatticeDeformer : MonoBehaviour
{
    // The mesh to be deformed
    public Mesh targetMesh;
    public GameObject grabPoint;
    public GameObject b0, b1, b2, b3, t0, t1, t2, t3;
    
    [Tooltip("Strength of spring")]
    public float spring = 100f;
    [Tooltip("Higher the value the faster the spring oscillation stops")]
    public float damper = 0.2f;
    [Header("Other Settings")]
    public float colliderSize = 0.35f;
    public float rigidbodyMass = 1f;

    private Vector3[] _initialVerts;
    private Vector3[] _gridPoints;
    // trilinear uvw value for each vertex
    // calculate once, used every frame to lerp vertex positions based on lattice points' positions...
    private Vector3[] _vertexUVWs;

    private Mesh _deformedMesh;
    
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

    private void Start()
    {
        Softbody.Init(Softbody.ColliderShape.Box, colliderSize, rigidbodyMass, spring, damper, RigidbodyConstraints.FreezeRotation);
        
        // Colliders
        /*
        Softbody.AddCollider(ref b0); Softbody.AddCollider(ref b1); Softbody.AddCollider(ref b2); Softbody.AddCollider(ref b3);
        Softbody.AddCollider(ref t0); Softbody.AddCollider(ref t1); Softbody.AddCollider(ref t2); Softbody.AddCollider(ref t3);
        */
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
        // GrabPoint
        Softbody.AddSpring(ref grabPoint, ref b0);
        Softbody.AddSpring(ref grabPoint, ref b1);
        Softbody.AddSpring(ref grabPoint, ref b2);
        Softbody.AddSpring(ref grabPoint, ref b3);
        Softbody.AddSpring(ref grabPoint, ref t0);
        Softbody.AddSpring(ref grabPoint, ref t1);
        Softbody.AddSpring(ref grabPoint, ref t2);
        Softbody.AddSpring(ref grabPoint, ref t3);
        
        // Precalculate weight for each vertex (UVW)
        Bounds localBounds = targetMesh.bounds;
        Vector3 extents = localBounds.extents;
        // Get the center of the Bounds
        Vector3 center = localBounds.center;
        // Calculate the eight corners
        _gridPoints = new Vector3[8];
        _gridPoints[0] = center + new Vector3(-extents.x, -extents.y, -extents.z);
        _gridPoints[1] = center + new Vector3(extents.x, -extents.y, -extents.z);
        _gridPoints[2] = center + new Vector3(extents.x, -extents.y, extents.z);
        _gridPoints[3] = center + new Vector3(-extents.x, -extents.y, extents.z);
        _gridPoints[4] = center + new Vector3(-extents.x, extents.y, -extents.z);
        _gridPoints[5] = center + new Vector3(extents.x, extents.y, -extents.z);
        _gridPoints[6] = center + new Vector3(extents.x, extents.y, extents.z);
        _gridPoints[7] = center + new Vector3(-extents.x, extents.y, extents.z);

        _initialVerts = targetMesh.vertices;
        _vertexUVWs = new Vector3[targetMesh.vertices.Length];
        for (int i = 0; i < _vertexUVWs.Length; i++)
        {
            _vertexUVWs[i] = CalculateTrilinearWeights(_gridPoints, targetMesh.vertices[i]);
        }
        
        if (_deformedMesh==null)
        {
            _deformedMesh = new Mesh()
            {
                name = "DeformedSlimeMeshInstance",
                vertices = _initialVerts,
                uv = targetMesh.uv,
                normals = targetMesh.normals,
                tangents = targetMesh.tangents,
                triangles = targetMesh.triangles,
                bounds = targetMesh.bounds
            };
        }

        GetComponent<MeshFilter>().mesh = _deformedMesh;
    }

    private Vector3 CalculateTrilinearWeights(Vector3[] gridPoints, Vector3 targetPoint)
    {
        // First, calculate the normalized coordinates (u, v, w) within the grid.
        float u = (targetPoint.x - gridPoints[0].x) / (gridPoints[1].x - gridPoints[0].x);
        float v = (targetPoint.y - gridPoints[0].y) / (gridPoints[4].y - gridPoints[0].y);
        float w = (targetPoint.z - gridPoints[0].z) / (gridPoints[6].z - gridPoints[0].z);
        return new Vector3(u, v, w);
    }
    
    // Code From https://forum.unity.com/threads/how-to-recalculate-normals-so-they-appear-smoother-on-chunked-meshes.898649/
    private void RecalculateNormalsSeamless(Mesh mesh) {
        var trianglesOriginal = mesh.triangles;
        var triangles = trianglesOriginal.ToArray();
   
        var vertices = mesh.vertices;
   
        var mergeIndices = new Dictionary<int, int>();
 
        for (int i = 0; i < vertices.Length; i++) {
            var vertexHash = vertices[i].GetHashCode();                  
       
            if (mergeIndices.TryGetValue(vertexHash, out var index)) {
                for (int j = 0; j < triangles.Length; j++)
                    if (triangles[j] == i)
                        triangles[j] = index;
            } else
                mergeIndices.Add(vertexHash, i);
        }
 
        mesh.triangles = triangles;
   
        var normals = new Vector3[vertices.Length];
   
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        var newNormals = mesh.normals;
   
        for (int i = 0; i < vertices.Length; i++)
            if (mergeIndices.TryGetValue(vertices[i].GetHashCode(), out var index))
                normals[i] = newNormals[index];
 
        mesh.triangles = trianglesOriginal;
        mesh.normals = normals;
    }
    
    private void DeformMesh(Vector3[] latticePositions)
    {
        Vector3[] deformedVerts = new Vector3[_initialVerts.Length];
        for (int i = 0; i < _initialVerts.Length; i++)
        {
            // Interpolate along the x-axis (bottom face).
            Vector3 bottomInterpolationA = Vector3.Lerp(latticePositions[0], latticePositions[1], _vertexUVWs[i].x);
            Vector3 bottomInterpolationB = Vector3.Lerp(latticePositions[3], latticePositions[2], _vertexUVWs[i].x);
            Vector3 bottomInterpolation = Vector3.Lerp(bottomInterpolationA, bottomInterpolationB, _vertexUVWs[i].z);

            // Interpolate along the x-axis (top face).
            Vector3 topInterpolationA = Vector3.Lerp(latticePositions[4], latticePositions[5], _vertexUVWs[i].x);
            Vector3 topInterpolationB = Vector3.Lerp(latticePositions[7], latticePositions[6], _vertexUVWs[i].x);
            Vector3 topInterpolation = Vector3.Lerp(topInterpolationA, topInterpolationB, _vertexUVWs[i].z);
            
            // Interpolate along the z-axis (between bottom and top faces).
            Vector3 resultWorldSpace = Vector3.Lerp(bottomInterpolation, topInterpolation, _vertexUVWs[i].y);
            Vector3 resultLocalSpace = transform.InverseTransformPoint(resultWorldSpace);
            deformedVerts[i] = resultLocalSpace;
        }

        _deformedMesh.vertices = deformedVerts;
        RecalculateNormalsSeamless(_deformedMesh);
        // Recalculate actual mesh bounding box otherwise might be culled by renderer
        _deformedMesh.RecalculateBounds();
    }
    
    private void FixedUpdate()
    {
        Vector3[] latticePositions = new[]
        {
            b0.transform.position, b1.transform.position, b2.transform.position, b3.transform.position,
            t0.transform.position, t1.transform.position, t2.transform.position, t3.transform.position
        };
        
        DeformMesh(latticePositions);
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
