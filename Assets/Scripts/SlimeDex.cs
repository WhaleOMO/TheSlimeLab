using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SlimeDex : MonoBehaviour
{
    public int SlimeID = 99;
    public float fadeTime = 1f;
    public GameObject vfxPrefab;
    
    public List<Image> Items = new List<Image>();
    public List<Image> lockicons = new List<Image>();
    public List<Image> unlockicons = new List<Image>();
    private List<bool> isLightUp = Enumerable.Repeat(false, 30).ToList();
    public List<GameObject> SlimeInContainer = new List<GameObject>(8);
    
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

        foreach (var slime in SlimeInContainer)
        {
            slime.SetActive(false);
        }
        
        Items[0].color = Color.white;
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
                /*case 0:
                    StartCoroutine(ItemsAnimation(lockicons[0], unlockicons[0]));
                    Items[0].color = Color.white;
                    SlimeID = 99;
                    break;*/
                case 1:
                    if (!isLightUp[1])
                    {
                        StartCoroutine(ItemsAnimation(lockicons[1], unlockicons[1]));
                        Items[1].color = Color.white;
                        isLightUp[1] = true;
                    }
                    SlimeID = 99;
                    break;
                case 2:
                    if (!isLightUp[2])
                    {
                        StartCoroutine(ItemsAnimation(lockicons[2], unlockicons[2]));
                        Items[2].color = Color.white;
                        isLightUp[2] = true;
                    }
                    SlimeID = 99;
                    break;
                case 3:
                    if (!isLightUp[3])
                    {
                        StartCoroutine(ItemsAnimation(lockicons[3], unlockicons[3]));
                        Items[3].color = Color.white;
                        isLightUp[3] = true;
                    }
                    SlimeID = 99;
                    break;
                case 4:
                    if (!isLightUp[4])
                    {
                        StartCoroutine(ItemsAnimation(lockicons[4], unlockicons[4]));
                        Items[4].color = Color.white;
                        isLightUp[4] = true;
                    }
                    SlimeID = 99;
                    break;
                case 5:
                    if (!isLightUp[5])
                    {
                        StartCoroutine(ItemsAnimation(lockicons[5], unlockicons[5]));
                        Items[5].color = Color.white;
                        isLightUp[5] = true;
                    }
                    SlimeID = 99;
                    break;
                case 6:
                    if (!isLightUp[6])
                    {
                        StartCoroutine(ItemsAnimation(lockicons[6], unlockicons[6]));
                        Items[6].color = Color.white;
                        isLightUp[6] = true;
                    }
                    SlimeID = 99;
                    break;
                case 7:
                    if (!isLightUp[7])
                    {
                        StartCoroutine(ItemsAnimation(lockicons[7], unlockicons[7]));
                        Items[7].color = Color.white;
                        isLightUp[7] = true;
                    }
                    SlimeID = 99;
                    break;
                case 8:
                    if (!isLightUp[8])
                    {
                        StartCoroutine(ItemsAnimation(lockicons[8], unlockicons[8]));
                        Items[8].color = Color.white;
                        isLightUp[8] = true;
                    }
                    SlimeID = 99;
                    break;
                case 9:
                    if (!isLightUp[9])
                    {
                        StartCoroutine(ItemsAnimation(lockicons[9], unlockicons[9]));
                        Items[9].color = Color.white;
                        isLightUp[9] = true;
                    }
                    SlimeID = 99;
                    break;
                case 10:
                    if (!isLightUp[10])
                    {
                        StartCoroutine(ItemsAnimation(lockicons[10], unlockicons[10]));
                        Items[10].color = Color.white;
                        isLightUp[10] = true;
                        SlimeInContainer[0].SetActive(true);
                        GameObject vfx = Instantiate(vfxPrefab, SlimeInContainer[2].transform.position, SlimeInContainer[2].transform.rotation);
                    }
                    SlimeID = 99;
                    break;
                case 11:
                    if (!isLightUp[11])
                    {
                        StartCoroutine(ItemsAnimation(lockicons[11], unlockicons[11]));
                        Items[11].color = Color.white;
                        isLightUp[11] = true;
                        SlimeInContainer[1].SetActive(true);
                        GameObject vfx = Instantiate(vfxPrefab, SlimeInContainer[3].transform.position, SlimeInContainer[3].transform.rotation);
                    }
                    SlimeID = 99;
                    break;
                case 12:
                    if (!isLightUp[12])
                    {
                        StartCoroutine(ItemsAnimation(lockicons[12], unlockicons[12]));
                        Items[12].color = Color.white;
                        isLightUp[12] = true;
                        SlimeInContainer[2].SetActive(true);
                        GameObject vfx = Instantiate(vfxPrefab, SlimeInContainer[4].transform.position, SlimeInContainer[4].transform.rotation);
                    }
                    SlimeID = 99;
                    break;
                case 13:
                    if (!isLightUp[13])
                    {
                        StartCoroutine(ItemsAnimation(lockicons[13], unlockicons[13]));
                        Items[13].color = Color.white;
                        isLightUp[13] = true; 
                        SlimeInContainer[3].SetActive(true);
                        GameObject vfx = Instantiate(vfxPrefab, SlimeInContainer[5].transform.position, SlimeInContainer[5].transform.rotation);
                    }
                    SlimeID = 99;
                    break;
                case 14:
                    if (!isLightUp[14])
                    {
                        StartCoroutine(ItemsAnimation(lockicons[14], unlockicons[14]));
                        Items[14].color = Color.white;
                        isLightUp[14] = true;
                        SlimeInContainer[4].SetActive(true);
                        GameObject vfx = Instantiate(vfxPrefab, SlimeInContainer[6].transform.position, SlimeInContainer[6].transform.rotation);
                    }
                    SlimeID = 99;
                    break;
                case 15:
                    if (!isLightUp[15])
                    {
                        StartCoroutine(ItemsAnimation(lockicons[15], unlockicons[15]));
                        Items[15].color = Color.white;
                        isLightUp[15] = true;
                        SlimeInContainer[5].SetActive(true);
                        GameObject vfx = Instantiate(vfxPrefab, SlimeInContainer[6].transform.position, SlimeInContainer[6].transform.rotation);
                    }
                    SlimeID = 99;
                    break;
                case 16:
                    if (!isLightUp[16])
                    {
                        StartCoroutine(ItemsAnimation(lockicons[16], unlockicons[16]));
                        Items[16].color = Color.white;
                        isLightUp[16] = true;
                        SlimeInContainer[6].SetActive(true);
                        GameObject vfx = Instantiate(vfxPrefab, SlimeInContainer[6].transform.position, SlimeInContainer[6].transform.rotation);
                    }
                    SlimeID = 99;
                    break;
                
            }
        }
    }
}
