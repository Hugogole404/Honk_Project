using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FeetPlacement : MonoBehaviour
{
    public Object PasDumanchot;
    public bool isSnow;
    int layerMask = 1 << 11;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 2f, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
            isSnow = true;
        }
        else
            isSnow = false;
}

    public void TriggerEvent()
    {

        if (isSnow)
        {
            Instantiate(PasDumanchot, gameObject.transform.position + new Vector3(0.4f, 0.2f, 0), gameObject.transform.rotation * Quaternion.Euler(Vector3.right * 90) * Quaternion.Euler(Vector3.forward));
        }
       
    }
    public void TriggerEvent2()
    {
        if (isSnow)
        {
            Instantiate(PasDumanchot, gameObject.transform.position + new Vector3(-0.4f, 0.2f, 0), gameObject.transform.rotation * Quaternion.Euler(Vector3.right * 90) * Quaternion.Euler(Vector3.forward));
        }
    }


    public void AtterissageNeige()
    {
        if (isSnow)
        {
            Instantiate(PasDumanchot, gameObject.transform.position + new Vector3(-0.3f, 0.2f, 0), gameObject.transform.rotation * Quaternion.Euler(Vector3.right * 90) * Quaternion.Euler(Vector3.forward));
            Instantiate(PasDumanchot, gameObject.transform.position + new Vector3(0.3f, 0.2f, 0), gameObject.transform.rotation * Quaternion.Euler(Vector3.right * 90) * Quaternion.Euler(Vector3.forward));
        }
    }
}
