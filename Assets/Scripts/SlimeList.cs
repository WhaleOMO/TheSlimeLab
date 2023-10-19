using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Slime List", menuName ="SlimeList")]
public class SlimeList : ScriptableObject
{
    public string[] slimeName;

    public GameObject[] slimePrefab;
}
