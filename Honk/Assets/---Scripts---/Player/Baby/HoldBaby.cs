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

    [HideInInspector] public bool CanHoldBaby = false;
    [HideInInspector] public bool IsOnHisBack;

    private void Start()
    {
        IsOnHisBack = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Baby>() != null)
        {
            CanHoldBaby = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Baby>() != null)
        {
            CanHoldBaby = false;
        }
    }
}