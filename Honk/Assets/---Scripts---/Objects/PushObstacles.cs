using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObstacles : MonoBehaviour
{
    private float _maxTimer = 2;
    private float _currentTimer = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovements>() != null)
        {
            other.GetComponent<PlayerMovements>().CanPushObstacles = true;
            other.GetComponent<PlayerMovements>().ActualObstacle = gameObject.transform.parent.transform.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerMovements>() != null)
        {
            other.GetComponent<PlayerMovements>().CanPushObstacles = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerMovements>() != null)
        {
            other.GetComponent<PlayerMovements>().CanPushObstacles = true;
            other.GetComponent<PlayerMovements>().ActualObstacle = gameObject.transform.parent.transform.gameObject;
        }
    }
    private void Start()
    {
        _maxTimer = 2;
    }
    private void Update()
    {
        _maxTimer += Time.deltaTime;
        if (_currentTimer >= _maxTimer)
        {
            _currentTimer = _maxTimer;
            GetComponentInParent<Rigidbody>().useGravity = false;
        }
    }
}