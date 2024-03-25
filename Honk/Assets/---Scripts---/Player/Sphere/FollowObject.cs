using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] GameObject _objectToFollow;

    private void Update()
    {
        transform.position = new Vector3(_objectToFollow.transform.position.x, _objectToFollow.transform.position.y + 0.5f, _objectToFollow.transform.position.z);
    }
}