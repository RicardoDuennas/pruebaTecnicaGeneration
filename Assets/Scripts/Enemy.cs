using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    void Start()
    {
                
    }

    private void Attack()
    {
        AudioManager.Instance.PlayMusic(1);
    }

    private void OnDisable()
    {
        AudioManager.Instance.PlayMusic(0);
    }


}
