using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using UnityEngine.Events;

public class MergeManager : MonoBehaviour
{
    public static MergeManager instance;
    
    // Start is called before the first frame update
    GameObject slime1, slime2;
    GameObject catch1, catch2;
    
    public GameObject slimePrefab;
    public GameObject vfxPrefab;
    public SlimeManager slimeManager;

    public int _maxAllowed;
    
    private int _mergedAmount;
    private int _slimeCount;
    
    void Start()
    {
        slime1 = null;
        slime2 = null;
        catch1 = null;
        catch2 = null;
        instance = this;
    }

    // Update is called once per frame

    private void FixedUpdate()
    {   
        if (catch1 != null && catch2 != null)
        {
            MergeImplement();
        }

        if (_mergedAmount >= 3)
        {
            SpawnSlime(new Vector3(0, 8, 0), Quaternion.identity, Random.ColorHSV(0, 1, 0.3f, 0.6f, 0.5f, 0.8f),new Vector3(1,1,1));
            _mergedAmount-=2;
        }
        
    }

    public void AddSlimeAtDefaultPos()
    {
        _maxAllowed = slimeManager.maxInscene;
        _slimeCount = slimeManager.slimeCount;
        if (_slimeCount < _maxAllowed)
        {
            SpawnSlime(new Vector3(0, 8, 0), Quaternion.identity, Random.ColorHSV(0, 1, 0.3f, 0.6f, 0.5f, 0.8f), new Vector3(1,1,1));
            _mergedAmount-=2;
        }
    }
    
    public void AddSlime(GameObject slime, GameObject catchPoint)
    {
        if (slime1 == null)
        {
            slime1 = slime;
            catch1 = catchPoint;
        }
        else
        {
            slime2 = slime;
            catch2 = catchPoint;
        }
    }

    public void ReleaseSlime(GameObject slime, GameObject catchPoint)
    {
        if (slime1 != null && slime1.Equals(slime))
        {
            slime1 = null;
            catch1 = null;
        }
        else
        {
            slime2 = null;
            catch2 = null;
        }
    }
    
    async void MergeImplement()
    {
        float distanceOfCentre = (catch1.transform.position - catch2.transform.position).magnitude;
        float threshold = catch1.GetComponent<SphereCollider>().radius + catch2.GetComponent<SphereCollider>().radius;
        if (distanceOfCentre > 0.25 * threshold && distanceOfCentre < 0.9 * threshold)
        {
            SlimeSound.instance.PlaySqueezeSound(distanceOfCentre);
        }
        else if (distanceOfCentre < 0.25 * threshold)
        {
            /*
            Destroy(slime1);
            
            Destroy(slime2);
            
            Destroy(catch1);
            
            Destroy(catch2);
            */
            Vector3 spawnPosition = (catch1.transform.position + catch1.transform.position)/2;
            Quaternion spawnRotation = slime1.transform.rotation;
            Material mat_1 = slime1.GetComponentInChildren<MeshRenderer>().material;
            Material mat_2 = slime2.GetComponentInChildren<MeshRenderer>().material;
            Vector3 size_1 = slime1.transform.localScale;
            Vector3 size_2 = slime2.transform.localScale;

            Vector3 newSize = size_1 + size_2;
            Color newSlimeColor = mat_1.GetColor("_BaseColor")/2 + mat_2.GetColor("_BaseColor")/2;
            slime1.SetActive(false);
            catch1.SetActive(false);
            slime2.SetActive(false);            
            catch2.SetActive(false);

            await SpawnSlime(spawnPosition, spawnRotation, newSlimeColor, newSize);
            Destroy(slime1);
            Destroy(slime2);
            catch1 = null;
            catch2 = null;
        }
    }
    
    async Task SpawnSlime(Vector3 spawnPosition,Quaternion spawnRotation,Color baseColor, Vector3 size)
    {
        GameObject vfx = Instantiate(vfxPrefab, spawnPosition, spawnRotation);
        //GameObject newSlimeModel;
        await Task.Delay(TimeSpan.FromSeconds(0.5f));
        GameObject newSlime = Instantiate(slimePrefab, spawnPosition, spawnRotation);
        newSlime.transform.localScale = size;
        MeshRenderer renderer = newSlime.GetComponentInChildren<MeshRenderer>();
        Material mat = renderer.material;
        mat.SetColor(SlimeShaderProperties.BaseColor, baseColor);
        mat.SetColor(SlimeShaderProperties.AmbientColor, baseColor * 0.4f);
        mat.SetColor(SlimeShaderProperties.RimColor, baseColor*3);
        renderer.material = mat;
        _mergedAmount++;
    }
}
