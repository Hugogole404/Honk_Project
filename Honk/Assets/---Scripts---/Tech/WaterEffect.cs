using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterEffect : MonoBehaviour
{
    public GameObject CameraWater;
    private void OnTriggerEnter(Collider other)
    {
        CameraWater.SetActive(true);
    }
    private void OnTriggerExit(Collider other)
    {
        CameraWater.SetActive(false);
    }
}
