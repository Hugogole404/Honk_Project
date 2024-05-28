using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTrigger : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera Cam;
    [SerializeField] private Collider Player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovements>() != null)
        {
            Debug.Log("fix");
            Cam.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerMovements>() != null)
        {
            Debug.Log("InCamSwap");
            Cam.gameObject.SetActive(false);
        }
    }
}