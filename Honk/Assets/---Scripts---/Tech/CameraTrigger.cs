using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTrigger : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera Cam1, Cam2, Cam3, Cam4;
    [SerializeField] private Collider Collider2, Collider3, Collider4;
    [SerializeField] private CinemachineBrain Brain;

    private void OnTriggerEnter(Collider other)
    {
        if (other == Collider2)
        {
            Debug.Log("InC2");
            Cam2.gameObject.SetActive(true);
        }   
        else if (other == Collider3)
        {
            Debug.Log("InC3");
            Cam3.gameObject.SetActive(true);
        }
        else if (other == Collider4)
        {
            Debug.Log("InC4");
            Cam4.gameObject.SetActive(true);
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other == Collider2)
        {
            Debug.Log("OutC2");
            Cam2.gameObject.SetActive(false);
        }
        else if (other == Collider3)
        {
            Debug.Log("OutC3");
            Cam3.gameObject.SetActive(false);
        }
        else if (other == Collider4)
        {
            Debug.Log("OutC4");
            Cam4.gameObject.SetActive(false);
        }
    }


}
    