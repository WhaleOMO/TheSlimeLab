using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class InventoryUI: MonoBehaviour
    {
        public Transform itemSlotContainer;        // Ref to the UI container (with GridLayoutGroup)
        public GameObject itemSlotPrefab;          // Prefab for the item slot UI

        private void OnEnable()
        {
            // Register as Listener to Inventory update event
            InventoryManager.Instance.OnInventoryUpdate += UpdateUI;
        }

        private void OnDisable()
        {
            // Deregister
            InventoryManager.Instance.OnInventoryUpdate -= UpdateUI;
        }

        private void Start()
        {
            UpdateUI();
        }

        void UpdateUI()
        {
            // Clear existing slots
            foreach (Transform child in itemSlotContainer)
            {
                Destroy(child.gameObject);
            }

            // Populate slots based on current inventory
            foreach (var inventoryItem in InventoryManager.Instance.inventoryList)
            {
                GameObject slot = Instantiate(itemSlotPrefab, itemSlotContainer);
                slot.transform.Find("Image").GetComponent<Image>().sprite = inventoryItem.item.icon;
                slot.transform.Find("Count").GetComponent<TextMeshProUGUI>().SetText(inventoryItem.count.ToString());
            }
        }
    }
}