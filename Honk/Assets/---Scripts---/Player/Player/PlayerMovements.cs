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
    [SerializeField] private float _maxSpeed, _baseSpeed;
    public float ActualSpeed;

    [SerializeField] private float _speedAugmentation;
    [SerializeField] private float _speedDecrease;

    public Vector3 Direction;
    private Vector2 _input;

    [SerializeField] private float _smoothTime;
    public float CurrentVelocity;
    public CharacterController CharacterController;

    private bool _canSpeedAugment = false, _canSpeedDecrease = true;

    [Header("Gravity")]
    [HideInInspector] public float Velocity;
    [SerializeField] private float _gravityMultiplier;
    private float _gravity = -9.81f;

    [Header("Jump")]
    [SerializeField] private float _jumpPower;

    [Header("TimerCoyauteJump")]
    [SerializeField] private float _maxTimer;
    private float _currentTimer;
    private bool _canJump;

    [Header("Silde")]
    private PlayerSlides _playerSlide;

    [Header("States")]
    [HideInInspector] public bool IsWaking;
    [HideInInspector] public bool IsSliding;
    [HideInInspector] public bool IsSwimming;
    #endregion

    public void Move(InputAction.CallbackContext context)
    {
        if (IsWaking)
        {
            //if (context.performed)
            //{
            _canSpeedAugment = true;
            _canSpeedDecrease = false;
            //}
            _input = context.ReadValue<Vector2>();
            Direction = new Vector3(_input.x, Direction.y, _input.y);
            if (context.canceled)
            {
                _canSpeedAugment = false;
                _canSpeedDecrease = true;
            }
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

    private void ResetBools()
    {
        IsWaking = true;
        IsSliding = false;
        IsSwimming = false;
    }
    private void IncreaseTimer()
    {
        _currentTimer += Time.deltaTime;
    }
    private void ResetTimer()
    {
        _currentTimer = 0;
    }
    private void ResetJumpCounter()
    {
        _canJump = true;
    }
    private void CheckIsGroundedCoyauteJump()
    {
        if (!IsGrounded())
        {
            IncreaseTimer();
            //_canJump = false;
            if (/*_canJump && */_currentTimer > _maxTimer)
            {
                _canJump = false;
            }
        }
        if (IsGrounded())
        {
            ResetTimer();
            ResetJumpCounter();
        }
    }
    private void AugmentSpeedToMaxSpeed()
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
        if (ActualSpeed > _baseSpeed && _canSpeedDecrease)
        {
            ActualSpeed -= _speedDecrease * Time.deltaTime;
        }
        if (ActualSpeed < _baseSpeed)
        {
            ActualSpeed = _baseSpeed;
        }
    }
    private void TeleportToSpawnPoint()
    {
        transform.position = _spawnPoint.position;
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
            //_playerSlide.transform.rotation = Quaternion.EulerRotation(_playerSlide.AngleSlide, transform.rotation.y, transform.rotation.z);
            //_playerSlide.transform.position = new Vector3(Direction.x * ActualSpeed, Direction.x * ActualSpeed, Direction.x * ActualSpeed);
        }
    }
    private void ApplySpeed()
    {
        AugmentSpeedToMaxSpeed();
        DecreaseSpeed();
    }


    private void Awake()
    {
        CharacterController = GetComponent<CharacterController>();
        _playerSlide = GetComponent<PlayerSlides>();
    }
    private void Start()
    {
        TeleportToSpawnPoint();
        ActualSpeed = _baseSpeed;
        ResetBools();
    }
    private void FixedUpdate()
    {
        //CheckIsGroundedCoyauteJump();
        ApplyMovement();
        ApplyGravity();
        ApplySpeed();
    }
    private void Update()
    {
        ApplyRotation();
        if (IsGrounded())
        {
            _canJump = true;
        }
    }
}