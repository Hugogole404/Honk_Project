using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentingObj : MonoBehaviour
{
    public GameObject child;
    public GameObject parent;
    // Start is called before the first frame update

    public void parenting()
    {
        child.transform.parent = parent.transform;
        gameObject.transform.localPosition = new Vector3(-0.0011f, -0.0214f, -0.0319f);
        gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }
}
