using System;
using System.Collections.Generic;
using Inventory;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Interactables
{
    public class ItemReceiver : MonoBehaviour
    {
        // UI display
        public Image icon1, icon2;
        
        private List<Item> _tempItems;
        private Item _item1, _item2;
    
        private void Start()
        {
            _tempItems = new List<Item>();
        }
    
        public void OnGenerateButtonPressed()
        {
            if (_tempItems.Count < 2) {
                return;
            }
            Color targetColor = Color.black;
            Item item1 = _tempItems[0];
            bool isLeagel = false;
            bool twoObj = false;
            if (item1.id == 1)
            {
                _tempItems.RemoveAt(0);
                targetColor = item1.color;
                isLeagel = true;
            }
    
            Item item2 = new Item();
            int length = _tempItems.Count;
            for(int i = 0; i < length; i++)
            {
                if (_tempItems[i].id != item1.id)
                {
                    twoObj = true;
                    item2 = _tempItems[i];
                    _tempItems.Remove(item2);
                    if (item1.id > 1)
                    {
                        _tempItems.Remove(item1);
                        targetColor = item2.color;
                    }
                    isLeagel = true;
                }
            }
    
            if (isLeagel)
            {
                var mergeManager = FindObjectOfType<MergeManager>();
                mergeManager.SpawnSlime(
                    new Vector3(0,12,0), 
                    Quaternion.identity, 
                    targetColor, 
                    new Vector3(1, 1, 1), 
                    twoObj?Mathf.Max(item1.id, item2.id)-1 : item1.id-1);
            }

            Color _tempColor = icon1.color;
            _tempColor.a = 0;
            _tempColor = icon2.color;
            _tempColor.a = 0;
        }
        
        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.TryGetComponent<ItemHolder>(out var itemHolder))
            {
                if (itemHolder.item.itemType == ItemType.Crystal)
                {
                    Destroy(collision.gameObject);
                    _tempItems.Add(itemHolder.item);
                    
                    if (_item1 == null && itemHolder.item.id == 1)
                    {
                        _item1 = itemHolder.item;
                        icon1.sprite = itemHolder.item.icon;
                        icon1.color = itemHolder.item.color;

                    }

                    if (_item2 == null && itemHolder.item.id > 1)
                    {
                        _item2 = itemHolder.item;
                        icon2.sprite = itemHolder.item.icon;
                        Color _tempColor = icon2.color;
                        _tempColor.a = 1;
                    }
                }
            }
        }
    
        private void OnTriggerExit(Collider other)
        {
            // if (other.gameObject.TryGetComponent<ItemHolder>(out var itemHolder))
            // {
            //     if (itemHolder.item.itemType == ItemType.Crystal)
            //     {
            //         _tempItems.Remove(itemHolder.item);
            //     }
            // }
        }
    }

    // public class ItemReceiver : MonoBehaviour
    // {
    //     private Item _colorItem, _shapeItem;
    //     
    //     private void OnTriggerEnter(Collider collision)
    //     {
    //         if (_colorItem != null && _shapeItem != null)
    //         {
    //             return;
    //         }
    //         
    //         if (collision.gameObject.TryGetComponent<ItemHolder>(out var itemHolder))
    //         {
    //             if (itemHolder.item.itemType == ItemType.Crystal)
    //             {
    //                 Destroy(collision.gameObject);
    //             }
    //         }
    //     }
    // }
}
