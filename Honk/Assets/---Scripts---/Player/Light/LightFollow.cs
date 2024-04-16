using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFollow : MonoBehaviour
{
    public GameObject whatItFollows;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3 (whatItFollows.transform.position.x, whatItFollows.transform.position.y, whatItFollows.transform.position.z - 8);
    }
}
