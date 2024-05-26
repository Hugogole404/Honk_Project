using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lapin_TutoBreach : MonoBehaviour
{
    public Ecureuil[] ecureuil;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        foreach (var item in ecureuil)
        {
            item.GoTo();
        } 
    }

    // Update is called once per frame

}
