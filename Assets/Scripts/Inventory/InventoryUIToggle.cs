using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace Inventory
{
    public class InventoryUIToggle : MonoBehaviour
    {
        public InputActionReference inputAction;
        public Canvas uiCanvas;
    
        private bool _isToggled;
    
        private void OnEnable()
        {
            inputAction.action.started += OnToggle;
        }

        private void OnDisable()
        {
            inputAction.action.started -= OnToggle;
        }

        private void OnToggle(InputAction.CallbackContext ctx)
        {
            _isToggled = !_isToggled;
            uiCanvas.gameObject.SetActive(_isToggled);
            // Disable Ray Interactor When showing UI 
            GetComponent<XRRayInteractor>().enabled = !_isToggled;
        }
    }
}
