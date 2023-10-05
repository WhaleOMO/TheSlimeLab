using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoader : MonoBehaviour
{
    // Start is called before the first frame update
    public TextAsset mergeTree;
    public static List<int[]> mergeData = new List<int[]>();
    
    void Start()
    {
        LoadData();
    }

    // Update is called once per frame
    public void LoadData()
    {
        string[] mergeTreeRow = mergeTree.text.Split('\n');
        for (int i=0; i < mergeTreeRow.Length-1; i++)
        {
            string[] rowArray = mergeTreeRow[i+1].Split(',');
            int[] rowData = new int[rowArray.Length-1];
            for (int j = 0; j < rowArray.Length-1; j++)
            {
                rowData[j] = int.Parse(rowArray[j + 1]);
            }
            mergeData.Add(rowData);
        }

        //foreach (int[] test in mergeData)
        //{
        //    foreach (int number in test)
        //    {
        //        Debug.Log(number);
        //    }
        //}

    }
}
