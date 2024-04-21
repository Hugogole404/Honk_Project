using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] GameObject _objectToFollow;
    public float OffsetY = 0;
    public float OffsetZ = 0;

    private void Update()
    {
        //GetComponent<CharacterController>().enabled = false;
        transform.position = new Vector3(_objectToFollow.transform.position.x , _objectToFollow.transform.position.y + OffsetY, _objectToFollow.transform.position.z + OffsetZ);
        //GetComponent<CharacterController>().enabled = true;
        //transform.eulerAngles = new Vector3(transform.eulerAngles.x, _objectToFollow.transform.eulerAngles.y, transform.eulerAngles.z);
    }
}