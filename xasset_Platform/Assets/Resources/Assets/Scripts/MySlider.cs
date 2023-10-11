using UnityEngine;
using System;
public class MySlider : MonoBehaviour
{
    [Range(0.0f,1.0f)]
    public float value=0.0f;

    private _Slider_ slider;
    private void Awake()
    {
        slider = transform.GetComponentInChildren<_Slider_>();
    }
    private void OnEnable()
    {
        value = 0.0f;
    }
    private void FixedUpdate()
    {
        slider.Value = value;
    }
}
