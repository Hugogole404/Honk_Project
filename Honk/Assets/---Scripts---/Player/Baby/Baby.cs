using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;

public class Baby : MonoBehaviour
{
    public Vector3 Offset;
    public List<Vector3> LasPositionPlayer;

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
            LasPositionPlayer.Add(_playerMov.gameObject.transform.position + Offset);
        }
        if(_holdBaby.IsOnHisBack == true)
        {
            LasPositionPlayer.Clear();
            _point = 0;
        }
    }
    private void OffsetMovBaby()
    {
        //Offset = _playerMov.gameObject.transform.position - gameObject.transform.position;
        //Offset.y = 0;
    }
    private void MoveBaby()
    {
        // calculer la différence de LastPlaterPos[X] et de LastPosPlayer[X-1]
        // l'ajouter au vector du player 

        if (_holdBaby.IsOnHisBack == false && _isDadMoving && LasPositionPlayer.Count >= 2)
        {
            //transform.position = ToFollow.position - _offset;
            //transform.rotation = ToFollow.rotation;

            foreach (var item in LasPositionPlayer)
            {
                //transform.position += LasPositionPlayer[_point] - LasPositionPlayer[_point - 1];
                //_point++;
                transform.position = item + Offset;
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
        OffsetMovBaby();
        UpdateLists();
        MoveBaby();

        //Le petit bouge et il reproduit le déplacement de son papa. 
    }
    private void FixedUpdate()
    {
        CheckIsPlayerMoving();
    }
}