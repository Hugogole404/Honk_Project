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

    public Collider CollideroutColliderPointLight;
    public float PointMinIntensity = 0;
    public Light PointLight;
    public float PointintensityChangeSpeed = 1.0f;

    private bool isOn =false;
    private bool isInside = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other == inCollider)
        {
            isInside = true;
            Debug.Log("IN");
            directionalLight.intensity = minIntensity;
            PointLight.intensity = 10f;
            isOn = true;
        }

        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == outCollider && isInside == true)
        {
            isInside = false;
        }
        if (other == CollideroutColliderPointLight)
        {
            isOn = false;
        }
    }

    

    private void Update()
    {
        if (directionalLight.intensity < maxIntensity && isInside == false)
        {
            directionalLight.intensity += intensityChangeSpeed * Time.deltaTime;
            directionalLight.intensity = Mathf.Min(directionalLight.intensity, maxIntensity);
        }

        if (PointLight.intensity > PointMinIntensity && isOn == false)
        {
            PointLight.intensity -= PointintensityChangeSpeed * Time.deltaTime;
            PointLight.intensity = Mathf.Max(PointLight.intensity, PointMinIntensity);
        }
    }
}

