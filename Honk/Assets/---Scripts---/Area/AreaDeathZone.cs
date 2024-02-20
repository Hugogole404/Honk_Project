using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovements>() != null)
        {
            other.GetComponent<CharacterController>().enabled = false;
            other.transform.position = other.GetComponent<PlayerMovements>().SpawnPoint.transform.position;
            other.GetComponent<CharacterController>().enabled = true;
        }
    }
}