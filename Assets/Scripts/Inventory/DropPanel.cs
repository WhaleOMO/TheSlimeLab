using System;
using UnityEngine;

namespace Inventory
{
    public enum DropAction
    {
        Remove,
        Take
    }
    
    public class DropPanel: MonoBehaviour
    {
        public DropAction actionOnDrop;

        private InventoryUI uiRoot;
        
        public void OnEnable()
        {
            if (transform.parent.TryGetComponent<InventoryUI>(out InventoryUI inventoryUI))
            {
                uiRoot = inventoryUI;
                uiRoot.OnItemBeginDrag += SetVisible;
                uiRoot.OnItemEndDrag += SetInvisible;
            }
            SetInvisible();
        }

        private void OnDisable()
        {
            if (uiRoot)
            {
                uiRoot.OnItemBeginDrag -= SetVisible;
                uiRoot.OnItemEndDrag -= SetInvisible;
            }
        }

        public void SetVisible()
        {
            var canvasGroup = this.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }

        public void SetInvisible()
        {
            var canvasGroup = this.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
        }
        
        public void DoDrop(Item item, GameObject tempObject, float objectScaleFactor)
        {
            if (actionOnDrop == DropAction.Remove)
            {
                Destroy(tempObject);
                InventoryManager.Instance.RemoveItem(item);
                return;
            }

            if (actionOnDrop == DropAction.Take)
            {
                // Try Enable Gravity
                if (tempObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
                {
                    rb.useGravity = true;
                }
                tempObject.transform.localScale /= objectScaleFactor;
                InventoryManager.Instance.RemoveItem(item);
            }
        }
    }
}