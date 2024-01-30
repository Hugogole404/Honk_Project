using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTrigger : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera Cam1, Cam2, Cam3;
    [SerializeField] private CinemachineBrain Brain;
    //[SerializeField] private Collider collider;


    private void OnTriggerEnter(Collider other)
    {
            Debug.Log("F");
            Cam2.gameObject.SetActive(true);
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Off");
        Cam2.gameObject.SetActive(false);
    }
}
    