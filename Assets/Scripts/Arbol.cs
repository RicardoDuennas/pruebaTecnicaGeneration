using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arbol : MonoBehaviour
{
    public bool tieneProteccion = false;
    public int dañoProteccion = 5;
    public int vidaArbol = 50;

    public void RecibirDaño(int cantidad)
    {
        vidaArbol -= cantidad;
        if (vidaArbol <= 0)
        {
            Destroy(gameObject);
        }
    }
}


