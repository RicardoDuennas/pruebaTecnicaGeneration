using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraScore : MonoBehaviour
{
    public Image PorcentageVida;
    public float porcentajeMax;
    public float porcentajeMin;
    public Grid getLandManager;

    void Start()
    {
        // Asignar la referencia del LandManager si no se ha hecho en el Inspector
        if (getLandManager == null)
        {
            getLandManager = FindObjectOfType<Grid>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (getLandManager != null)
        {
            // Obtener el porcentaje de tierra del LandManager
            float landPercentage = getLandManager.getLandPercentage();

            // Usar ese valor para actualizar la barra
            PorcentageVida.fillAmount = landPercentage;
        }
        else
        {
            Debug.LogError("LandManager no asignado.");
        }



       
    }
}
