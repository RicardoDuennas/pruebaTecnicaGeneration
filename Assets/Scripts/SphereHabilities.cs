using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereHabilities : MonoBehaviour
{
    public GameObject clone;
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Singleton.Instance.CreateClone(clone, clone.transform.GetChild(0));
        }
    }
}
