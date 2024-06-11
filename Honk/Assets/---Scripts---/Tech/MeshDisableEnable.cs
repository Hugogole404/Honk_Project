using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDisableEnable : MonoBehaviour
{
    public void On()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = true;
    }

    public void Off()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
}
