using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName = "newItem", menuName = "Inventory/Item")]
    public class Item: ScriptableObject
    {
        public int id;
        public string itemName;
        [TextArea]
        public string description;
        public Sprite icon;
        public GameObject prefab;   
        public Color color;                 
        public bool isStackable;                // if the item could stack in UI Item slot
        public int maxStackAmount = 10;         // The maximum amount to stack

        /// <summary>
        /// Create a new Item, with only color difference
        /// </summary>
        /// <param name="from">The original item to copy from</param>
        /// <param name="newColor">The new color to be set</param>
        /// <returns>a new runtime instance of the Item. This new instance won't be saved as an asset in the project</returns>
        public Item CreateColorVariant(Item from, Color newColor)
        {
            Item newItem = ScriptableObject.CreateInstance<Item>();

            // Copy properties from the original item to the new item
            {
                newItem.id = from.id;
                newItem.itemName = from.itemName;
                newItem.description = from.description;
                newItem.icon = from.icon;
                newItem.prefab = from.prefab;
                newItem.isStackable = from.isStackable;
                newItem.maxStackAmount = from.maxStackAmount;
            }
            
            // Set the new color
            newItem.color = newColor;

            return newItem;
        }
        
        public override bool Equals(object o)
        {
            if (o.GetType() == typeof(Item))
            {
                return id == (o as Item).id && color.CompareRGB((o as Item).color);
            }

            return false;
        }

        public override int GetHashCode()
        {
            int hash = 17; // Starting value
            hash = hash * 23 + id.GetHashCode();
            hash = hash * 23 + color.GetHashCode();
            return hash;
        }
    }
}