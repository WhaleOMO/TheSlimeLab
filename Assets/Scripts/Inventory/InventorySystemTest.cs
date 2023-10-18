using System;
using UnityEngine;

namespace Inventory
{
    public class InventorySystemTest: MonoBehaviour
    {
        public Item testItem1;
        public Item testItem2;

        private void Start()
        {
            Invoke("TestAddItems", 1);
        }

        [ContextMenu("Test Add Items")]
        public void TestAddItems()
        {
            InventoryManager.Instance.AddItem(testItem1);
            InventoryManager.Instance.AddItem(testItem1);
            InventoryManager.Instance.AddItem(testItem2);
            InventoryManager.Instance.AddItem(testItem2);
        }
        
        [ContextMenu("Test Remove Items")]
        public void TestRemoveItems()
        {
            InventoryManager.Instance.RemoveItem(testItem1);
            InventoryManager.Instance.RemoveItem(testItem1);
            InventoryManager.Instance.RemoveItem(testItem2);
            InventoryManager.Instance.RemoveItem(testItem2);
        }
    }
}