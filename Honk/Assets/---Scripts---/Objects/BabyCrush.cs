using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyCrush : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<TestBabyWalk>() != null)
        {
            if (transform.parent.gameObject.GetComponent<Rigidbody>().velocity.magnitude > 0.1f)
            {
                Debug.Log("CRUSH"); ;
                other.GetComponent<TestBabyWalk>().transform.localScale = new Vector3(other.GetComponent<TestBabyWalk>().transform.localScale.x,
                    other.GetComponent<TestBabyWalk>().ScaleCrushedBaby, other.GetComponent<TestBabyWalk>().transform.localScale.z);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<TestBabyWalk>() != null)
        {
            other.gameObject.GetComponent<TestBabyWalk>().CanGoToNormalScale = true;
        }
    }
}