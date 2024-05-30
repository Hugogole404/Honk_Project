using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BabyCrush : MonoBehaviour
{
    HoldBaby _holdBaby;
    PlayerMovements _playerMovements;
    [SerializeField] float _minValToCalculateVelocity;
    /// <summary>
    /// CRUSH 
    /// </summary>

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.GetComponent<TestBabyWalk>() != null)
    //    {
    //        if (transform.parent.gameObject.GetComponent<Rigidbody>().velocity.magnitude > 0.1f && _holdBaby.IsOnHisBack == false)
    //        {
    //            other.GetComponent<TestBabyWalk>().transform.localScale = new Vector3(other.GetComponent<TestBabyWalk>().transform.localScale.x,
    //                other.GetComponent<TestBabyWalk>().ScaleCrushedBaby, other.GetComponent<TestBabyWalk>().transform.localScale.z);
    //        }
    //    }
    //    /// bloc entre eux 
    //    if (other.GetComponent<PushObstacles>() != null)
    //    {
    //        //print(other.GetComponent<PushObstacles>()._bloc.GetComponent<Rigidbody>().gameObject.name);
    //        Rigidbody rbParent = GetComponentInParent<Rigidbody>();

    //        //if (rbParent.velocity.x < 0)
    //        //    GetComponentInParent<Rigidbody>().gameObject.transform.position -= new Vector3(-1, 0, 0);

    //        //if (rbParent.velocity.x > 0)
    //        //{
    //        //    print("OUI");
    //        //    GetComponentInParent<Rigidbody>().gameObject.transform.position -= new Vector3(50, 0, 0);
    //        //}

    //        other.GetComponent<PushObstacles>()._bloc.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

    //        GetComponentInParent<Rigidbody>().velocity = new Vector3(0, 0, 0);
    //    }
    //}
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.GetComponent<PushObstacles>() != null)
    //    {
    //        other.GetComponent<PushObstacles>()._bloc.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
    //        GetComponentInParent<Rigidbody>().velocity = new Vector3(0, 0, 0);
    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.GetComponent<TestBabyWalk>() != null /*&& _holdBaby.IsOnHisBack == false*/)
    //    {
    //        other.gameObject.GetComponent<TestBabyWalk>().CanGoToNormalScale = true;
    //    }
    //}


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<TestBabyWalk>() != null && GetComponentInParent<Rigidbody>().velocity.magnitude > _minValToCalculateVelocity)
        {
            _playerMovements.TakeBaby(); 
        }
    }

    private void Awake()
    {
        _holdBaby = FindObjectOfType<HoldBaby>();
        _playerMovements = FindObjectOfType<PlayerMovements>();
    }
}