using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static Singleton Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void CreateClone(GameObject prefab, Transform pointToSpawn)
    {
        Instantiate(prefab, pointToSpawn.position, Quaternion.identity);
    }
}
