using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyPutDetection : MonoBehaviour
{
    public bool CanBePut;

    private void Update()
    {
        Collider[] hitCollider = Physics.OverlapSphere(transform.position, 0f);
        if (hitCollider.Length == 2) CanBePut = true;
        else if (hitCollider.Length >= 2) CanBePut = false;
        foreach (Collider col in hitCollider)
        {
            print(col.gameObject.name);
        }
    }
}