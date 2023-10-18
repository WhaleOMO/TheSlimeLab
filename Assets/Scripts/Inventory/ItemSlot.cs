using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace Inventory
{
    public class ItemSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Item item;

        private InventoryUI uiRoot;
        private GameObject tempObject;
        private float scaleFactor = 0.35f;

        private XRRayInteractor rightRayInteractor;
        private bool isDragging;

        private void OnEnable()
        {
            rightRayInteractor = GameObject.Find("Right Controller").GetComponent<XRRayInteractor>();
            //rightRayInteractor.uiHoverExited.AddListener(ExitUI);
        }

        public void SetUIRoot(InventoryUI ui) => uiRoot = ui;

        public void OnBeginDrag(PointerEventData eventData)
        {
            isDragging = true;
            
            if (item == null)
            {
                // Cancel Dragging
                eventData.pointerDrag = null;
                return;
            }
            
            tempObject = Instantiate(item.prefab, null);
            // Try setting color
            if (tempObject.TryGetComponent<MeshRenderer>(out MeshRenderer renderer))
            {
                var tempMaterial = new Material(renderer.sharedMaterial);
                tempMaterial.color = item.color;
                renderer.sharedMaterial = tempMaterial;
            }
            
            // Try Disable Collider
            if (tempObject.TryGetComponent<Collider>(out Collider col))
            {
                col.enabled = false;
            }
            
            // Try Disable Gravity
            if (tempObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.useGravity = false;
            }
            tempObject.transform.position = eventData.pointerCurrentRaycast.worldPosition;
            tempObject.transform.localScale *= scaleFactor;
            
            // Fire drag event on UI root
            uiRoot?.OnItemBeginDrag?.Invoke();
        }

        public void OnDrag(PointerEventData eventData)
        { 
            tempObject.transform.position = eventData.pointerCurrentRaycast.worldPosition;
            // Not working
            if (eventData.fullyExited)
            {
                Destroy(tempObject);
                // Cancel Dragging
                eventData.pointerDrag = null;
                uiRoot?.OnItemEndDrag?.Invoke();
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDragging = false;
            
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raycastResults);
            // Fire end drag event on UI root 
            uiRoot?.OnItemEndDrag?.Invoke();
            // Find Drop Panel and trigger drop action
            if (raycastResults.Count > 0)
            {
                foreach (var result in raycastResults)
                {
                    if (result.gameObject.TryGetComponent<DropPanel>(out DropPanel panel))
                    {
                        panel.DoDrop(item, tempObject, scaleFactor);
                        return;
                    }        
                }
                
                Destroy(tempObject);
            }
        }

        public void ExitUI(UIHoverEventArgs args)
        {
            Destroy(tempObject);
            // Cancel Dragging
            uiRoot?.OnItemEndDrag?.Invoke();
        }
        
        private void Update()
        {
            
        }

        private bool IsPointerOverCanvas()
        {
            TrackedDeviceGraphicRaycaster raycaster = uiRoot.GetComponent<TrackedDeviceGraphicRaycaster>();
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerData, results);

            if (results.Count > 0)
            {
                return true;
            }

            return false;
        }
    }
}
