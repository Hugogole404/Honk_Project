using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    private OrcaMovements _orcaMov;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<OrcaMovements>() != null)
        {
            _orcaMov.HavePlayerAggro = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<OrcaMovements>() != null)
        {
            _orcaMov.HavePlayerAggro = false;
        }
    }

    private void Start()
    {
        _orcaMov = GetComponentInParent<OrcaMovements>();
    }
}