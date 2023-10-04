using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class SlimeManager : MonoBehaviour
{
    public static List<Slime> allSlimes = new List<Slime>();
    public static int maxInScene = 15;
    public static int slimeCount;


    private Slime _slime;
    private int ID;



    // Start is called before the first frame update
    private void OnEnable()
    {
        //Generate slime data
        FetchData();

        AddSlime(_slime);

        Debug.Log(allSlimes.Count);
    }

    void Start()
    {
        ID = _slime.GetSlimeID();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScene();

    }

    private void OnDisable()
    {
        DeletSlime(ID);
        Debug.Log(allSlimes.Count);
    }


    private void UpdateScene()
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

    public void AddSlime(Slime slime)
    {
        allSlimes.Add(slime);
        slimeCount = allSlimes.Count;
    }

    public void FetchData()
    {
        //Generate slime data
        int ID = GetInstanceID();
        string level_string = gameObject.tag;
        int level = level_string[5];
        Color slimeColor = GetComponentInChildren<MeshRenderer>().material.GetColor("_BaseColor");
        this._slime = new Slime(ID, level, slimeColor);
    }

    public void DeletSlime(int ID)
    {
        int iter = 0;
        while(iter < slimeCount)
        {
            if (allSlimes[iter].GetSlimeID() == ID) break;
            iter++;
        }
        allSlimes.RemoveAt(iter);
        slimeCount--;
    }

    public Slime GetSlime()
    {
        return _slime;
    }

}
