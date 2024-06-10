using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestBabyWalk : MonoBehaviour
{
    public bool CanBabyInputs;
    public int SetGravityBaby;
    public float Speed;
    public float VelocityMax;
    public Vector3 Direction;
    [Space]
    public Vector3 BaseScaleBaby;
    public Vector3 BaseScaleBabyTest;
    public Vector3 Offset;
    public Vector3 LastPOSPLAYER;

    [SerializeField] private float _gravityMultiplier;

    public int Point;
    private bool _isDadMoving;
    private Vector3 _oldPosPlayer;
    private HoldBaby _holdBaby;
    private PlayerMovements _playerMov;

    [Header("Crushed Baby")]
    public GameObject Visual;
    public float ScaleCrushedBaby;
    private float _baseScaleCrushedBaby;
    [SerializeField] private float _speedToNormalScale;
    [SerializeField] private float _maxTimerToNormalScale;
    private float _currentTimerToNormalScale;
    [HideInInspector] public bool CanGoToNormalScale;
    private Vector3 _walkSpd;
    private CharacterController _characterController;
    private float _velo;

    [HideInInspector] public GameObject ActualIceBlocParent;


    private void RescaleBaby()
    {
        if (CanGoToNormalScale)
        {
            _currentTimerToNormalScale += Time.deltaTime;
            if (_currentTimerToNormalScale >= _maxTimerToNormalScale)
            {
                transform.localScale += new Vector3(0, Time.deltaTime * _speedToNormalScale, 0);
            }
            if (transform.localScale.y >= _baseScaleCrushedBaby)
            {
                transform.localScale = new Vector3(transform.localScale.x, _baseScaleCrushedBaby, transform.localScale.z);
                _currentTimerToNormalScale = 0;
                CanGoToNormalScale = false;
            }
        }
    }
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
    }
    private void UpdateRotationBaby()
    {
        if (CanBabyInputs)
        {
            if (_holdBaby.IsOnHisBack == false && _isDadMoving && _playerMov.CanBabyFollow)
            {
                transform.rotation = _playerMov.transform.rotation;
            }
        }
    }
    private void BabyMov()
    {
        if (CanBabyInputs)
        {
            if (_holdBaby.IsOnHisBack)
            {
                Point = 0;
            }
            if (_isDadMoving && _holdBaby.IsOnHisBack == false && _playerMov.CanBabyFollow)
            {
                //// chara controller
                Direction = _playerMov.Direction;

                _velo = 0;
                _velo -= 9.75f * Time.deltaTime * _gravityMultiplier;
                Direction.y = _velo * SetGravityBaby;
                //Direction.y = transform.position.y * SetGravityBaby;
                _walkSpd = Direction * Speed;

                float distanceX = _oldPosPlayer.x - LastPOSPLAYER.x;
                float distanceZ = _oldPosPlayer.z - LastPOSPLAYER.z;

                print(distanceX);
                print(distanceZ);

                if (distanceX > 0.1f || distanceX < - 0.1f)
                {
                    _walkSpd.x = 0;
                }
                if (distanceZ > 0.1f || distanceZ < - 0.1f)
                {
                    _walkSpd.z = 0;
                }
                GetComponent<CharacterController>().Move(_walkSpd * Time.deltaTime);

                // code d'avant
                //Offset = LastPOSPLAYER - _playerMov.transform.position;
                //Offset.y = 0;
                //transform.position -= Offset;
                //Point++;
            }
            else if (_holdBaby.IsOnHisBack == false)
            {
                // Gravity
                _velo = 0;
                _velo -= 9.75f * Time.deltaTime * _gravityMultiplier;
                Direction.y = _velo * SetGravityBaby;
                Direction.x = 0;
                Direction.z = 0;
                _walkSpd = Direction * Speed;

                GetComponent<CharacterController>().Move(_walkSpd * Time.deltaTime);
            }
            UpdatePlayerPos();
        }
    }

    public bool IsGrounded() => _characterController.isGrounded;
    private void Awake()
    {
        _holdBaby = FindAnyObjectByType<HoldBaby>();
        _playerMov = FindAnyObjectByType<PlayerMovements>();
        _characterController = GetComponent<CharacterController>();
    }
    void Start()
    {
        BaseScaleBaby = transform.lossyScale;
        BaseScaleBabyTest = transform.localScale;
        Offset = new Vector3(1, 0, 1);
        _oldPosPlayer = _playerMov.gameObject.transform.position;
        LastPOSPLAYER = _playerMov.transform.position;

        _baseScaleCrushedBaby = 1;
        CanBabyInputs = true;
        //_baseScaleCrushedBaby = transform.localScale.y;
    }
    private void Update()
    {
        CheckIsPlayerMoving();
        BabyMov();
        UpdateRotationBaby();
        RescaleBaby();
    }
}