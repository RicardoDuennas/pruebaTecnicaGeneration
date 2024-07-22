using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    private Renderer objRenderer;

    private void Start() => objRenderer = GetComponent<Renderer>();

    public void ChangeColor()
    {
        Color newColor = CreateRandomColor();
        objRenderer.material.SetColor("_Color", newColor);
    }

    private Color CreateRandomColor() => new(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));

}
