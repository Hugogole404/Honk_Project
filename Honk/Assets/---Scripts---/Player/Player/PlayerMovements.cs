using UnityEngine;
using UnityEngine.InputSystem;

[HelpURL("https://app.milanote.com/1RscWs1SJGPM9j/playermovements?p=5Aw4gcZ0pqp")]
[RequireComponent(typeof(CharacterController))]
public class PlayerMovements : MonoBehaviour
{
    #region Variables
    [Header("SpawnPoint")]
    [SerializeField] private Transform _spawnPoint;

    [Header("Movements")]
    public float BaseSpeed;
    public float ActualSpeed;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _speedAugmentation;
    [SerializeField] private float _speedDecrease;

    [HideInInspector] public Vector3 Direction;
    [HideInInspector] public float CurrentVelocity;
    [HideInInspector] private Vector2 _input;

    [SerializeField] private float _smoothTime;

    private bool _canSpeedAugment = false;
    private bool _canSpeedDecrease = true;

    [Header("Gravity")]
    [SerializeField] private float _gravityMultiplier;
    [HideInInspector] public float Velocity;
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

    public void Move(InputAction.CallbackContext context)
    {
        if (IsWaking)
        {
            _canSpeedAugment = true;
            _canSpeedDecrease = false;

            _input = context.ReadValue<Vector2>();
            Direction = new Vector3(_input.x, Direction.y, _input.y);
            if (context.canceled)
            {
                _canSpeedAugment = false;
                _canSpeedDecrease = true;
            }
        }
        if (IsSliding)
        {
            _input = context.ReadValue<Vector2>();
            Direction = new Vector3(_input.x, Direction.y, _input.y);
        }
        if (IsSwimming)
        {
            _input = context.ReadValue<Vector2>();
            Direction = new Vector3(_input.x, Direction.y, _input.y) / 3;
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

    public bool IsGrounded() => CharacterController.isGrounded;

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

    private void ResetJumpCounter()
    {
        _canJump = true;
    }
    private void TeleportToSpawnPoint()
    {
        transform.position = _spawnPoint.position;
    }
    private void SurfaceAllignementSlide()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit info = new RaycastHit();
        if (Physics.Raycast(ray, out info, _whatIsGround))
        {
            transform.rotation = Quaternion.FromToRotation(/*Vector3.back*/-Direction + _orientationPlayerSlope, info.normal);
            //transform.rotation = Quaternion.FromToRotation(-Direction, -info.normal);
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(/*Vector3.up*/-Direction + _orientationPlayerSlope, info.normal), _animationCurve.Evaluate(_timerSlopeOrientation));
        }
    }
    private void StartSlidingInpulse()
    {
        Vector3 inpulseGiven = new Vector3(Direction.x * 10,0,Direction.z * 10);
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
        if (_input.sqrMagnitude == 0)
        {
            return;
        }
        var targetAngle = Mathf.Atan2(Direction.x, Direction.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref CurrentVelocity, _smoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }
    private void ApplyMovement()
    {
        if (IsWaking)
        {
            CharacterController.Move(Direction * ActualSpeed * Time.deltaTime);
        }
        if (IsSliding)
        {
            //CharacterController.Move(new Vector3(Direction.x * ActualSpeed / 10, Velocity, Direction.z * ActualSpeed / 10) * ActualSpeed * Time.deltaTime);
            CharacterController.Move(new Vector3(Direction.x * ActualSpeed / 10, Velocity * 2, Direction.z * ActualSpeed / 10) * ActualSpeed * Time.deltaTime);
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
        ApplyMovement();
        ApplyGravity();
        ApplySpeed();
    }
    private void Update()
    {
        ApplyRotation();
        CheckLastPosition();
        if (IsGrounded())
        {
            _canJump = true;
        }
        if (IsSliding)
        {
            SurfaceAllignementSlide();
        }
    }
}