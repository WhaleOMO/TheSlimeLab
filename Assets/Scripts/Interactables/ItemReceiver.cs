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
        private Item _item1, _item2;
        private bool _item1Received = false;
        private bool _item2Received = false;
        private void Start()
        {

        }
    
        public void OnGenerateButtonPressed()
        {
            Color targetColor = Color.black;
            if (_item1 != null)
            {
                var mergeManager = FindObjectOfType<MergeManager>();
                mergeManager.SpawnSlime(
                    new Vector3(0,12,0), 
                    Quaternion.identity, 
                    targetColor, 
                    new Vector3(1, 1, 1), 
                    (_item2 != null)?_item2.id-1 : _item1.id-1);
                _item1 = null;
                _item2 = null;
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
                    if (_item1 == null && itemHolder.item.id == 1)
                    {
                        _item1 = itemHolder.item;
                        icon1.sprite = itemHolder.item.icon;
                        icon1.color = itemHolder.item.color;
                        Destroy(collision.gameObject);

                    }

                    if (_item2 == null && itemHolder.item.id > 1)
                    {
                        _item2 = itemHolder.item;
                        icon2.sprite = itemHolder.item.icon;
                        Color _tempColor = icon2.color;
                        _tempColor.a = 1;
                        Destroy(collision.gameObject);
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
