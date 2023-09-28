using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class UpdateCatchPosition : MonoBehaviour
{
    public GameObject[] controlPoints;
    public GameObject catchPoint;
    public MergeManager mergeManager;

    public XRRayInteractor leftRayInteractor;
    public XRRayInteractor rightRayInteractor;
    
    private Vector3 _catchPosition;
    private LatticeSlimeMoving _moving;
    private bool _isGrab = false;
    
    private void OnEnable()
    {
        leftRayInteractor.selectEntered.AddListener(OnGrabEnter);
        rightRayInteractor.selectEntered.AddListener(OnGrabEnter);
        leftRayInteractor.selectExited.AddListener(OnGrabExit);
        rightRayInteractor.selectExited.AddListener(OnGrabExit);
        _moving = gameObject.GetComponent<LatticeSlimeMoving>();
    }

    void Start()
    {
        _catchPosition = catchPoint.GetComponent<Rigidbody>().position;
        mergeManager = GameObject.Find("MergeManager").GetComponent<MergeManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_isGrab)
        {
            Vector3 viewDir = -Camera.main.transform.forward;
            
            Vector3 newPosition = catchPoint.GetComponent<Rigidbody>().position;
            foreach (var controlPoint in controlPoints)
            {
                var rb = controlPoint.GetComponent<Rigidbody>();
                Vector3 faceDir = -rb.transform.right;
                float angle = Mathf.Acos(Vector3.Dot(viewDir, faceDir));
                rb.velocity = CalculateUpdateVelocity(_catchPosition, newPosition);
                rb.transform.Rotate(controlPoint.transform.up, angle);
                //print(CalculateLanuchVelocity());
            }
        }
        _catchPosition = catchPoint.GetComponent<Rigidbody>().position;
    }

    Vector3 CalculateUpdateVelocity(Vector3 oldPosition, Vector3 newPosition)
    {
        Vector3 displacement = newPosition - oldPosition;
        Vector3 velocity = displacement / Time.fixedDeltaTime;
        return velocity;
    }
    
    private void OnGrabEnter(SelectEnterEventArgs args)
    {
        if (!args.interactableObject.transform.gameObject.Equals(catchPoint.gameObject))
        {
            return;
        }
        mergeManager.AddSlime(gameObject.transform.parent.gameObject, catchPoint);
        _moving.enabled = false;
        _isGrab = true;
    }
    
    private void OnGrabExit(SelectExitEventArgs args)
    {
        if (!args.interactableObject.transform.gameObject.Equals(catchPoint.gameObject))
        {
            return;
        }
        mergeManager.ReleaseSlime(gameObject.transform.parent.gameObject, catchPoint);
        _moving.enabled = true;
        _isGrab = false;
    }
    
    private void OnDisable()
    {

    }
}
