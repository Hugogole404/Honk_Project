using DG.Tweening;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class Slope : MonoBehaviour
{
    #region VARIABLES
    [Header("Player")]
    public GameObject ModelPlayer;
    public Animator m_Animator;                                                         //J'ai rajouté ça (Adam)
    [Header("Speed")]
    public float Speed;
    public float _maxSpeed;
    [Space]
    public float SpeedSlope;
    public float _maxSpeedSlope;
    [Space]
    public float VelocityMax;
    [Header("Orientation")]
    public float OrientationTurningSpeed;
    [Header("Jump")]
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _maxTimerJump;
    [Header("Gravity")]
    [SerializeField] private float _gravityMultiplier;
    [Header("Spawn Point")]
    public GameObject SpawnPoint;
    [Header("Speed Modification")]
    [SerializeField] private float _speedDecreaseValue;
    [Space]
    [SerializeField] private float _speedAugmentationSlopeValue;
    [SerializeField] private float _speedDecreaseSlopeValue;
    [Header("Slope")]
    [SerializeField] private float _rotationSpeed;
    [Header("VFX")]
    [SerializeField] private ParticleSystem _SmokeFX;
    [SerializeField] private ParticleSystem _SnowFX;

    private bool _modWalk = true;
    private bool _modOnSlope = false;
    private bool _modSlide = false;
    private bool _slideTimer = false;
    private bool _isMovingDown, _isMovingUp, _isMovingStraight;
    private bool _canJump;
    private float _originSpeedSlope;
    private float _gravity = -9.81f;
    private float _currentTimerJump;
    private float _currentTimerSlide;
    private Vector3 _inputsJoystick;
    private Vector3 _lastPosition;
    private Vector2 _moveInput;
    private PlayerMovements _playerMovements;

    private float _aimAngle;

    [HideInInspector] public bool IsGrounded = false;
    [HideInInspector] public bool CanSpeedDown = false;
    [HideInInspector] public bool SpeedMaxCanDecrease = false;
    [HideInInspector] public Rigidbody _rigidbody;
    [HideInInspector] private float _maxTimerSlide;
    [HideInInspector] public float OldSpeedMax, OldSpeedSlopeMax, SpeedToReduce, OldSpeed, OldSpeedSlope;

    public AreaUI AreaUIFadeStart;
    [SerializeField] private VolumeManager _volumeManager;
    [SerializeField] private float _fadeTimer;
    [SerializeField] private CanvasGroup _fadeCanvasGroup;

    private Liste_sound _sounds;
    private bool _isPlayingSound;
    [SerializeField] bool _needSlopeSound;

    #endregion
    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();

        //_inputsJoystick = new Vector3(_moveInput.x, _inputsJoystick.y, _moveInput.y);
        _inputsJoystick = new Vector3(_moveInput.x, transform.position.y, _moveInput.y);

        m_Animator.SetBool("IsMoving", true);

        if (context.canceled)                                                           //J'ai rajouté ça (Adam)
        {
            m_Animator.SetBool("IsMoving", false);
        }
    }
    public void StartSlide(InputAction.CallbackContext context)
    {
        _inputsJoystick = new Vector3(_moveInput.x, _inputsJoystick.y, _moveInput.y);

        //if (context.started && _slideTimer && m_Animator.GetBool("IsSliding") == false)
        //{
        //    _modSlide = true;
        //    _modWalk = false;
        //    _slideTimer = false;
        //    m_Animator.SetBool("IsSliding", true);                                     //J'ai rajouté ça (Adam)
        //    m_Animator.SetTrigger("StartSlide");
        //}
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started && _canJump == true && IsGrounded == true)
        {
            _rigidbody.AddForce(new Vector3(0, 1, 0) * _jumpForce, ForceMode.Impulse);
            _canJump = false;
            _currentTimerJump = 0;
            m_Animator.SetTrigger("Jump");
            m_Animator.SetBool("IsJumping", true);
        }
    }

    public void SpeedModificationAfterSpeedUpArea()
    {
        if (SpeedMaxCanDecrease)
        {
            if (_maxSpeed <= OldSpeedMax && _maxSpeedSlope <= OldSpeedSlopeMax)
            {
                _maxSpeed = OldSpeedMax;
                _maxSpeedSlope = OldSpeedSlopeMax;

                Speed = OldSpeed;
                SpeedSlope = OldSpeedSlope;

                SpeedMaxCanDecrease = false;
            }
            else
            {
                _maxSpeedSlope -= SpeedToReduce * Time.deltaTime;
                _maxSpeed -= SpeedToReduce * Time.deltaTime;
                if (_maxSpeed <= OldSpeedMax)
                {
                    _maxSpeed = OldSpeedMax;
                }
                if (_maxSpeedSlope <= OldSpeedSlopeMax)
                {
                    _maxSpeedSlope = OldSpeedSlopeMax;
                }
            }
        }
    }
    private void SpeedDown()
    {
        if (_modWalk && _rigidbody.velocity.magnitude > 1)
        {
            _rigidbody.velocity -= new Vector3(_rigidbody.velocity.x * Time.deltaTime * _speedDecreaseValue * 10, 0, _rigidbody.velocity.z * Time.deltaTime * _speedDecreaseValue * 10);
        }
        if ((_modSlide || _modOnSlope) && IsGrounded && (_isMovingStraight || _isMovingUp) && _rigidbody.velocity.magnitude > 1)
        {
            _rigidbody.velocity -= _rigidbody.velocity * Time.deltaTime * _speedDecreaseValue;
            _modOnSlope = true;
            _modSlide = false;
        }
    }
    private void CheckLastPosition()
    {
        if (_lastPosition.y - transform.position.y < 0)
        {
            _isMovingUp = true;
            _isMovingDown = false;
            _isMovingStraight = false;
            //Debug.Log("Il Monte");

            if (_modOnSlope && IsGrounded)
            {
                SpeedSlope -= _speedDecreaseSlopeValue * Time.deltaTime;
            }
            if (SpeedSlope <= 0)
            {
                _rigidbody.velocity = new Vector3(0, 0, 0);
            }
        }
        else if (_lastPosition.y - transform.position.y > 0)
        {
            _isMovingUp = false;
            _isMovingDown = true;
            _isMovingStraight = false;
            if (_modOnSlope && IsGrounded)
            {
                SpeedSlope += _speedAugmentationSlopeValue * Time.deltaTime;
            }
            //Debug.Log("Il descend");
        }
        else if (_lastPosition.y == transform.position.y)
        {
            _isMovingUp = false;
            _isMovingDown = false;
            _isMovingStraight = true;
            //Debug.Log("Il ne change pas de hauteur");
        }
        _lastPosition = transform.position;
    }
    private void CheckMaxSpeed()
    {
        if (_modWalk)
        {
            if (_rigidbody.velocity.magnitude > _maxSpeed)
            {
                _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, _maxSpeed);
            }
            if (_rigidbody.velocity.magnitude < -_maxSpeed)
            {
                _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, -_maxSpeed);
            }
            if (Speed > _maxSpeed)
            {
                Speed = _maxSpeed;
            }
            if (SpeedSlope > _maxSpeedSlope)
            {
                SpeedSlope = _maxSpeedSlope;
            }
        }
        else
        {
            if (_rigidbody.velocity.magnitude > _maxSpeedSlope)
            {
                _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, _maxSpeedSlope);
            }
            if (_rigidbody.velocity.magnitude < -_maxSpeedSlope)
            {
                _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, -_maxSpeedSlope);
            }
            if (Speed > _maxSpeed)
            {
                Speed = _maxSpeed;
            }
            if (SpeedSlope > _maxSpeedSlope)
            {
                SpeedSlope = _maxSpeedSlope;
            }
        }
    }

    private void ApplyRotationSlope()
    {
        if (_modOnSlope)
        {
            _rigidbody.velocity += new Vector3(_rotationSpeed * _moveInput.x * Time.deltaTime, 0, _rotationSpeed * _moveInput.y * Time.deltaTime);
        }
    }
    private void ApplyMovement()
    {
        if (_modWalk)
        {
            _rigidbody.AddForce(new Vector3(_moveInput.x, 0, _moveInput.y) * Speed * Time.deltaTime * 100, ForceMode.Force);
        }
        if (_modSlide /*&& _rigidbody.velocity.magnitude > 0.05f*/)
        {
            _rigidbody.AddForce(new Vector3(_moveInput.x, 0, _moveInput.y) * SpeedSlope * Time.deltaTime * 100, ForceMode.Force);
            _modOnSlope = true;
            _modSlide = false;
        }
    }
    private void ApplyGravity()
    {
        _rigidbody.velocity += new Vector3(0, _gravity * Time.deltaTime * _gravityMultiplier, 0);
    }

    private void TimerSlide()
    {
        _currentTimerSlide += Time.deltaTime;

        if (_currentTimerSlide > _maxTimerSlide)
        {
            _slideTimer = true;
        }
    }
    private void TimerJump()
    {
        _currentTimerJump += Time.deltaTime;

        if (_currentTimerJump > _maxTimerJump)
        {
            _canJump = true;
        }
    }
    private void OrientationPlayer()
    {
        if (_inputsJoystick.x != 0 || _inputsJoystick.z != 0)
        {
            Vector3 targetPos = new Vector3(ModelPlayer.transform.position.x + 1 * _inputsJoystick.x, 0, ModelPlayer.transform.position.z - 1 * _inputsJoystick.z);

            Vector3 POS = new Vector3(targetPos.x, 0, targetPos.z);

            Vector3 aimDir = POS - ModelPlayer.transform.position;
            _aimAngle = Mathf.Atan2(aimDir.z, aimDir.x) * Mathf.Rad2Deg - 90f;
            ModelPlayer.transform.localEulerAngles = new Vector3(0, _aimAngle + 180, 0);
        }

    }

    private void FX()
    {
        if (IsGrounded == true)
        {
            _SmokeFX.enableEmission = true;
            _SnowFX.enableEmission = true;
        }
        else
        {
            _SmokeFX.enableEmission = false;
            _SnowFX.enableEmission = false;
        }
    }

    private void Start()
    {
        _sounds = FindObjectOfType<Liste_sound>();
        AreaUIFadeStart.FadeOut(1.5f);

        _rigidbody = GetComponent<Rigidbody>();
        _playerMovements = FindAnyObjectByType<PlayerMovements>();
        _lastPosition = transform.position;
        transform.position = SpawnPoint.transform.position;
        _originSpeedSlope = SpeedSlope;

        _modSlide = true;
        _modWalk = false;
        _slideTimer = false;
        m_Animator.SetBool("IsSliding", true);                                     //J'ai rajouté ça (Adam)
        m_Animator.SetTrigger("StartSlide");

        AreaUIFadeStart.UI_ToActivate_or_not = _fadeCanvasGroup;
        AreaUIFadeStart.FadeOut(_fadeTimer);

    }
    private void Update()
    {
        SpeedDown();
        ApplyGravity();
        SpeedModificationAfterSpeedUpArea();
        ApplyMovement();

        OrientationPlayer();
        ApplyRotationSlope();

        TimerJump();
        TimerSlide();

        CheckMaxSpeed();

        FX();
        if (IsGrounded && _isPlayingSound == false)
        {
            if (_needSlopeSound == false)
            {
                _sounds.SlopeSound.Play();
            }
            _isPlayingSound = true;
        }
        else if (IsGrounded == false)
        {
            if (_needSlopeSound == false)
            {
                _sounds.SlopeSound.Pause();
            }
            _isPlayingSound = false;
        }
    }
    private void FixedUpdate()
    {
        //CheckLastPosition();

    }
}