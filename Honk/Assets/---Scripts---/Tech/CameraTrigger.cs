using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTrigger : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera Cam1, Cam2, Cam3;
    [SerializeField] private CinemachineBrain Brain;
    [SerializeField] private Collider collider;
    bool tt = true;

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("trigger");
        if (tt == true)
        {
            Debug.Log("T");
            Cam2.gameObject.SetActive(true);
            tt = false;
        }
        else
        {
            Debug.Log("F");
            Cam2.gameObject.SetActive(false);
            tt = true;
        }

    }
}
    