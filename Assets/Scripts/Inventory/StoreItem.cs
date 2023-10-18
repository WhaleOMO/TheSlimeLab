using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace Inventory
{
    public class StoreItem : MonoBehaviour
    {
        public InputActionReference inputAction;

        private GameObject _targetObject;
        
        private void OnEnable()
        {
            inputAction.action.started += OnStoreItem;
        }

        private void OnDisable()
        {
            inputAction.action.started -= OnStoreItem;
        }

        private void Start()
        {
            var rayInteractor = GetComponent<XRRayInteractor>();
            rayInteractor.selectEntered.AddListener(OnSelectEntered);
            rayInteractor.selectExited.AddListener(OnSelectExited);
        }

        private void OnSelectEntered(SelectEnterEventArgs args)
        {
            // Try Get target item
            GameObject targetObject = args.interactableObject.transform.gameObject;
            if (targetObject.TryGetComponent(out ItemHolder itemHolder))
            {
                _targetObject = targetObject;
            }
            // Debug.Log("Grabbed!");
        }

        private void OnSelectExited(SelectExitEventArgs args)
        {
            _targetObject = null;
        }
        
        private void OnStoreItem(InputAction.CallbackContext ctx)
        {
            // Debug.Log("On Store Item");
            if (_targetObject != null)
            {
                InventoryManager.Instance.AddItem(_targetObject.GetComponent<ItemHolder>().item);
                Destroy(_targetObject);
            }
        }
    }
}
