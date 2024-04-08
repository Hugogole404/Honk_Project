using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDrop : MonoBehaviour
{
    public ConstantForce Spike;
    public GameObject fx;
    public float Speed;

    private void OnTriggerEnter(Collider other)
    {
        
        
        fx.gameObject.SetActive(true);
        Spike.force = new Vector3 (0,Speed * -1,0); 
    }


}
