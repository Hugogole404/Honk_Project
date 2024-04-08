using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] GameObject _objectToFollow;

    private void Update()
    {
        //GetComponent<CharacterController>().enabled = false;
        transform.position = new Vector3(_objectToFollow.transform.position.x, _objectToFollow.transform.position.y + 0.5f, _objectToFollow.transform.position.z);
        //GetComponent<CharacterController>().enabled = true;
        //transform.eulerAngles = new Vector3(transform.eulerAngles.x, _objectToFollow.transform.eulerAngles.y, transform.eulerAngles.z);
    }
}