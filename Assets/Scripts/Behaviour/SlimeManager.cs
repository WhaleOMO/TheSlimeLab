using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class SlimeManager : MonoBehaviour
{
    public Slime[] slimes;
    public GameObject[] slimeObjects;
    public GameObject[] slimePrefabs;
    public int maxInscene = 15; //
    public int slimeCount;
    public GameObject vfxPrefab;

    // Start is called before the first frame update
    void Start()
    {
        slimeCount = slimeObjects.Length;
        for (int i = 0;i < slimeCount; i++)
        {
            Color color = slimeObjects[i].GetComponentInChildren<Renderer>().material.GetColor("_BaseColor");
            slimes[i] = new Slime(1, color);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScene();
    }

    void UpdateScene()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        if (Input.GetKeyDown(KeyCode.N))
        {
           MergeManager.instance.AddSlimeAtDefaultPos();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.developerConsoleVisible = !Debug.developerConsoleVisible;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
