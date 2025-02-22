using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealbar : MonoBehaviour
{
    [SerializeField]Slider slider;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateHealbar(float totalHeal , float maxHeal)
    {
        slider.value = totalHeal/ maxHeal;
    }
}
