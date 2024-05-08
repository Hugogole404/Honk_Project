using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissiveControl : MonoBehaviour
{
    public float maxDistance = 10f;
    public float maxEmission = 5f;

    private Renderer rend;
    private Material emissiveMaterial;
    private Transform playerTransform;

    void Start()
    {
        rend = GetComponent<Renderer>();
        emissiveMaterial = rend.material; 
        GameObject playerObject = GameObject.Find("PlayerWalkModif");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player object not found with the given name: ");
            enabled = false;
        }
    }

    void Update()
    {
        if (playerTransform == null)
            return;

        float distance = Vector3.Distance(transform.position, playerTransform.position);
        if (distance > maxDistance)
        {
            emissiveMaterial.SetColor("_EmissionColor", Color.black); 
        }
        else
        {
            float emission = Mathf.Lerp(0f, maxEmission, 1f - (distance / maxDistance)); 
            emissiveMaterial.SetColor("_EmissionColor", Color.white * emission);
        }
        rend.material = emissiveMaterial;
        rend.material.EnableKeyword("_EMISSION");
    }
}
