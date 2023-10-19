using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class MergeManager : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject slime1, slime2;
    GameObject catch1, catch2;
    Slime slimeData1, slimeData2;

    public GameObject[] level2Slimes;
    public GameObject[] level3Slimes;

    public GameObject[] slimeCrystals;
    
    public GameObject basicPrefab;
    public GameObject vfxPrefab;

    private int _maxAllowed;
    private int _mergedAmount;
    private int _slimeCount;
    
    void Start()
    {
        slime1 = null;
        slime2 = null;
        catch1 = null;
        catch2 = null;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {   
        if (catch1 != null && catch2 != null && slimeData1.CombinationCheck(slimeData2))
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
        _maxAllowed = SlimeManager.maxInScene;
        _slimeCount = SlimeManager.slimeCount;
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
            slimeData1 = slime1.GetComponent<SlimeManager>().GetSlime();
            catch1 = catchPoint;
        }
        else
        {
            slime2 = slime;
            slimeData2 = slime2.GetComponent<SlimeManager>().GetSlime();
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
        if (distanceOfCentre < 0.75 * threshold)
        {
            /*
            Destroy(slime1);
            
            Destroy(slime2);
            
            Destroy(catch1);
            
            Destroy(catch2);
            */
            Vector3 spawnPosition = (catch1.transform.position + catch1.transform.position)/2;
            Quaternion spawnRotation = slime1.transform.rotation;
            Debug.Log("slime1: "+ slimeData1.GetSlimeLevel() + "and slime2: " + slimeData2.GetSlimeLevel());

            Slime newSlimeData = new Slime(slimeData1, slimeData2);
            Debug.Log("newSlime level is" + newSlimeData.GetSlimeLevel());
            int prefabIndex = 0;
            int newSlimeLevel = newSlimeData.GetSlimeLevel();
            if(newSlimeLevel == 2)
            {
                prefabIndex = newSlimeData.GetSlimeDecorationIndex();
            }
            if(newSlimeLevel == 3)
            {
                prefabIndex = DataLoader.mergeData[slimeData1.GetSlimeDecorationIndex()][slimeData2.GetSlimeDecorationIndex()];
            }

            Vector3 size_1 = slime1.transform.localScale;
            Vector3 size_2 = slime2.transform.localScale;

            Vector3 newSize = size_1 + size_2;




            slime1.SetActive(false);
            catch1.SetActive(false);
            slime2.SetActive(false);            
            catch2.SetActive(false);



            await SpawnCrystal(spawnPosition, spawnRotation, slimeData1);
            await SpawnCrystal(spawnPosition, spawnRotation, slimeData2);
            await SpawnSlime(spawnPosition, spawnRotation, newSlimeData, newSize, prefabIndex);
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
        GameObject newSlime = Instantiate(basicPrefab, spawnPosition, spawnRotation);
        newSlime.transform.localScale = size;
        MeshRenderer renderer = newSlime.GetComponentInChildren<MeshRenderer>();
        Material mat = renderer.material;
        mat.SetColor(SlimeShaderProperties.BaseColor, baseColor);
        mat.SetColor(SlimeShaderProperties.AmbientColor, baseColor * 0.4f);
        mat.SetColor(SlimeShaderProperties.RimColor, baseColor*3);
        renderer.material = mat;
        _mergedAmount++;
    }

    async Task SpawnSlime(Vector3 spawnPosition, Quaternion spawnRotation, Slime slime, Vector3 size, int prefabIndex = 0)
    {
        GameObject vfx = Instantiate(vfxPrefab, spawnPosition, spawnRotation);
        //GameObject newSlimeModel;
        await Task.Delay(TimeSpan.FromSeconds(0.5f));
        GameObject slimePrefab;
        int slimeLevel = slime.GetSlimeLevel();

        slimePrefab = (slimeLevel == 2) ? level2Slimes[prefabIndex] : level3Slimes[prefabIndex];
        //else if (slimeLevel == 2) slimePrefab = level2Slimes[prefabIndex];
        //else slimePrefab = level3Slimes[prefabIndex];

        GameObject newSlime = Instantiate(slimePrefab, spawnPosition, spawnRotation);
        newSlime.transform.localScale = size;
        MeshRenderer renderer = newSlime.GetComponentInChildren<MeshRenderer>();
        Material mat = renderer.material;
        Color newColor = slime.GetSlimeColor();
        mat.SetColor(SlimeShaderProperties.BaseColor, newColor);
        mat.SetColor(SlimeShaderProperties.AmbientColor, newColor * 0.4f);
        mat.SetColor(SlimeShaderProperties.RimColor, newColor * 3);
        renderer.material = mat;
        newSlime.GetComponent<SlimeManager>().SetSlime(slime);


        _mergedAmount++;
    }

    async Task SpawnCrystal(Vector3 spawnPosition, Quaternion spawnRotation, Slime slime)
    {
        int index = 0;
        int level = slime.GetSlimeLevel();
        if(level != 1)
            index = slime.GetSlimeDecorationIndex();
        GameObject newCrystal = Instantiate(slimeCrystals[index], spawnPosition, spawnRotation);


    }

}
    