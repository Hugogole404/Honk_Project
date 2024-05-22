using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
//using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UIElements;

public class TestBabyWalk : MonoBehaviour
{
    public bool CanBabyInputs;
    public float Speed;
    public float VelocityMax;
    [Space]
    public Vector3 BaseScaleBaby;
    public Vector3 BaseScaleBabyTest;
    public Vector3 Offset;
    public List<Vector3> LastPositionPlayer;
    public Vector3 LastPOSPLAYER;

    //[SerializeField] private float _speed;
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
        GetComponent<Rigidbody>().AddForce(Vector3.down * Time.deltaTime * _gravityMultiplier);
    }
    private void UpdateRotationBaby()
    {
        if (CanBabyInputs)
        {
            if (_holdBaby.IsOnHisBack == false && _isDadMoving && _playerMov.CanBabyFollow)
            {
                transform.rotation = _playerMov.transform.rotation;
                //transform.eulerAngles = _playerMov.transform.eulerAngles;
            }
        }
    }
    private void BabyMov()
    {
        if (CanBabyInputs)
        {
            if (_holdBaby.IsOnHisBack)
            {
                LastPositionPlayer.Clear();
                Point = 0;
            }
            if (_isDadMoving && _holdBaby.IsOnHisBack == false && _playerMov.CanBabyFollow)
            {
                //// chara controller
                //Vector3 direction = _playerMov.Direction;
                //direction.y = transform.position.y;
                ////direction = direction.normalized;
                ////GetComponent<CharacterController>().Move(_walkSpd * Time.deltaTime);
                //GetComponent<CharacterController>().Move(Speed * Time.deltaTime * direction);
                ////GetComponent<CharacterController>().enabled = true;


                // code d'avant
                Offset = LastPOSPLAYER - _playerMov.transform.position;
                Offset.y = 0;
                transform.position -= Offset;

                Point++;


                /// pas pris
                ///if (Offset.x < 0) Offset.x = -1;
                ///if (Offset.x > 0) Offset.x = 1;

                ///if (Offset.z < 0) Offset.z = -1;
                ///if (Offset.z > 0) Offset.z = 1;

                ///if (GetComponent<Rigidbody>().velocity.magnitude <= VelocityMax)
                ///    GetComponent<Rigidbody>().AddForce(new Vector3(Offset.x, 0, Offset.z).normalized * -Speed, ForceMode.Force);

                //print(GetComponent<Rigidbody>().velocity.magnitude);
            }
            UpdatePlayerPos();
        }
    }

    private void Awake()
    {
        _holdBaby = FindAnyObjectByType<HoldBaby>();
        _playerMov = FindAnyObjectByType<PlayerMovements>();
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
        _walkSpd = _playerMov.Direction * Speed;
        CheckIsPlayerMoving();
        BabyMov();
        UpdateRotationBaby();
        RescaleBaby();
    }
}