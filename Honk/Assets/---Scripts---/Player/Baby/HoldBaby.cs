using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldBaby : MonoBehaviour
{
    [Header("Hold baby penguin")]
    public GameObject ParentObjectBaby;
    public GameObject PositionBabyPut;
    public GameObject Baby;
    public GameObject BasePositionBaby;

    public bool CanHoldBaby = false;
    public bool IsOnHisBack;

    private void Start()
    {
        IsOnHisBack = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<TestBabyWalk>() != null && IsOnHisBack == false)
        {
            CanHoldBaby = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<TestBabyWalk>() != null && IsOnHisBack == false)
        {
            CanHoldBaby = false;
        }
    }
}