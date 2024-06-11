using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTakeBaby : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerMovements>() != null)
        {
            other.gameObject.GetComponent<PlayerMovements>().TakeBaby();
        }
    }
}