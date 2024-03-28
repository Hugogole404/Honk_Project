using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Slope>() != null)
        {
            other.transform.position = other.GetComponent<Slope>().SpawnPoint.transform.position;
        }
        if (other.GetComponent<PlayerMovements>() != null)
        {
            other.GetComponent<CharacterController>().enabled = false;
            other.transform.position = other.GetComponent<PlayerMovements>().SpawnPoint.transform.position;
            other.transform.rotation = other.GetComponent<PlayerMovements>().PlayerOriginRotation;
            other.GetComponent<CharacterController>().enabled = true;
        }
    }
}