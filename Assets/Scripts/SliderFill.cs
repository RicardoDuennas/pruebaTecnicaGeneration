using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderFill : MonoBehaviour
{
    public Grid grid;
    Slider _slider;

    private void Start()
    {
        _slider = GetComponent<Slider>();
        _slider.minValue = 0f;
        _slider.maxValue = 160f;
    }

    private void Update()
    {

        FillSlider();

    }

    void FillSlider()
    {
        _slider.value = grid.getLandPercentage();
        Debug.Log(grid.getLandPercentage());
    }

    void ResetSlider()
    {
        _slider.value = _slider.minValue;
    }
}