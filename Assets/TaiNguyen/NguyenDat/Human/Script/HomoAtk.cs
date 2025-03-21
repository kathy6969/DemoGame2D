using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomoAtk : MonoBehaviour
{
    public GameObject Atkpoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Atk()
    {
        Atkpoint.SetActive(true);
    }
    public void StopAtk()
    {
        Atkpoint.SetActive(false);
    }
}
