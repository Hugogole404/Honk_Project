using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EcureilNextPos : MonoBehaviour
{
    public Lapin_Nav[] ecureuils;
    public Transform nextStop;
    private void OnTriggerEnter(Collider other)
    {
        foreach (var item in ecureuils)
        {
            item.player = nextStop;
            item.stopchecking = true;
        }
    }
}
