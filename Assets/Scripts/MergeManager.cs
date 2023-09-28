using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeManager : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject slime1, slime2;
    GameObject catch1, catch2;
    public GameObject slimePrefab;
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
    void MergeImplement()
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
            slime1.SetActive(false);
            catch1.SetActive(false);
            slime2.SetActive(false);            
            catch2.SetActive(false);
            SpawnSlime(spawnPosition, spawnRotation);
            slime1 = null;
            slime2 = null;
            catch1 = null;
            catch2 = null;
        }
    }

    void SpawnSlime(Vector3 spawnPosition,Quaternion spawnRotation)
    {
        GameObject newSlime = Instantiate(slimePrefab, spawnPosition, spawnRotation);
    }
}
