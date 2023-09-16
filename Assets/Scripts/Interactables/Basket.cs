using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Basket : MonoBehaviour
{
    public static float score;

    [SerializeField] 
    private float basketCredit = 1;         // How much score player will get when shoot in
    [SerializeField] 
    private GameObject textGameObject;      // The text game object to display score
    
    static Basket()
    {
        score = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<SlimeMoving>(out var slimeMoving))
        {
            slimeMoving.transform.gameObject.SetActive(false);
            Destroy(slimeMoving.gameObject);
            score += basketCredit;
            
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.Beep();
            #endif
            
            if (textGameObject)
            {
                textGameObject.GetComponent<TextMeshPro>().SetText(score.ToString());
            }
            
        }
    }
}
