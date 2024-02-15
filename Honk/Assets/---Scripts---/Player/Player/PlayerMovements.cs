using UnityEngine;
using UnityEngine.InputSystem;

[HelpURL("https://app.milanote.com/1RscWs1SJGPM9j/playermovements?p=5Aw4gcZ0pqp")]
[RequireComponent(typeof(CharacterController))]
public class PlayerMovements : MonoBehaviour
{
    #region VARIABLES
    [Header("SpawnPoint")]
    [SerializeField] private Transform _spawnPoint;

    [Header("Movements")]
    public float BaseSpeed;
    public float ActualSpeed;
    public float SlideActualSpeed;
    public Vector3 CurrentSpeed;
    public Vector3 NormalAngle;
    public Vector3 WalkingSpeed;
    [SerializeField] private float _maxSpeed;
    [Space]
    [SerializeField] private float _speedAugmentation;
    [SerializeField] private float _speedDecrease;
    [Space]
    [SerializeField] private float _smoothTime;
    [Space]
    private bool _canSpeedAugment = false;
    private bool _canSpeedDecrease = true;

    [HideInInspector] public Vector3 Direction;
    [HideInInspector] public float CurrentVelocity;
    [HideInInspector] public Vector2 Input;

    [Header("Gravity")]
    [SerializeField] private float _gravityMultiplier;
    public float Velocity;
    private float _gravity = -9.81f;

    [Header("Jump")]
    [SerializeField] private float _jumpPower;

    [Header("TimerCoyauteJump")]
    [SerializeField] private float _maxTimer;
    private float _currentTimer;
    private bool _canJump;

    [Header("Silde")]
    public Vector3 LastPos;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private Vector3 _orientationPlayerSlope = new Vector3();
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] private float _timerSlopeOrientation;
    //[SerializeField] private GameObject _visualPlayer;
    //private Vector3 _velo;

    [Header("States")]
    [HideInInspector] public bool IsWaking;
    [HideInInspector] public bool IsSliding;
    [HideInInspector] public bool IsSwimming;

    [HideInInspector] public CharacterController CharacterController;
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
            //Direction.Normalize();
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
            //Direction.Normalize();
        }
        if (IsSwimming)
        {
            Input = context.ReadValue<Vector2>();
            Direction = new Vector3(Input.x, Direction.y, Input.y) / 3;
            //Direction.Normalize();
        }
        //_gamepad.SetMotorSpeeds(0.075f, 0.134f);
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
            //_canJump = false;
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
        transform.position = _spawnPoint.position;
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
            //transform.rotation = Quaternion.FromToRotation(/*Vector3.back*/-Direction + _orientationPlayerSlope, info.normal);

            float slopeAngle = Mathf.Deg2Rad * Vector3.Angle(Vector3.up, info.normal);
            float speedAngle = slopeAngle - 90;
            SlideActualSpeed = slopeAngle * Mathf.Deg2Rad * _playerSlides.SlidingSpeed;
            NormalAngle = new Vector3(info.normal.x, Mathf.Max(info.normal.y - 1.5f, -1f), info.normal.z);

            CurrentSpeed += NormalAngle * SlideActualSpeed;
            CurrentSpeed = new Vector3(CurrentSpeed.x, WalkingSpeed.y, CurrentSpeed.z);

            //if (IsGrounded())
            //{
            //    CurrentSpeed.y += _gravity;
            //}
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

    #region CHECKS
    public bool IsGrounded() => CharacterController.isGrounded;
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
            Debug.Log("Il descend");
        }
        else if (LastPos.y < gameObject.transform.position.y)
        {
            Debug.Log("Il monte");
        }
        else
        {
            Debug.Log("Il ne change pas de hauteur");
        }
        LastPos = gameObject.transform.position;
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
            //_velo = AdjustVelocityToSlope(_velo);
            //_velo.y += ActualSpeed;
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
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
        else if (IsSliding)
        {
            var targetAngle = Mathf.Atan2(Direction.x, Direction.y) * Mathf.Rad2Deg;
            var angle = Mathf.SmoothDampAngle(transform.eulerAngles.z, targetAngle, ref CurrentVelocity, _smoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }
    private void ApplyMovement()
    {
        if (IsWaking)
        {
            CharacterController.Move(WalkingSpeed * Time.deltaTime);
        }
        if (IsSliding)
        {
            //CharacterController.Move(new Vector3(Direction.x * ActualSpeed / 10, Velocity, Direction.z * ActualSpeed / 10) * ActualSpeed * Time.deltaTime);
            //CharacterController.Move(new Vector3(Direction.x * ActualSpeed / 10, Velocity * 2, Direction.z * ActualSpeed / 10) * ActualSpeed * Time.deltaTime);
            CharacterController.Move(CurrentSpeed * Time.deltaTime);
            //StartSlidingInpulse();
        }
        if (IsSwimming)
        {
            CharacterController.Move(new Vector3(Direction.x * ActualSpeed / 10, Velocity, Direction.z * ActualSpeed / 10) * ActualSpeed * Time.deltaTime);
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
        CharacterController = GetComponent<CharacterController>();
        _playerSlides = GetComponent<PlayerSlides>();
        _timerManager = FindAnyObjectByType<TimerManager>();
    }
    private void Start()
    {
        TeleportToSpawnPoint();
        ActualSpeed = BaseSpeed;
        IsWalkingBools();
    }
    private void FixedUpdate()
    {
        CheckIsGroundedCoyauteJump();
        ApplyMovement(); ;
        ApplySpeed();
    }
    private void Update()
    {
        Debug.Log(_gravity);
        //Direction.Normalize();
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
    }
}