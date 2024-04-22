using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrotteEntree : MonoBehaviour
{
    public Collider honkCollider;
    public CinemachineVirtualCamera Camera;
    private float StartPos;
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if (other == honkCollider)
        {
            Camera.enabled = true;
        }
    }
}
