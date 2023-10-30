using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SlimeDex : MonoBehaviour
{
    public int SlimeID = 99;
    public float fadeTime = 1f;
    public List<Image> Items = new List<Image>();
    public List<Image> lockicons = new List<Image>();
    public List<Image> unlockicons = new List<Image>();
        
    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in Items)
        {
            item.color = Color.black;
        }

        foreach (var icon in unlockicons)
        {
            var iconColor = icon.color;
            iconColor.a = 0f;
        }
    }

    public void IconFadeIn(Image icon)
    {
        var iconColor = icon.color;
        iconColor.a = 0f;
        icon.DOFade(1, fadeTime);
    }

    public void IconFadeOut(Image icon)
    {
        var iconColor = icon.color;
        iconColor.a = 1f;
        icon.DOFade(0, fadeTime);
    }

    IEnumerator ItemsAnimation(Image lockIcon, Image unlockIcon)
    {
        IconFadeOut(lockIcon);
        IconFadeIn(unlockIcon);
        yield return new WaitForSeconds(1f);
        IconFadeOut(unlockIcon);
    }
    // Update is called once per frame
    void Update()
    {
        if (SlimeID != 99)
        {
            switch (SlimeID)
            {
                case 0:
                    StartCoroutine(ItemsAnimation(lockicons[0], unlockicons[0]));
                    Items[0].color = Color.white;
                    SlimeID = 99;
                    break;
                case 1:
                    StartCoroutine(ItemsAnimation(lockicons[1], unlockicons[1]));
                    Items[1].color = Color.white;
                    SlimeID = 99;
                    break;
                case 2: 
                    StartCoroutine(ItemsAnimation(lockicons[2], unlockicons[2]));
                    Items[2].color = Color.white;
                    SlimeID = 99;
                    break;
                case 3: 
                    StartCoroutine(ItemsAnimation(lockicons[3], unlockicons[3]));
                    Items[3].color = Color.white;
                    SlimeID = 99;
                    break;
                case 4: 
                    StartCoroutine(ItemsAnimation(lockicons[4], unlockicons[4]));
                    Items[4].color = Color.white;
                    SlimeID = 99;
                    break;
            }
        }
    }
}
