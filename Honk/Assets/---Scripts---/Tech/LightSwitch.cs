using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    private Light light;
    public float timeFlash;
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timeFlash >= 0)
        {
            
            timeFlash -= 0.1f;
            
        }
        else
        {
            if (light.enabled)
                light.enabled = false;
            else
                light.enabled = true;
            timeFlash = Random.Range(1f, 15f);
        }
            
    }
}
