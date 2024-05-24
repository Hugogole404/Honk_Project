using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewPushObject : MonoBehaviour
{
    public GameObject Bloc;
    public GameObject ParentBabyAfterDeath;
    [SerializeField] private GameObject _parentBaby;
    [SerializeField] private GameObject _top;
    [SerializeField] private float _timerMaxGetBaby;
    [SerializeField] GameObject _parentInBloc;
    private float _currentTimerGetBaby;
    private bool _canTimerGetBaby;
    private bool _isOnCube;
    private TestBabyWalk _baby;

    private void CheckTimerToGetBaby()
    {
        if (_canTimerGetBaby)
        {
            _currentTimerGetBaby += Time.deltaTime;
            if(_currentTimerGetBaby >= _timerMaxGetBaby)
            {
                _isOnCube = true;
                _baby.GetComponent<CharacterController>().enabled = false;
                _baby.transform.parent = _parentInBloc.transform;
                //_baby.GetComponent<CharacterController>().enabled = true;
                print("OUI");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovements>() != null)
        {
            other.GetComponent<PlayerMovements>().CanPushObstacles = true;
            other.GetComponent<PlayerMovements>().ActualObstacle = Bloc;
        }
        if (other.GetComponent<TestBabyWalk>() != null && other.gameObject.transform.position.y >= _top.transform.position.y)
        {
            other.GetComponent<TestBabyWalk>().SetGravityBaby = 0;
            _canTimerGetBaby = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<TestBabyWalk>() != null)
        {
            other.GetComponent<TestBabyWalk>().SetGravityBaby = 1;
            _canTimerGetBaby = false;
            _currentTimerGetBaby = 0;
            _isOnCube = false;
        }
    }
    private void Awake()
    {
     _baby = FindObjectOfType<TestBabyWalk>();   
    }
    private void Update()
    {
        CheckTimerToGetBaby();   
    }
}