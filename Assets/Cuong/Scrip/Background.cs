using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public Transform mainCam;
    public Transform Br1;
    public Transform Br2;
    public float length;


    // Update is called once per frame
    void Update()
    {
        if (mainCam.position.x > Br1.position.x) {
            UpdateBackground(Vector3.right);
        } else if (mainCam.position.x < Br1.position.x) {
            UpdateBackground(Vector3.left);
        }
    }
    void UpdateBackground(Vector3 direction) {
        Br2.position = Br1.position + direction * length;
        Transform temp = Br1;
        Br1 = Br2;
        Br2 = temp;
    }
}
