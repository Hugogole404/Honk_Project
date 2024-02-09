using UnityEngine;

public class AreaSwimming : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerMovements>() != null) 
        {
            other.GetComponent<PlayerMovements>().IsSwimmingBools();
        }
            
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerMovements>() != null)
        {
            other.GetComponent<PlayerMovements>().IsWalkingBools();
        }
    }
}