using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class EmissiveColider : MonoBehaviour
{
    private Material emissiveMaterial;
    private float Time = 0;
    private bool InOut = false;
    public float EaseIn = 0.05f;
    public float EaseOut = 0.01f;
    public float EmissiveAmount = 5;
    private Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        emissiveMaterial = rend.material;

    }
    private void Update()
    {
        if (InOut == true && Time<= EmissiveAmount)
        {
            
            Time += EaseIn;
            
            emissiveMaterial.SetColor("_EmissionColor", Color.white * Time);
            rend.material = emissiveMaterial;
            rend.material.EnableKeyword("_EMISSION");
        }
        else if (InOut == false && Time>0) 
        {
            Time -= EaseOut;
            emissiveMaterial.SetColor("_EmissionColor", Color.white * Time);
            rend.material = emissiveMaterial;
            rend.material.EnableKeyword("_EMISSION");

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        InOut = true;

    }

    private void OnTriggerExit(Collider other)
    {
        InOut = false;
    }
}
