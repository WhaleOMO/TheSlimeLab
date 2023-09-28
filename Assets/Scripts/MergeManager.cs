using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MergeManager : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject slime1, slime2;
    GameObject catch1, catch2;
    
    public GameObject slimePrefab;
    public GameObject vfxPrefab;
    
    void Start()
    {
        slime1 = null;
        slime2 = null;
        catch1 = null;
        catch2 = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {   
        if (catch1 != null && catch2 != null)
        {
            MergeImplement();
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
            Material mat_1 = slime1.GetComponentInChildren<MeshRenderer>().material;
            Material mat_2 = slime2.GetComponentInChildren<MeshRenderer>().material;
            Color newSlimeColor = mat_1.GetColor("_BaseColor")/2 + mat_2.GetColor("_BaseColor")/2;
            slime1.SetActive(false);
            catch1.SetActive(false);
            slime2.SetActive(false);            
            catch2.SetActive(false);

            await SpawnSlime(spawnPosition, spawnRotation, newSlimeColor);
            slime1 = null;
            slime2 = null;
            catch1 = null;
            catch2 = null;
        }
    }
    
    async Task SpawnSlime(Vector3 spawnPosition,Quaternion spawnRotation,Color baseColor)
    {
        GameObject vfx = Instantiate(vfxPrefab, spawnPosition, spawnRotation);
        //GameObject newSlimeModel;
        await Task.Delay(TimeSpan.FromSeconds(0.5f));
        GameObject newSlime = Instantiate(slimePrefab, spawnPosition, spawnRotation);
        newSlime.transform.localScale *= 1.5f;
        MeshRenderer renderer = newSlime.GetComponentInChildren<MeshRenderer>();
        Material mat = renderer.material;
        mat.SetColor("_BaseColor", baseColor);
        mat.SetColor("_RimColor", baseColor*3);
        renderer.material = mat;
    }
}
