using UnityEngine;

public class AreaSwimming : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerMovements>() != null) 
        {
            //other.GetComponent<PlayerMovements>().IsSwimmingBools();
            Debug.Log("He is swimming");
        }
            
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerMovements>() != null)
        {
            //other.GetComponent<PlayerMovements>().IsWalkingBools();
            Debug.Log("He is not swimming");
        }
    }
}