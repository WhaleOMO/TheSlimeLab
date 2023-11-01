using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Inventory;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace Interactables
{
    public class ItemReceiver : MonoBehaviour
    {
        // UI display
        public Image icon1, icon2;
        public Transform targetPosition;
        
        public Liquid liquid;
        public float halfLiquidHeight = 2.5f;
        
        private Item _item1, _item2;
        private bool _item1Received = false;
        private bool _item2Received = false;
        
        private void Start()
        {

        }

        private async Task WaitAndPlayAnimation()
        {
            liquid.transform.parent.GetComponent<Animator>().SetBool("AnimTrigger", true);
            await Task.Delay(TimeSpan.FromSeconds(2.5f));
        }
        
        public async void OnGenerateButtonPressed()
        {
            Color targetColor = _item1.color;
            await WaitAndPlayAnimation();
            if (_item1 != null)
            {
                var mergeManager = FindObjectOfType<MergeManager>();
                mergeManager.SpawnSlime(
                    targetPosition.position, 
                    targetPosition.rotation,
                    targetColor, 
                    new Vector3(1, 1, 1), 
                    (_item2 != null)?_item2.id-1 : _item1.id-1);
                _item1 = null;
                _item2 = null;
                icon1.sprite = null;
                icon2.sprite = null;
                icon1.color = Color.white;
                icon2.color = Color.white;
                liquid.fillAmount = 2 * halfLiquidHeight;
            }
        }

        private IEnumerator AddLiquid()
        {
            float second = 0.5f;
            int count = 50;
            for (int i = 0; i < count; i++)
            {
                liquid.fillAmount -= halfLiquidHeight / count;
                yield return new WaitForSeconds(second/count);
            }
        }
        
        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.TryGetComponent<ItemHolder>(out var itemHolder))
            {
                liquid.transform.parent.GetComponent<Animator>().SetBool("AnimTrigger", false);
                if (itemHolder.item.itemType == ItemType.Crystal)
                {                   
                    if (_item1 == null && itemHolder.item.id == 1)
                    {
                        _item1 = itemHolder.item;
                        icon1.sprite = itemHolder.item.icon;
                        icon1.color = itemHolder.item.color;
                        Destroy(collision.gameObject);
                        liquid.GetComponent<MeshRenderer>().material.SetColor("_BottomColor", _item1.color);
                        liquid.GetComponent<MeshRenderer>().material.SetColor("_FoamColor", _item1.color + 0.4f * Random.ColorHSV());
                        StartCoroutine(AddLiquid());
                    }

                    if (_item2 == null && itemHolder.item.id > 1)
                    {
                        _item2 = itemHolder.item;
                        icon2.sprite = itemHolder.item.icon;
                        Color _tempColor = icon2.color;
                        _tempColor.a = 1;
                        Destroy(collision.gameObject);
                        StartCoroutine(AddLiquid());
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
