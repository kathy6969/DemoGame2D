using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Br : MonoBehaviour
{
    public Transform mainCam;
    public Transform midBr;
    public Transform sideBr;
    public float length;
    void Update()
    {
        if (mainCam.position.x > midBr.position.x) {
            UpdateBackgroundPosition(Vector3.right);
        }
        else if (mainCam.position.x<midBr.position.x) {
            UpdateBackgroundPosition(Vector3.left);
        }
    }
    void UpdateBackgroundPosition(Vector3 direction) { 
        sideBr.position = midBr.position + direction * length;
        Transform temp = midBr;
        midBr = sideBr;
        sideBr = temp;
    }
}
