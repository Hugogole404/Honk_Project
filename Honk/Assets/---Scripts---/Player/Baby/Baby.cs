using System.Collections.Generic;
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
    private HoldBaby _holdBaby;
    private PlayerMovements _playerMov;

    private void UpdateLists()
    {
        foreach (var item in _lastPositionPlayer)
        {
            if(item != null)
            {
                _lastPositionPlayerCopy.Add(item);
            }
        }
        _lastPositionPlayer.Clear();
        _lastPositionPlayer = _lastPositionPlayerCopy;
        _lastPositionPlayerCopy.Clear();
        _point = 0;
    }
    private void MoveBaby()
    {
        if (_holdBaby.IsOnHisBack == false)
        {
            //transform.position = ToFollow.position - _offset;
            //transform.rotation = ToFollow.rotation;

            transform.position = _lastPositionPlayer[_point];
            _point++;
            _lastPositionPlayer.Remove(_lastPositionPlayer[_point]);
            UpdateLists();
        }
    }
    void Start()
    {
        _offset = _toFollow.position - transform.position;
    }
    private void Awake()
    {
        _holdBaby = FindAnyObjectByType<HoldBaby>();
        _playerMov = FindAnyObjectByType<PlayerMovements>();
        _toFollow = FindAnyObjectByType<PlayerMovements>().transform;
    }
    private void Update()
    {
        //_lastPositionPlayer.Append(_playerMov.gameObject.transform.position);
        _lastPositionPlayer.Add(_playerMov.gameObject.transform.position);
        MoveBaby();
        UpdateLists();

        //Le petit bouge et il reproduit le déplacement de son papa. w
    }
}