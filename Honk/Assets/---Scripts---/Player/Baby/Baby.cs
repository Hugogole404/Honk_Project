using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Baby : MonoBehaviour
{
    public Vector3 Offset;
    public List<Vector3> LastPositionPlayer;

    [SerializeField] private float _speed;

    private int _point;
    private bool _isDadMoving;
    private Vector3 _oldPosPlayer;
    private Transform _toFollow;
    private HoldBaby _holdBaby;
    private PlayerMovements _playerMov;

    private void CheckIsPlayerMoving()
    {
        if (_oldPosPlayer.x != _playerMov.transform.position.x || _oldPosPlayer.z != _playerMov.transform.position.z)
            _isDadMoving = true;
        else
            _isDadMoving = false;
        _oldPosPlayer = _playerMov.transform.position;
    }
    private void UpdateLists()
    {
        if (_isDadMoving && _holdBaby.IsOnHisBack == false)
        {
            //LastPositionPlayer.Add(_playerMov.gameObject.transform.position + Offset);
            LastPositionPlayer.Add(_playerMov.gameObject.transform.position);
        }
        if (_holdBaby.IsOnHisBack == true)
        {
            LastPositionPlayer.Clear();
            _point = 0;
        }
    }
    private void OffsetMovBaby()
    {
        //Offset.z = LastPositionPlayer[_point].z - LastPositionPlayer[_point - 1].z;
        //Offset.x = LastPositionPlayer[_point].x - LastPositionPlayer[_point - 1].x;
    }
    private void MoveBaby()
    {
        // calculer la différence de LastPlaterPos[X] et de LastPosPlayer[X-1]
        // l'ajouter au vector du player 

        //if (_isDadMoving == false)
        //{
        //    LastPositionPlayer.Clear();
        //    _point = 0;
        //}
        if (_holdBaby.IsOnHisBack == false && _isDadMoving)
        {
            foreach (var item in LastPositionPlayer)
            {
                Offset = _playerMov.gameObject.transform.position - LastPositionPlayer[_point];
                Offset.y = 0;
                //OffsetMovBaby();
                transform.position += Offset * 2;
                Debug.Log(_point);
                _point++;
                //transform.position += Offset;
            }
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
        Offset = _toFollow.position - transform.position;
        _oldPosPlayer = _playerMov.gameObject.transform.position;
    }
    private void Update()
    {

        //Le petit bouge et il reproduit le déplacement de son papa. 
    }
    private void FixedUpdate()
    {
        UpdateLists();
        CheckIsPlayerMoving();
        MoveBaby();
    }
}