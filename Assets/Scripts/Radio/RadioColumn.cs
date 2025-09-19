using System;
using UnityEngine;
using UnityEngine.UI;

public class RadioColumn : MonoBehaviour
{
    [SerializeField] 
    private Slider columnSlider;

    public void SetValue(float x)
    {
        columnSlider.value = x;
    }
}
