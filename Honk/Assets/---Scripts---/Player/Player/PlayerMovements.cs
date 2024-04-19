using UnityEngine;
using UnityEngine.InputSystem;

[HelpURL("https://app.milanote.com/1RscWs1SJGPM9j/playermovements?p=5Aw4gcZ0pqp")]
[RequireComponent(typeof(CharacterController))]
public class PlayerMovements : MonoBehaviour
{
    #region VARIABLES
    [Header("Player")]
    public Animator AnimatorHonk;
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
    [Header("Jump")]
    [SerializeField] private float _jumpPower;
    [SerializeField] private float _timerAnimJump;
    [Header("Gravity")]
    public float Velocity;
    [SerializeField] private float _gravityMultiplier;
    [Header("TimerCoyauteJump")]
    [SerializeField] private float _maxTimer;
    [Header("SpawnPoint")]
    public GameObject SpawnPoint;
    [Header("FX")]
    public GameObject SnowTrail;

    [Header("Slope")]
    public float _rotationSpeedSlope = 1f;
    public float InertieSlopeSlow;
    [SerializeField] private float _frictionForce;
    [SerializeField] private RaycastHit _slopeHit;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private GameObject _inputsForBaby;

    private float _gravity = -9.81f;
    private float _currentTimerAnimJump;
    private bool _canTimerAnimJump = false;
    private bool _canJump;
    private float _currentTimer;
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
    [HideInInspector] private bool IsMoving;
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

    #region ACTIONS
    public void Move(InputAction.CallbackContext context)
    {
        if (IsWalking)
        {
            AnimatorHonk.SetBool("IsMoving", true);
            _canSpeedAugment = true;
            _canSpeedDecrease = false;

            Input = context.ReadValue<Vector2>();
            Direction = new Vector3(Input.x, Direction.y, Input.y);
            if (context.canceled)
            {
                AnimatorHonk.SetBool("IsMoving", false);
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
    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        if (!IsGrounded() || _currentTimer <= _maxTimer)
        {
            _canJump = false;
            return;
        }
        else
        {
            _canJump = true;
        }
        if (_canJump)
        {
            _canTimerAnimJump = true;
            AnimatorHonk.SetBool("IsJumping", true);
            AnimatorHonk.SetTrigger("Jump");
            Velocity += _jumpPower;
            _currentTimer = 0;
        }
    }
    public void HonkNoise(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_holdBaby.IsOnHisBack)
            {
                Debug.Log("IS NOT");
                //_baby.GetComponent<BabyMovements>().enabled = true;
                //_inputsForBaby.SetActive(true);
                _baby.GetComponent<Rigidbody>().useGravity = true;
                //_baby.GetComponent<CharacterController>().enabled = false;
                _baby.LastPositionPlayer.Add(_holdBaby.PositionBabyPut.gameObject.transform.position);

                //_baby.Offset = transform.position - _baby.transform.position;
                //_baby.Offset.y = 0;

                _holdBaby.Baby.gameObject.transform.parent = _holdBaby.ParentObjectBaby.gameObject.transform;
                _holdBaby.Baby.transform.position = _holdBaby.PositionBabyPut.transform.position;
                _holdBaby.IsOnHisBack = false;
                //_baby.GetComponent<CharacterController>().enabled = true;
            }
            else
            {
                if (_holdBaby.CanHoldBaby)
                {
                    Debug.Log("IS ON");
                    _baby.GetComponent<BabyMovements>().enabled = false;
                    _inputsForBaby.SetActive(false);
                    _baby.GetComponent<Rigidbody>().useGravity = false;
                    //_baby.GetComponent<CharacterController>().enabled = false;
                    _holdBaby.Baby.gameObject.transform.parent = gameObject.transform;
                    _holdBaby.Baby.gameObject.transform.position = new Vector3(_holdBaby.BasePositionBaby.transform.position.x, _holdBaby.BasePositionBaby.transform.position.y, _holdBaby.BasePositionBaby.transform.position.z);
                    _holdBaby.IsOnHisBack = true;
                    //_baby.GetComponent<CharacterController>().enabled = true;
                }
            }
        }
    }
    #endregion

    #region BOOLS SWAP
    public void IsWalkingBools()
    {
        IsWalking = true;
        IsSliding = false;
        IsSwimming = false;
    }
    public void IsSlidingBools()
    {
        IsWalking = false;
        IsSliding = true;
        IsSwimming = false;
    }
    public void IsSwimmingBools()
    {
        IsWalking = false;
        IsSliding = false;
        IsSwimming = true;
    }
    #endregion

    #region FUNCTIONS
    private void TeleportToSpawnPoint()
    {
        CharaController.enabled = false;
        transform.position = SpawnPoint.transform.position;
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

            float slopeAngle = Mathf.Deg2Rad * Vector3.Angle(Vector3.up, info.normal);

            NormalAngle = new Vector3(info.normal.x, Mathf.Max(info.normal.y - 1.5f, -1f), info.normal.z);

            Vector3 varTemp = new Vector3(CurrentSpeed.x * BaseSpeed / 10 * SpeedModification - (_frictionForce * CurrentSpeed.x * Time.deltaTime),
                WalkingSpeed.y,
                CurrentSpeed.z * BaseSpeed / 10 * SpeedModification - (_frictionForce * CurrentSpeed.z * Time.deltaTime));
            CurrentSpeed = Vector3.Lerp(CurrentSpeed, varTemp, ModifyTurn);
        }
    }
    private void IncreaseTimerAnimJump()
    {
        if (_canTimerAnimJump)
        {
            _currentTimerAnimJump += Time.deltaTime;
        }
        if (_currentTimerAnimJump > _timerAnimJump)
        {
            if (IsGrounded())
            {
                AnimatorHonk.SetBool("IsJumping", false);
                _currentTimerAnimJump = 0;
                _canTimerAnimJump = false;
            }
        }
    }
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
        //else if (IsSliding)
        //{
        //    var targetAngle = Mathf.Atan2(Direction.x, Direction.z) * Mathf.Rad2Deg;
        //    var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref CurrentVelocity, _rotationSpeedSlope);
        //    transform.rotation = Quaternion.Euler(0f, angle, 0f);
        //}
    }
    private void ApplyMovement()
    {
        if (IsWalking)
        {
            CharaController.Move(WalkingSpeed * Time.deltaTime);
            //_baby.GetComponent<CharacterController>().Move(WalkingSpeed * Time.deltaTime);
            //if (_holdBaby.IsOnHisBack == false)
            //{
            //}
            GetComponent<CharacterController>().enabled = true;
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
        _holdBaby = FindAnyObjectByType<HoldBaby>();
        CharaController = GetComponent<CharacterController>();
        _timerManager = FindAnyObjectByType<TimerManager>();
        _baby = FindAnyObjectByType<Baby>();
    }
    private void Start()
    {
        TeleportToSpawnPoint();
        ActualSpeed = BaseSpeed;
        IsWalkingBools();
        PlayerOriginRotation = ModelPlayer.transform.rotation;

        _baby.GetComponent<BabyMovements>().enabled = false;
        _inputsForBaby.SetActive(false);
    }
    private void FixedUpdate()
    {
        //CheckIsGroundedCoyauteJump();
        ApplyMovement();
        ApplySpeed();
    }
    private void Update()
    {
        TimerCoolDownSlope += Time.deltaTime;
        WalkingSpeed = Direction * ActualSpeed;
        _currentTimer += Time.deltaTime;
        ApplyRotation();
        //CheckLastPosition();
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
        IncreaseTimerAnimJump();
    }
}