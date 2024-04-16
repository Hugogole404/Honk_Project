using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;

public class Baby : MonoBehaviour
{
    public List<Vector3> _lastPositionPlayer;
    private List<Vector3> _lastPositionPlayerCopy;
    private Transform _toFollow;

    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _speed;

    private int _point;
    private bool _isDadMoving;
    private Vector3 _oldPosPlayer;
    private HoldBaby _holdBaby;
    private PlayerMovements _playerMov;

    private void CheckIsPlayerMoving()
    {
        if (_oldPosPlayer.x != _playerMov.transform.position.x || _oldPosPlayer.z != _playerMov.transform.position.z)
        {
            _isDadMoving = true;
        }
        else
        {
            _isDadMoving = false;
        }
        _oldPosPlayer = _playerMov.transform.position;
    }
    private void UpdateLists()
    {
        if(_isDadMoving && _holdBaby.IsOnHisBack == false)
        {
            _lastPositionPlayer.Add(_playerMov.gameObject.transform.position);
        }
        if(_holdBaby.IsOnHisBack == true)
        {
            _lastPositionPlayer.Clear();
            _point = 0;
        }

        //foreach (var item in _lastPositionPlayer)
        //{
        //    if(item != null)
        //    {
        //        _lastPositionPlayerCopy.Add(item);
        //    }
        //}
        //_lastPositionPlayer.Clear();
        //_lastPositionPlayer = _lastPositionPlayerCopy;
        //_lastPositionPlayerCopy.Clear();
        //_point = 0;
    }
    private void MoveBaby()
    {
        if (_holdBaby.IsOnHisBack == false)
        {
            //transform.position = ToFollow.position - _offset;
            //transform.rotation = ToFollow.rotation;
            foreach (var item in _lastPositionPlayer)
            {
                transform.position = item;
            }
            transform.position = _lastPositionPlayer[_point];
            _point++;
            //_lastPositionPlayer.Remove(_lastPositionPlayer[_point]);
            UpdateLists();
        }
    }
    private void Awake()
    {
        _holdBaby = FindAnyObjectByType<HoldBaby>();
        _playerMov = FindAnyObjectByType<PlayerMovements>();
    }
    void Start()
    {
        _toFollow = _playerMov.gameObject.transform;
        _offset = _toFollow.position - transform.position;
        _oldPosPlayer = _playerMov.gameObject.transform.position;
    }
    private void Update()
    {
        UpdateLists();
        MoveBaby();

        //Le petit bouge et il reproduit le déplacement de son papa. 
    }
    private void FixedUpdate()
    {
        CheckIsPlayerMoving();
    }
}