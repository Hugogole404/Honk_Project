using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [HideInInspector] public bool CanTimerGetBaby;
    [HideInInspector] public bool IsOnCube;
    private TestBabyWalk _baby;
    private PlayerMovements _playerMovements;

    private void CheckVelocity()
    {
        if (GetComponentInParent<Rigidbody>() != null)
        {
            if (GetComponentInParent<Rigidbody>().velocity.magnitude <= 0.001f)
            {
                GetComponentInParent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            }
        }
    }
    private void CheckTimerToGetBaby()
    {
        if (_playerMovements.CanBabyFollow == false && CanTimerGetBaby)
        {
            _currentTimerGetBaby += Time.deltaTime;
            if (_currentTimerGetBaby >= _timerMaxGetBaby)
            {
                IsOnCube = true;
                _baby.GetComponent<CharacterController>().enabled = false;
                _baby.transform.parent = _parentInBloc.transform;
                CanTimerGetBaby = false;
                _currentTimerGetBaby = 0;
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
            CanTimerGetBaby = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<TestBabyWalk>() != null)
        {
            other.GetComponent<TestBabyWalk>().SetGravityBaby = 1;
            CanTimerGetBaby = false;
            _currentTimerGetBaby = 0;
            IsOnCube = false;
        }
    }
    private void Awake()
    {
        _baby = FindObjectOfType<TestBabyWalk>();
        _playerMovements = FindObjectOfType<PlayerMovements>();
    }
    private void Update()
    {
        CheckTimerToGetBaby();
        CheckVelocity();
    }
}