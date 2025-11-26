using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointsLoader : MonoBehaviour
{

    public Transform[] SpawnPoints; 

    public static SpawnPointsLoader Instance;

    private void Awake()
    {
        Instance = this;
    }

}
