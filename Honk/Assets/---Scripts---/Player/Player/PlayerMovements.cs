using UnityEngine;
using UnityEngine.InputSystem;

[HelpURL("https://app.milanote.com/1RscWs1SJGPM9j/playermovements?p=5Aw4gcZ0pqp")]
[RequireComponent(typeof(CharacterController))]
public class PlayerMovements : MonoBehaviour
{
    #region VARIABLES
    public float ModifyTurn;
    public RaycastHit INFOOOO;
    [Header("SpawnPoint")]
    public Transform SpawnPoint;

    [Header("Movements")]
    public float BaseSpeed;
    public float ActualSpeed;
    public float SlideActualSpeed;
    [Space]
    public float BoostSpeedDash;
    public float SpeedModification;
    public float SpeedModificationValueToUpAndDown;
    [Space]
    public Vector3 CurrentSpeed;
    public Vector3 NormalAngle;
    public Vector3 WalkingSpeed;
    [Space]
    public GameObject ModelPlayer;
    [SerializeField] private float _maxSpeed;
    [Space]
    [SerializeField] private float _speedAugmentation;
    [SerializeField] private float _speedDecrease;
    [Space]
    [SerializeField] private float _smoothTime;
    [Space]
    private bool _canSpeedAugment = false;
    private bool _canSpeedDecrease = true;

    [HideInInspector] public float CurrentVelocity;
    [HideInInspector] public Vector3 Direction;
    [HideInInspector] public Vector2 Input;
    [HideInInspector] public Quaternion PlayerOriginRotation;

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
    public float CoolDownSlope;
    public float _slopeTurnSpeed;
    private float _slopeValueToRotation;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private Vector3 _orientationPlayerSlope = new Vector3();
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] private float _timerSlopeOrientation;
    [HideInInspector] public float TimerCoolDownSlope;
    //[SerializeField] private GameObject _visualPlayer;
    //private Vector3 _velo;

    [Header("Friction")]
    [SerializeField] float _frictionForce;

    [Header("States")]
    [HideInInspector] public bool IsWaking;
    [HideInInspector] public bool IsSliding;
    [HideInInspector] public bool IsSwimming;

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
            //float dirX = ValueRotation(Input.x)
            //float dirY = 
            ValueRotation(Input.x);
            Direction = new Vector3(Input.x, Direction.y, Input.y);
            //ModelePlayer.transform.rotation = new Quaternion(Input.x, Direction.y, Input.y, 0);
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
            //ModelPlayer.transform.rotation = Quaternion.Euler(

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
            //transform.rotation = Quaternion.Euler(0f, angle, 0f);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
        else if (IsSliding)
        {
            var targetAngle = Mathf.Atan2(Direction.x, Direction.z) * Mathf.Rad2Deg;
            var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref CurrentVelocity, _smoothTime);
            //ModelPlayer.transform.rotation = Quaternion.Euler(0f, angle, 0f);
            //transform.rotation = Quaternion.Euler(0f, angle, 0f);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            ModelPlayer.transform.rotation = Quaternion.Euler(INFOOOO.normal.x, angle, INFOOOO.normal.z);

        }
    }
    private void ApplyMovement()
    {
        if (IsWaking)
        {
            CharaController.Move(WalkingSpeed * Time.deltaTime);
        }
        if (IsSliding)
        {
            //CharacterController.Move(new Vector3(Direction.x * ActualSpeed / 10, Velocity, Direction.z * ActualSpeed / 10) * ActualSpeed * Time.deltaTime);
            //CharacterController.Move(new Vector3(Direction.x * ActualSpeed / 10, Velocity * 2, Direction.z * ActualSpeed / 10) * ActualSpeed * Time.deltaTime);
            //StartSlidingInpulse();
            CharaController.Move(CurrentSpeed * Time.deltaTime);
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
        //Debug.Log(_gravity);
        //Direction.Normalize();
        WalkingSpeed = Direction * ActualSpeed;

        //Debug.Log(Input);

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