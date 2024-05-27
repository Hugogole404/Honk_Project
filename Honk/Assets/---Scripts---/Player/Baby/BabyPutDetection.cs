using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BabyPutDetection : MonoBehaviour
{
    public bool CanBePut;
    public int NumberMinOfColliderToDetect;
    private void Update()
    {
        Collider[] hitCollider = Physics.OverlapSphere(transform.position, 0f);
        if (hitCollider.Length == NumberMinOfColliderToDetect) CanBePut = true;
        else if (hitCollider.Length >= NumberMinOfColliderToDetect) CanBePut = false;
        foreach (Collider col in hitCollider)
        {
            //print(col.gameObject.name);
            print(hitCollider.Length);
        }
    }
}