using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDrop : MonoBehaviour
{
    public GameObject spike;
    public Collider CACA;

    private void OnTriggerEnter(Collider col)
    {
        Debug.Log("cc");
        spike.GetComponent<Rigidbody>().useGravity = true;
    }


}
