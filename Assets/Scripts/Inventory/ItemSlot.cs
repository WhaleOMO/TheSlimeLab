using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory
{
    public class ItemSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Item item;

        private InventoryUI uiRoot;
        private GameObject tempObject;
        private float scaleFactor = 0.35f;

        public void SetUIRoot(InventoryUI ui) => uiRoot = ui;

        public void OnBeginDrag(PointerEventData eventData)
        {
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
    }
}
