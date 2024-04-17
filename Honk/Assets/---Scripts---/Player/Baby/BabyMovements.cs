using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BabyMovements : MonoBehaviour
{
    #region VARIABLES
    [Header("Player")]
    public GameObject ModelPlayer;
    [Header("Orientation")]
    public float ModifyTurn;
    [SerializeField] private float _smoothTime;
    [Header("Movements")]
    public float BaseSpeed;
    [SerializeField] private float _maxSpeed;
    [Space]
    [SerializeField] private float _speedAugmentation;
    [SerializeField] private float _speedDecrease;
    [Space]
    public float SpeedModification;
    public float SpeedModificationValueToUpAndDown;
    [Header("Gravity")]
    public float Velocity;
    [SerializeField] private float _gravityMultiplier;
    [Header("FX")]
    public GameObject SnowTrail;

    [Header("Slope")]
    public float _rotationSpeedSlope = 1f;
    public float InertieSlopeSlow;
    [SerializeField] private float _frictionForce;
    [SerializeField] private RaycastHit _slopeHit;
    [SerializeField] private LayerMask _whatIsGround;

    private float _gravity = -9.81f;
    private bool _canSpeedAugment = false;
    private bool _canSpeedDecrease = true;
    private HoldBaby _holdBaby;
    private Baby _baby;


    [HideInInspector] public RaycastHit INFOOOO;
    [HideInInspector] public float ActualSpeed;
    [HideInInspector] public float CurrentVelocity;
    [HideInInspector] public float TimerCoolDownSlope;
    [HideInInspector] public bool IsWalking;
    [HideInInspector] public bool IsSliding;
    [HideInInspector] public bool IsSwimming;
    [HideInInspector] public Vector3 CurrentSpeed;
    [HideInInspector] public Vector3 NormalAngle;
    [HideInInspector] public Vector3 WalkingSpeed;
    [HideInInspector] public Vector3 LastPos;
    [HideInInspector] public Vector3 Direction;
    [HideInInspector] public Vector2 Input;
    [HideInInspector] public Quaternion PlayerOriginRotation;
    [HideInInspector] public CharacterController CharaController;
    [HideInInspector] private TimerManager _timerManager;
    #endregion

    public void Move(InputAction.CallbackContext context)
    {
        if (IsWalking)
        {
            _canSpeedAugment = true;
            _canSpeedDecrease = false;

            Input = context.ReadValue<Vector2>();
            Direction = new Vector3(Input.x, Direction.y, Input.y);
            if (context.canceled)
            {
                _canSpeedAugment = false;
                _canSpeedDecrease = true;
            }
        }
        if (IsSliding)
        {
            Input = context.ReadValue<Vector2>();
            Direction = new Vector3(Input.x, Direction.y, Input.y);
        }
        if (IsSwimming)
        {
            Input = context.ReadValue<Vector2>();
            Direction = new Vector3(Input.x, Direction.y, Input.y) / 3;
        }
    }
    public void IsWalkingBools()
    {
        IsWalking = true;
        IsSliding = false;
        IsSwimming = false;
    }

    #region FUNCTIONS
    private void IncreaseSpeed()
    {
        if (_canSpeedAugment)
        {
            if (ActualSpeed < _maxSpeed)
            {
                ActualSpeed += _speedAugmentation * Time.deltaTime;
            }
            if (ActualSpeed > _maxSpeed)
            {
                ActualSpeed = _maxSpeed;
            }
        }
    }
    private void DecreaseSpeed()
    {
        if (ActualSpeed > BaseSpeed && _canSpeedDecrease)
        {
            ActualSpeed -= _speedDecrease * Time.deltaTime;
        }
        if (ActualSpeed < BaseSpeed)
        {
            ActualSpeed = BaseSpeed;
        }
    }
    #endregion

    #region CHECKS
    public bool IsGrounded() => CharaController.isGrounded;
    public bool IsOnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out _slopeHit, 5f))
        {
            if (_slopeHit.normal != Vector3.up) { return true; }
            else { return false; }
        }
        return false;
    }
    private void CheckIsGroundedForParticles()
    {
        if (IsGrounded())
        {
            SnowTrail.GetComponent<ParticleSystem>().enableEmission = true;
        }
        else
        {
            SnowTrail.GetComponent<ParticleSystem>().enableEmission = false;
        }
    }
    #endregion

    #region APPLY
    private void ApplyGravity()
    {
        if (IsGrounded() && Velocity < 0f)
        {
            Velocity = -1f;
        }
        else
        {
            Velocity += _gravity * _gravityMultiplier * Time.deltaTime;
        }
        Direction.y = Velocity;
    }
    private void ApplyRotation()
    {
        if (Input.sqrMagnitude == 0)
        {
            return;
        }
        if (IsWalking)
        {
            var targetAngle = Mathf.Atan2(Direction.x, Direction.z) * Mathf.Rad2Deg;
            var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref CurrentVelocity, _smoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }
    private void ApplyMovement()
    {
        if (IsWalking)
        {
            CharaController.Move(WalkingSpeed * Time.deltaTime);
            GetComponent<CharacterController>().enabled = true;
        }
    }
    private void ApplySpeed()
    {
        IncreaseSpeed();
        DecreaseSpeed();
    }
    #endregion

    private void Awake()
    {
        _holdBaby = FindAnyObjectByType<HoldBaby>();
        CharaController = GetComponent<CharacterController>();
        _timerManager = FindAnyObjectByType<TimerManager>();
        _baby = FindAnyObjectByType<Baby>();
    }
    private void Start()
    {
        ActualSpeed = BaseSpeed;
        IsWalkingBools();
        PlayerOriginRotation = ModelPlayer.transform.rotation;
    }
    private void FixedUpdate()
    {
        ApplyMovement();
        ApplySpeed();
    }
    private void Update()
    {
        WalkingSpeed = Direction * ActualSpeed;
        ApplyRotation();
        ApplyGravity();
        CheckIsGroundedForParticles();
    }
}