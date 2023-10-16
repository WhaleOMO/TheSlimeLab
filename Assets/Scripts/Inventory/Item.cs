using System;
using System.Collections.Generic;
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

        public override bool Equals(object other)
        {
            return base.Equals(other);
        }

        protected bool Equals(Item other)
        {
            return id == other.id && color.Equals(other.color);
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