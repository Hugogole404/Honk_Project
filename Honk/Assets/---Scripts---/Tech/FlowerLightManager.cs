using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerLightManager : MonoBehaviour
{
    public Light targetLight;
    public Collider triggerCollider;
    private float totalEmissive = 0f;

    private void OnTriggerEnter(Collider other)
    {
        Renderer rend = other.GetComponent<Renderer>();
        if (rend != null && rend.material != null && rend.material.IsKeywordEnabled("_EMISSION"))
        {
            totalEmissive += rend.material.GetColor("_EmissionColor").maxColorComponent;
            UpdateLightIntensity();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Renderer rend = other.GetComponent<Renderer>();
        if (rend != null && rend.material != null && rend.material.IsKeywordEnabled("_EMISSION"))
        {
            totalEmissive -= rend.material.GetColor("_EmissionColor").maxColorComponent;
            UpdateLightIntensity();
        }
    }

    private void UpdateLightIntensity()
    {
        targetLight.intensity = totalEmissive;
    }
}





