using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiftBaby : MonoBehaviour
{
    [SerializeField] private GameObject _pointEnterRift;
    private HoldBaby _holdBaby;

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.GetComponent<Baby>() != null && _holdBaby.IsOnHisBack == false)
        {
            // lancer l'anim où il rentre 
            // deplacer la cam 
            other.gameObject.transform.position = _pointEnterRift.transform.position;
            // lancer l'anim ou il ressort 
        }
    }
    private void Start()
    {
        _holdBaby = FindAnyObjectByType<HoldBaby>();
    }
}