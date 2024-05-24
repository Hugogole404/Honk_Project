using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PushObstacles : MonoBehaviour
{
    public GameObject _bloc;
    public GameObject ParentBabyAfterDeath;
    [SerializeField] private GameObject _top;
    [SerializeField] private GameObject _parentBaby;
    [SerializeField] private float _timerMaxGetBaby;
    [SerializeField] private float _currentTimerGetBaby;
    [SerializeField] GameObject _parentInBloc;
    private bool _isOnCube;
    private float _maxTimerGravity = 2;
    private float _currentTimerGravity = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovements>() != null)
        {
            other.GetComponent<PlayerMovements>().CanPushObstacles = true;
            other.GetComponent<PlayerMovements>().ActualObstacle = _bloc;
        }
        if(other.GetComponent<TestBabyWalk>() != null && other.gameObject.transform.position.y >= _top.transform.position.y)
        {
            other.GetComponent<TestBabyWalk>().SetGravityBaby = 0;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerMovements>() != null)
        {
            other.GetComponent<PlayerMovements>().CanPushObstacles = false;
        }
        if (other.GetComponent<TestBabyWalk>() != null)
        {
            _currentTimerGetBaby = 0;
            other.GetComponent<TestBabyWalk>().gameObject.transform.parent = _parentBaby.transform;
            _isOnCube = false;
            other.GetComponent<TestBabyWalk>().SetGravityBaby = 1;
            // Rescale du petit
            //other.GetComponent<TestBabyWalk>().transform.localScale = other.GetComponent<TestBabyWalk>().BaseScaleBaby;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerMovements>() != null)
        {
            if (GetComponentInParent<Rigidbody>().velocity.magnitude < 0.0001f)
                other.GetComponent<PlayerMovements>().CanPushObstacles = true;

            other.GetComponent<PlayerMovements>().ActualObstacle = gameObject.transform.parent.transform.gameObject;
        }
        if (other.GetComponent<TestBabyWalk>() != null)
        {
            if (other.GetComponent<TestBabyWalk>().transform.position.y > _top.transform.position.y)
            {
                other.GetComponent<TestBabyWalk>().SetGravityBaby = 0;
                _currentTimerGetBaby += Time.deltaTime;
            }
            if (_currentTimerGetBaby >= _timerMaxGetBaby)
            {
                other.GetComponent<TestBabyWalk>().gameObject.transform.parent = _parentInBloc.transform;
                _isOnCube = true;

                //Rescale du petit
                //other.GetComponent<TestBabyWalk>().transform.localScale = other.GetComponent<TestBabyWalk>().BaseScaleBaby;
            }
        }
    }
    private void Start()
    {
        ParentBabyAfterDeath = FindObjectOfType<BabyParentInDad>().gameObject;
        _maxTimerGravity = 2;
    }
    private void Update()
    {
        _currentTimerGravity += Time.deltaTime;

        if (_currentTimerGravity >= _maxTimerGravity)
        {
            _currentTimerGravity = _maxTimerGravity;
            GetComponentInParent<Rigidbody>().useGravity = false;
        }
        if (_isOnCube)
        {
            //_baby.GetComponent<Rigidbody>().velocity = _bloc.GetComponent<Rigidbody>().velocity;
        }
    }
}