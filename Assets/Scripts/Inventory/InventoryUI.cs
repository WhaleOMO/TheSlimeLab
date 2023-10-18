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

        public Action OnItemBeginDrag;
        public Action OnItemEndDrag;
        
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
            InitUI();
        }

        void InitUI()
        {
            // Destroy everything
            foreach (Transform child in itemSlotContainer)
            {
                Destroy(child.gameObject);
            }
            
            // Create Slots
            for (int i = 0; i < InventoryManager.Instance.maxInventorySize; i++)
            {
                GameObject slot = Instantiate(itemSlotPrefab, itemSlotContainer);
                slot.GetComponent<ItemSlot>().item = null;
                slot.GetComponent<ItemSlot>().SetUIRoot(this);
                slot.transform.Find("Image").GetComponent<Image>().sprite = null;
                slot.transform.Find("Count").GetComponent<TextMeshProUGUI>().SetText("Empty Slot");
            }
        }

        void UpdateUI()
        {
            // Clear UI
            foreach(Transform slot in itemSlotContainer)
            {
                slot.GetComponent<ItemSlot>().item = null;
                slot.transform.Find("Image").GetComponent<Image>().sprite = null;
                slot.transform.Find("Image").GetComponent<Image>().color = Color.black;
                slot.transform.Find("Count").GetComponent<TextMeshProUGUI>().SetText("Empty Slot");
            }
            
            // Populate slots based on current inventory
            for(int i = 0; i < InventoryManager.Instance.inventoryList.Count; i++)
            {
                Transform slot = itemSlotContainer.GetChild(i);
                var inventoryItem = InventoryManager.Instance.inventoryList[i];
                slot.GetComponent<ItemSlot>().item = inventoryItem.item;
                slot.GetComponent<ItemSlot>().SetUIRoot(this);
                slot.transform.Find("Image").GetComponent<Image>().sprite = inventoryItem.item.icon;
                slot.transform.Find("Image").GetComponent<Image>().color = inventoryItem.item.color;
                slot.transform.Find("Count").GetComponent<TextMeshProUGUI>().SetText(inventoryItem.count.ToString());
            }
        }
    }
}