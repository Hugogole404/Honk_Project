using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwapTexture : MonoBehaviour
{
    public Material Clay;
    public string PropertyName;
    private bool OnOff;
    public float Speed;
    private float Current;
   

    void Start()
    {
   
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Current < Speed) {Current += Time.deltaTime;}

        if (Current >= Speed) 
        { 
            Current = 0;
            if (OnOff) { Clay.SetFloat(PropertyName, 0); OnOff = false; }
            else { Clay.SetFloat(PropertyName, 1); OnOff = true; }
        }

    }
}
