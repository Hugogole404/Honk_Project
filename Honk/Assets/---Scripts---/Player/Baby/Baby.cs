using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Baby : MonoBehaviour
{
    public List<Vector3> _lastPositionPlayer;
    private List<Vector3> _lastPositionPlayerCopy;
    private Transform ToFollow;

    [SerializeField] private Vector3 _offset;
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
    }
    private void MoveBaby()
    {
        // pas encore operationnel
        if (_holdBaby.IsOnHisBack == false)
        {
            //transform.position = ToFollow.position - _offset;
            //transform.rotation = ToFollow.rotation;

            transform.position = _lastPositionPlayer[_point];
            _point++;
            _lastPositionPlayer.Remove(_lastPositionPlayer[_point]);
        }
    }
    void Start()
    {
        _offset = ToFollow.position - transform.position;
    }
    private void Awake()
    {
        _holdBaby = FindAnyObjectByType<HoldBaby>();
        _playerMov = FindAnyObjectByType<PlayerMovements>();
        ToFollow = FindAnyObjectByType<PlayerMovements>().transform;
    }
    private void Update()
    {
        //_lastPositionPlayer.Append(_playerMov.gameObject.transform.position);
        //_lastPositionPlayer.Add(_playerMov.gameObject.transform.position);
        //MoveBaby();
        //UpdateLists();
    }
}