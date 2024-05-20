using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyCrush : MonoBehaviour
{
    HoldBaby _holdBaby;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<TestBabyWalk>() != null)
        {
            if (transform.parent.gameObject.GetComponent<Rigidbody>().velocity.magnitude > 0.1f && _holdBaby.IsOnHisBack == false)
            {
                other.GetComponent<TestBabyWalk>().transform.localScale = new Vector3(other.GetComponent<TestBabyWalk>().transform.localScale.x,
                    other.GetComponent<TestBabyWalk>().ScaleCrushedBaby, other.GetComponent<TestBabyWalk>().transform.localScale.z);
            }
        }
        /// bloc entre eux 
        //if (other.GetComponent<PushObstacles>() != null)
        //{
        //    other.GetComponent<PushObstacles>()._bloc.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        //    print(other.GetComponent<PushObstacles>()._bloc.GetComponent<Rigidbody>().gameObject.name);
        //    GetComponentInParent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        //}
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<TestBabyWalk>() != null /*&& _holdBaby.IsOnHisBack == false*/)
        {
            other.gameObject.GetComponent<TestBabyWalk>().CanGoToNormalScale = true;
        }
    }

    private void Awake()
    {
        _holdBaby = FindObjectOfType<HoldBaby>();
    }
}