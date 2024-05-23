using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightIntensityController : MonoBehaviour
{
    public Light directionalLight;
    public Collider inCollider;
    public Collider outCollider;

    public float maxIntensity = 1.0f;
    public float minIntensity = 0.0f;
    public float intensityChangeSpeed = 1.0f;

    private bool isInside = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other == inCollider)
        {
            isInside = true;
            Debug.Log("aie");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == outCollider && isInside == true)
        {
            isInside = false;
        }
    }

    private void Update()
    {
        if (isInside)
        {
            if (directionalLight.intensity < maxIntensity)
            {
                directionalLight.intensity += intensityChangeSpeed * Time.deltaTime;
                directionalLight.intensity = Mathf.Min(directionalLight.intensity, maxIntensity);
            }
        }
        else
        {
            if (directionalLight.intensity > minIntensity && isInside == true)
            {
                directionalLight.intensity -= intensityChangeSpeed * Time.deltaTime;
                directionalLight.intensity = Mathf.Max(directionalLight.intensity, minIntensity);
            }
        }
    }
}

