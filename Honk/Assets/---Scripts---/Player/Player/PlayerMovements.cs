using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ParticleSystemJobs;

[HelpURL("https://app.milanote.com/1RscWs1SJGPM9j/playermovements?p=5Aw4gcZ0pqp")]
[RequireComponent(typeof(CharacterController))]
public class PlayerMovements : MonoBehaviour
{
    #region VARIABLES
    public float ModifyTurn;
    public RaycastHit INFOOOO;
    [SerializeField] private float _smoothTime;
    public float _rotationSpeedSlope = 1f;
    public GameObject ParticulesSyst;
    public GameObject SphereSlope;
    public float InertieSlopeSlow;

    [Header("SpawnPoint")]
    public Transform SpawnPoint;

    [Header("Movements")]
    public GameObject ModelPlayer;
    public float BaseSpeed;
    public float SlideActualSpeed;
    public float BoostSpeedDash;
    public float SpeedModification;
    public float SpeedModificationValueToUpAndDown;
    private bool _canSpeedAugment = false;
    private bool _canSpeedDecrease = true;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _speedAugmentation;
    [SerializeField] private float _speedDecrease;

    [Header("Gravity")]
    public float Velocity;
    private float _gravity = -9.81f;
    [SerializeField] private float _gravityMultiplier;

    [Header("Jump")]
    [SerializeField] private float _jumpPower;

    [Header("TimerCoyauteJump")]
    private bool _canJump;
    private float _currentTimer;
    [SerializeField] private float _maxTimer;

    [Header("Silde")]
    public float CoolDownSlope;
    public float _slopeTurnSpeed;
    private float _slopeValueToRotation;
    [SerializeField] private float _frictionForce;
    [SerializeField] private float _timerSlopeOrientation;
    [SerializeField] private Vector3 _orientationPlayerSlope = new Vector3();
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private AnimationCurve _animationCurve;

    [HideInInspector] public float ActualSpeed;
    [HideInInspector] public float CurrentVelocity;
    [HideInInspector] public float TimerCoolDownSlope;
    [HideInInspector] public bool IsWaking;
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
    [HideInInspector] private PlayerSlides _playerSlides;
    [HideInInspector] private TimerManager _timerManager;
    #endregion

    #region ACTIONS
    public void Move(InputAction.CallbackContext context)
    {
        if (IsWaking)
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
            //ValueRotation(Input.x);
            //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, /*ModifyTurn*/-1 * Time.deltaTime, 0));
            //ModelPlayer.transform.rotation = new Quaternion(Input.x, Direction.y, Input.y, 0);
        }
        if (IsSwimming)
        {
            Input = context.ReadValue<Vector2>();
            Direction = new Vector3(Input.x, Direction.y, Input.y) / 3;
        }
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        if (!IsGrounded() || _currentTimer >= _maxTimer)
        {
            _canJump = false;
            return;
        }
        if (_canJump)
        {
            Velocity += _jumpPower;
        }
    }
    public void HonkNoise(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("HonkNoise");
        }
    }
    #endregion

    #region BOOLS SWAP
    public void IsWalkingBools()
    {
        IsWaking = true;
        IsSliding = false;
        IsSwimming = false;
    }
    public void IsSlidingBools()
    {
        IsWaking = false;
        IsSliding = true;
        IsSwimming = false;
    }
    public void IsSwimmingBools()
    {
        IsWaking = false;
        IsSliding = false;
        IsSwimming = true;
    }
    #endregion

    private void TeleportToSpawnPoint()
    {
        CharaController.enabled = false;
        transform.position = SpawnPoint.position;
        CharaController.enabled = true;
    }
    private void ResetJumpCounter()
    {
        _canJump = true;
    }
    private void SurfaceAllignementSlide()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit info = new RaycastHit();
        if (Physics.Raycast(ray, out info, _whatIsGround))
        {
            INFOOOO.normal = info.normal;
            //ModelPlayer.transform.rotation = Quaternion.FromToRotation(/*Vector3.back*/new Vector3(0, 0, -1), info.normal);

            float slopeAngle = Mathf.Deg2Rad * Vector3.Angle(Vector3.up, info.normal);
            //float speedAngle = slopeAngle - 90;
            SlideActualSpeed = slopeAngle * Mathf.Deg2Rad * _playerSlides.SlidingSpeed;
            NormalAngle = new Vector3(info.normal.x, Mathf.Max(info.normal.y - 1.5f, -1f), info.normal.z);

            CurrentSpeed += NormalAngle * SlideActualSpeed;
            //CurrentSpeed = new Vector3(CurrentSpeed.x, WalkingSpeed.y, CurrentSpeed.z);
            //CurrentSpeed = new Vector3(CurrentSpeed.x * BaseSpeed / 10 * SpeedModification - (_frictionForce * CurrentSpeed.x * Time.deltaTime), 
            //    WalkingSpeed.y, 
            //    CurrentSpeed.z * BaseSpeed / 10 * SpeedModification - (_frictionForce * CurrentSpeed.z * Time.deltaTime));
            Vector3 varTemp = new Vector3(CurrentSpeed.x * BaseSpeed / 10 * SpeedModification - (_frictionForce * CurrentSpeed.x * Time.deltaTime),
                WalkingSpeed.y,
                CurrentSpeed.z * BaseSpeed / 10 * SpeedModification - (_frictionForce * CurrentSpeed.z * Time.deltaTime));
            CurrentSpeed = Vector3.Lerp(CurrentSpeed, varTemp, ModifyTurn);

            //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, ModifyTurn * Time.deltaTime, 0));
            //transform.rotation = Quaternion.FromToRotation(-Direction, -info.normal);
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(/*Vector3.up*/-Direction + _orientationPlayerSlope, info.normal), _animationCurve.Evaluate(_timerSlopeOrientation));
        }
    }

    private void StartSlidingInpulse()
    {
        Vector3 inpulseGiven = new Vector3(Direction.x * 10, 0, Direction.z * 10);
        Direction += inpulseGiven;
    }
    //private Vector3 AdjustVelocityToSlope(Vector3 velocity)
    //{
    //    var ray = new Ray(transform.position, Vector3.down);
    //    if (Physics.Raycast(ray, out RaycastHit hitInfo, 0.2f))
    //    {
    //        var slopeRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
    //        var adjustedVelocity = slopeRotation * velocity;
    //        if (adjustedVelocity.y < 0)
    //        {
    //            return adjustedVelocity;
    //        }
    //    }
    //    return velocity;
    //}

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
    private void ValueRotation(float inputValue)
    {
        if (inputValue < 0)
        {
            _slopeValueToRotation -= _slopeTurnSpeed * Time.deltaTime;
        }
        else if (inputValue >= 0)
        {
            _slopeValueToRotation += _slopeTurnSpeed * Time.deltaTime;
        }
    }

    #region CHECKS
    public bool IsGrounded() => CharaController.isGrounded;
    private void CheckIsGroundedCoyauteJump()
    {
        if (!IsGrounded())
        {
            _timerManager.IncreaseTimer(_currentTimer);
            if (/*_canJump && */_currentTimer > _maxTimer)
            {
                _canJump = false;
            }
        }
        if (IsGrounded())
        {
            _timerManager.ResetTimer(_currentTimer);
            ResetJumpCounter();
        }
    }
    private void CheckLastPosition()
    {
        if (LastPos.y > gameObject.transform.position.y)
        {
            SpeedModification += SpeedModificationValueToUpAndDown * Time.deltaTime;
            Debug.Log("Il descend");
        }
        else if (LastPos.y < gameObject.transform.position.y)
        {
            SpeedModification -= SpeedModificationValueToUpAndDown * Time.deltaTime;
            Debug.Log("Il monte");
        }
        else
        {
            SpeedModification = 1;
            Debug.Log("Il ne change pas de hauteur");
        }
        CheckSpeedModification();
        LastPos = gameObject.transform.position;
    }
    private void CheckSpeedModification()
    {
        if (SpeedModification > _maxSpeed)
        {
            SpeedModification = _maxSpeed;
        }
        if (SpeedModification < -_maxSpeed)
        {
            SpeedModification = -_maxSpeed;
        }
    }
    private void CheckIsGroundedForParticles()
    {
        if (IsGrounded())
        {
            ParticulesSyst.GetComponent<ParticleSystem>().enableEmission = true;
        }
        else
        {
            ParticulesSyst.GetComponent<ParticleSystem>().enableEmission = false;
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
        if (IsWaking)
        {
            var targetAngle = Mathf.Atan2(Direction.x, Direction.z) * Mathf.Rad2Deg;
            var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref CurrentVelocity, _smoothTime);
            //transform.rotation = Quaternion.Euler(0f, angle, 0f);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
        else if (IsSliding)
        {
            var targetAngle = Mathf.Atan2(Direction.x, Direction.z) * Mathf.Rad2Deg;
            var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref CurrentVelocity, _rotationSpeedSlope);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            //var targetAngle = Mathf.Atan2(Direction.x, Direction.z) * Mathf.Rad2Deg;
            //var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref CurrentVelocity, _smoothTime);
            ////var ANGLE = Quaternion.Euler(Input.x, transform.rotation.y, Input.y);
            ////ModelPlayer.transform.rotation = Quaternion.Euler(0f, angle, 0f);
            ////transform.rotation = Quaternion.Euler(0f, angle, 0f);
            ////transform.rotation = Quaternion.Euler(0f, angle, 0f);
            //ModelPlayer.transform.rotation = Quaternion.Euler(INFOOOO.normal.x, angle, INFOOOO.normal.z);
        }
    }
    private void ApplyMovement()
    {
        if (IsWaking)
        {
            CharaController.Move(WalkingSpeed * Time.deltaTime);
            GetComponent<CharacterController>().enabled = true;
            SphereSlope.SetActive(false);
        }
        if (IsSliding)
        {
            //Vector3 oui = new Vector3(Direction.x, 0, Direction.z) * Time.deltaTime;
            //CharaController.Move((CurrentSpeed * Time.deltaTime) + oui);
            //GetComponent<CharacterController>().enabled = false;
            //SphereSlope.GetComponent<Rigidbody>().AddForce(CurrentSpeed * Time.deltaTime / (InertieSlopeSlow*InertieSlopeSlow), ForceMode.VelocityChange);
            transform.position = SphereSlope.transform.position;
        }
        if (IsSwimming)
        {
            CharaController.Move(new Vector3(Direction.x * ActualSpeed / 10, Velocity, Direction.z * ActualSpeed / 10) * ActualSpeed * Time.deltaTime);
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
        CharaController = GetComponent<CharacterController>();
        _playerSlides = GetComponent<PlayerSlides>();
        _timerManager = FindAnyObjectByType<TimerManager>();
    }
    private void Start()
    {
        TeleportToSpawnPoint();
        ActualSpeed = BaseSpeed;
        IsWalkingBools();
        PlayerOriginRotation = ModelPlayer.transform.rotation;
    }
    private void FixedUpdate()
    {
        CheckIsGroundedCoyauteJump();
        ApplyMovement();
        ApplySpeed();
    }
    private void Update()
    {
        TimerCoolDownSlope += Time.deltaTime;
        WalkingSpeed = Direction * ActualSpeed;

        ApplyRotation();
        CheckLastPosition();
        ApplyGravity();
        if (IsGrounded())
        {
            _canJump = true;
        }
        if (IsSliding)
        {
            SurfaceAllignementSlide();
            Debug.DrawLine(transform.position, transform.position + NormalAngle * 8, Color.red, 8f);
        }
        CheckIsGroundedForParticles();
    }
}