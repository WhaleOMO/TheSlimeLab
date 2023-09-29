using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class SlimeManager : MonoBehaviour
{
    public List<Slime> slimes;
    public GameObject[] initialSlimes;
    public GameObject[] slimePrefabs;
    public int maxInscene = 15; //
    public int slimeCount;
    public GameObject vfxPrefab;

    // Start is called before the first frame update
    void Start()
    {
        slimeCount = initialSlimes.Length;
        for (int i = 0;i < slimeCount; i++)
        {
            Color color = initialSlimes[i].GetComponentInChildren<Renderer>().material.GetColor("_BaseColor");
            int ID = initialSlimes[i].GetInstanceID();
            slimes.Add(new Slime(ID, 1, color));
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

        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.developerConsoleVisible = !Debug.developerConsoleVisible;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    async Task SpawnSlime(Vector3 spawnPosition, Quaternion spawnRotation, Slime slime)
    {
        GameObject vfx = Instantiate(vfxPrefab, spawnPosition, spawnRotation);
        //GameObject newSlimeModel;
        await Task.Delay(TimeSpan.FromSeconds(0.5f));
        int level = slime.GetSlimeLevel();
        Color baseColor = slime.GetSlimeColor();

        GameObject newSlime = Instantiate(slimePrefabs[level], spawnPosition, spawnRotation);
        int ID = newSlime.GetInstanceID();
        newSlime.transform.localScale *= 1.5f;
        MeshRenderer renderer = newSlime.GetComponentInChildren<MeshRenderer>();
        Material mat = renderer.material;
        mat.SetColor(SlimeShaderProperties.BaseColor, baseColor);
        mat.SetColor(SlimeShaderProperties.AmbientColor, baseColor * 0.4f);
        mat.SetColor(SlimeShaderProperties.RimColor, baseColor * 3);
        renderer.material = mat;
        slimeCount++;
        slimes.Add(slime);
    }
}
