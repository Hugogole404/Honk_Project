using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDrop : MonoBehaviour
{
    public ConstantForce Spike;
    public GameObject FX_drop;
    public float Speed;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("pipi");
        //FX_drop.gameObject.SetActive(true);
        Spike.force = new Vector3 (0,Speed * -1,0); 
    }

    


}
