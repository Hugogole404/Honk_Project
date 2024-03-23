using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCameraSwap : MonoBehaviour
{

    [SerializeField] private Collider Collider;
    [SerializeField] private Camera Camera;

    private void OnTriggerEnter(Collider other)
    {
 
            Debug.Log("In");
            Camera.gameObject.SetActive(false);
        
    }

    private void OnTriggerExit(Collider other)
    {

            Debug.Log("Out");
            Camera.gameObject.SetActive(true);

    }
}
