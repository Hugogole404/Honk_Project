using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBabyWalk : MonoBehaviour
{
    public Vector3 Offset;
    public List<Vector3> LastPositionPlayer;
    public Vector3 LastPOSPLAYER;

    [SerializeField] private float _speed;

    public int Point;
    private bool _isDadMoving;
    private Vector3 _oldPosPlayer;
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
    private void UpdatePlayerPos()
    {
        LastPOSPLAYER = _playerMov.transform.position;
        GetComponent<Rigidbody>().AddForce(Vector3.down * Time.deltaTime);
    }

    private void Awake()
    {
        _holdBaby = FindAnyObjectByType<HoldBaby>();
        _playerMov = FindAnyObjectByType<PlayerMovements>();
    }
    void Start()
    {
        Offset = new Vector3(1, 0, 1);
        _oldPosPlayer = _playerMov.gameObject.transform.position;
        LastPOSPLAYER = _playerMov.transform.position;
    }
    private void BabyMov()
    {
        if (_holdBaby.IsOnHisBack)
        {
            LastPositionPlayer.Clear();
            Point = 0;
        }
        if (_isDadMoving && _holdBaby.IsOnHisBack == false && _playerMov.CanBabyFollow)
        {
            Offset = LastPOSPLAYER - _playerMov.transform.position;
            Offset.y = 0;
            transform.position -= Offset;
            Point++;
        }
        UpdatePlayerPos();
    }
    private void Update()
    {
        CheckIsPlayerMoving();
        BabyMov();
    }
}