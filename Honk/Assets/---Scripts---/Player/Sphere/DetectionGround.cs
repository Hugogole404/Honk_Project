using UnityEngine;

public class DetectionGround : MonoBehaviour
{
    [SerializeField] private Slope _slope;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Walkable>() != null)
        {
            _slope.IsGrounded = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Walkable>() != null)
        {
            _slope.IsGrounded = false;
        }
    }
}