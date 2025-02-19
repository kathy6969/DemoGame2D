using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealbar : MonoBehaviour
{
    [SerializeField]private Slider slider;
    public void UpdateHealbar(float totalHeal , float maxHeal)
    {
        slider.value = totalHeal/ maxHeal;
    }
}