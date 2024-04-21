using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObstacles : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerMovements>() != null)
        {
            other.GetComponent<PlayerMovements>().CanPushObstacles = true;
            other.GetComponent<PlayerMovements>().ActualObstacle = gameObject.transform.parent.transform.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerMovements>() != null)
        {
            other.GetComponent<PlayerMovements>().CanPushObstacles = false;
        }
    }
}