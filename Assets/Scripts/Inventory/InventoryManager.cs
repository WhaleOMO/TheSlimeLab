using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance { get; private set; }
        
        // For display purposes (UI reference)
        public readonly List<InventoryItem> inventoryList = new List<InventoryItem>();
        // For Operation (Add, Remove, Search, etc..)
        private Dictionary<Item, InventoryItem> inventory = new Dictionary<Item, InventoryItem>();
        // How many item slots are available in UI 
        public int maxInventorySize = 15;

        public event Action OnInventoryUpdate;
        private void Awake()
        {
            // Ensure that there's only one instance of InventoryManager
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
        
        public bool AddItem(Item newItem)
        {
            if (newItem.isStackable)
            {
                // Check if the item exists in the dictionary
                if (inventory.TryGetValue(newItem, out InventoryItem existingItem) 
                    && existingItem.count < newItem.maxStackAmount)
                {
                    existingItem.count++;
                    OnInventoryUpdate?.Invoke();
                    return true;
                }
            }

            if (inventory.Count < maxInventorySize)
            {
                InventoryItem invItem = new InventoryItem { item = newItem, count = 1 };
                inventory.Add(newItem, invItem);
                inventoryList.Add(invItem); 
                OnInventoryUpdate?.Invoke();
                return true;  
            }

            return false;  // Failed to add
        }

        public bool RemoveItem(Item itemToRemove)
        {
            if (inventory.TryGetValue(itemToRemove, out InventoryItem existingItem))
            {
                existingItem.count--;

                if (existingItem.count <= 0)
                {
                    inventory.Remove(itemToRemove);
                    inventoryList.Remove(existingItem); 
                }
                
                OnInventoryUpdate?.Invoke();
                return true; 
            }

            return false;  // Failed to remove
        }
        
        // Direct Check
        public bool HasItem(Item item)
        {
            return inventory.ContainsKey(item);
        }
    
        // Check by ID
        public bool HasItem(int itemID)
        {
            foreach (var key in inventory.Keys)
            {
                if (key.id == itemID)
                {
                    return true;
                }
            }

            return false;
        }
        
        public void ClearInventory()
        {
            this.inventory.Clear();
            this.inventoryList.Clear();
            OnInventoryUpdate?.Invoke();
        }
        // ... Additional methods like GetItem, HasItem, ClearInventory, etc. ...
    }

}