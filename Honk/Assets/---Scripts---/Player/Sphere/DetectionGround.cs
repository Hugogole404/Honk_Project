using UnityEngine;

public class DetectionGround : MonoBehaviour
{
    [SerializeField] private Slope _slope;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Walkable>() != null)
        {
            _slope.IsGrounded = true;
            _slope.m_Animator.SetBool("IsJumping", false);                                  //J'ai rajouté ça (Adam)
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Walkable>() != null)
        {
            _slope.IsGrounded = false;
            _slope.m_Animator.SetBool("IsJumping", true);                                  //J'ai rajouté ça (Adam)
        }
    }
}