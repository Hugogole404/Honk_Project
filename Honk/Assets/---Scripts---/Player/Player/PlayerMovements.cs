using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEditor;
using Unity.VisualScripting;

[HelpURL("https://app.milanote.com/1RscWs1SJGPM9j/playermovements?p=5Aw4gcZ0pqp")]
[RequireComponent(typeof(CharacterController))]
public class PlayerMovements : MonoBehaviour
{
    #region VARIABLES
    public bool CanPlayerUseInputs;
    [Header("Player")]
    public Animator AnimatorHonk;
    public Animator AnimatorHonkJR;
    public GameObject ModelPlayer;
    public GameObject FX_Honking;
    [SerializeField] private bool _hadSpawnPoint;
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
    public ParticleSystem FXGround;
    public GameObject IconFollowHonk;
    public GameObject IconFollowHonkJR;
    [Header("CameraSkake")]
    public ShakeData ShakeData;

    [Header("Slope")]
    public float _rotationSpeedSlope = 1f;
    public float InertieSlopeSlow;
    [SerializeField] private float _frictionForce;
    [SerializeField] private RaycastHit _slopeHit;
    [SerializeField] private LayerMask _whatIsGround;
    [Header("Push Obstacles")]
    [SerializeField] private float _pushForce;

    private float _gravity = -9.81f;
    private float _currentTimerAnimJump;
    private bool _canTimerAnimJump = false;
    private bool _canJump;
    private float _currentTimer;
    private bool _canSpeedAugment = false;
    private bool _canSpeedDecrease = true;
    public HoldBaby _holdBaby;
    public GameObject BabyParent;
    public float OffsetBabyParentX;
    public float OffsetBabyParentY;
    public float OffsetBabyParentZ;
    private Baby _baby;
    public TestBabyWalk _testBabyWalk;
    private bool _canTimerBabyJump;
    private float _currentTimerBabyJump;
    [SerializeField] private float _maxTimerBabyJump;
    [SerializeField] private LayerMask _putBabyLayer;
    [SerializeField] private int _takeBabyLayer;
    public TestBabyWalk _babyPrefab;
    private bool _canBePutHere;
    private Liste_sound _soundsList;

    [HideInInspector] public bool CanPushObstacles;
    [HideInInspector] public GameObject ActualObstacle;

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
    [HideInInspector] public bool CanBabyFollow;
    [HideInInspector] public bool CanTeleportbabyRift;
    [HideInInspector] public Transform TransformRotationBaby;
    [HideInInspector] public bool CanMove;
    [HideInInspector] public bool CanBabyTeleport;
    [HideInInspector] public Vector3 TPBabyPos;
    #endregion

    #region ACTIONS
    public void Move(InputAction.CallbackContext context)
    {
        if (CanPlayerUseInputs)
        {
            if (CanMove)
            {
                if (IsWalking)
                {
                    AnimatorHonk.SetBool("IsMoving", true);
                    AnimatorHonkJR.SetBool("IsMoving", true);
                    _canSpeedAugment = true;
                    _canSpeedDecrease = false;

                    Input = context.ReadValue<Vector2>();
                    Direction = new Vector3(Input.x, Direction.y, Input.y);
                    if (context.canceled)
                    {
                        AnimatorHonk.SetBool("IsMoving", false);
                        AnimatorHonkJR.SetBool("IsMoving", false);
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
        }
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (CanPlayerUseInputs)
        {
            if (CanMove)
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
                    _soundsList.JumpSound.Play();
                    _canTimerAnimJump = true;
                    AnimatorHonk.SetBool("IsJumping", true);
                    AnimatorHonk.SetTrigger("Jump");
                    Velocity += _jumpPower;
                    _currentTimer = 0;
                }
            }
        }
    }
    public void Push(InputAction.CallbackContext context)
    {
        if (CanPlayerUseInputs)
        {
            if (CanMove)
            {
                if (context.performed)
                {
                    // animation push
                    if (CanPushObstacles)
                    {
                        CanPushObstacles = false;
                        //Debug.Log("CA POUSSE LE BLOC");
                        Vector3 pushForce = Direction;

                        Vector3 distBetween = ActualObstacle.transform.position - transform.position;
                        distBetween.y = 0;

                        if (Mathf.Abs(distBetween.x) > Mathf.Abs(distBetween.z)) distBetween.z = 0;
                        else distBetween.x = 0;
                        distBetween = distBetween.normalized;
                        ActualObstacle.GetComponent<Rigidbody>().AddForce(_pushForce * distBetween, ForceMode.VelocityChange);
                    }
                }
            }
        }
    }
    public void TeleportBaby(InputAction.CallbackContext context)
    {
        //if (CanMove)
        //{
        //    if (context.performed)
        //    {
        //        CanTeleportbabyRift = true;
        //        Debug.Log("TRUEEE");
        //    }
        //    if (context.canceled)
        //    {
        //        CanTeleportbabyRift = false;
        //        Debug.Log("FALSEEE");
        //    }
        //}
    }
    public void HonkNoise(InputAction.CallbackContext context)
    {
        if (CanPlayerUseInputs)
        {
            if (CanMove)
            {
                if (context.performed)
                {
                    AnimatorHonk.SetTrigger("Shout");
                    ScreenShake.Instance.Shake(ShakeData);

                    //if (CanPushObstacles) { }

                    if (_holdBaby.CanHoldBaby && _holdBaby)
                    {
                        TakeBaby();
                    }
                    else if (_holdBaby.IsOnHisBack && _holdBaby.CanHoldBaby == false)
                    {
                        // DEPOSER LE PETIT
                        //Debug.Log("IS NOT");

                        //CanMove = false;
                        //_canTimerBabyJump = true;
                        //ActualSpeed = 0;
                        /////////////////////////////////////////////////////////////////////
                        _testBabyWalk.GetComponent<CharacterController>().enabled = false;
                        /////////////////////////////////////////////////////////////////////
                        _testBabyWalk.gameObject.layer = 14;
                        //_testBabyWalk.gameObject.layer = _putBabyLayer;
                        _testBabyWalk.gameObject.GetComponent<BoxCollider>().isTrigger = false;
                        ////////////////////////////////////////////////////////////////////////////////////////////////////
                        //_holdBaby.Baby.GetComponent<Rigidbody>().isKinematic = false;
                        ////////////////////////////////////////////////////////////////////////////////////////////////////

                        _holdBaby.Baby.gameObject.transform.parent = _holdBaby.ParentObjectBaby.gameObject.transform;
                        //_testBabyWalk.transform.DOJump(_holdBaby.PositionBabyPut.transform.position, 2, 1, _maxTimerBabyJump);

                        if (_holdBaby.PositionBabyPut.gameObject.GetComponent<BabyPutDetection>().CanBePut)
                            _testBabyWalk.transform.position = _holdBaby.PositionBabyPut.transform.position;
                        else
                            _testBabyWalk.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);


                        _testBabyWalk.LastPositionPlayer.Add(transform.position);
                        _holdBaby.IsOnHisBack = false;
                        AnimatorHonkJR.SetBool("OnBack", false);
                        /////////////////////////////////////////////////////////////////////
                        _testBabyWalk.GetComponent<CharacterController>().enabled = true;
                        /////////////////////////////////////////////////////////////////////
                        _soundsList.Tremblement.PlaySound();
                    }
                    //else if (_holdBaby.IsOnHisBack == false && _holdBaby.CanHoldBaby == false && CanBabyTeleport)
                    //{
                    //    _testBabyWalk.GetComponent<CharacterController>().enabled = false;
                    //    CanTeleportbabyRift = true;
                    //}
                    else if (_holdBaby.IsOnHisBack == false && _holdBaby.CanHoldBaby == false && CanBabyTeleport == false)
                    {
                        // LE FAIRE FOLLOW
                        if (CanBabyFollow)
                        {
                            CanBabyFollow = false;
                            AnimatorHonkJR.SetBool("IsActive", false);
                            AnimatorHonkJR.SetTrigger("ChangingState");

                            IconFollowHonk.SetActive(false);
                            IconFollowHonkJR.SetActive(false);

                        }
                        else
                        {
                            //_testBabyWalk.GetComponent<CharacterController>().enabled = false;
                            _testBabyWalk.transform.parent = _holdBaby.ParentObjectBaby.gameObject.transform;
                            //_testBabyWalk.GetComponent<CharacterController>().enabled = true;

                            CanBabyFollow = true;
                            AnimatorHonkJR.SetBool("IsActive", true);
                            AnimatorHonkJR.SetTrigger("ChangingState");
                            IconFollowHonk.SetActive(true);
                            IconFollowHonkJR.SetActive(true);
                        }
                    }
                }
                if (context.canceled)
                {
                    CanTeleportbabyRift = false;
                    _testBabyWalk.GetComponent<CharacterController>().enabled = true;
                    //Debug.Log("FALSEEE");
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

    public void TakeBaby()
    {
        // PRENDRE LE PETIT

        //CanMove = false;
        //_canTimerBabyJump = true;
        //ActualSpeed = 0;
        IconFollowHonk.SetActive(false);
        IconFollowHonkJR.SetActive(false);
        _testBabyWalk.SetGravityBaby = 1;
        /////////////////////////////////////////////////////////////////////
        _testBabyWalk.GetComponent<CharacterController>().enabled = false;
        _testBabyWalk.transform.rotation = transform.rotation;
        //_testBabyWalk.GetComponent<Rigidbody>().isKinematic = false;
        //_testBabyWalk.GetComponent<Rigidbody>().useGravity = true;
        /////////////////////////////////////////////////////////////////////
        _testBabyWalk.gameObject.layer = _takeBabyLayer;
        //_testBabyWalk.gameObject.layer = 13;
        _testBabyWalk.gameObject.GetComponent<BoxCollider>().isTrigger = true;
        _holdBaby.Baby.gameObject.transform.parent = BabyParent.gameObject.transform;
        //_holdBaby.Baby.gameObject.transform.DOJump(new Vector3(_holdBaby.BasePositionBaby.transform.position.x + OffsetBabyParentX, _holdBaby.BasePositionBaby.transform.position.y + OffsetBabyParentY, _holdBaby.BasePositionBaby.transform.position.z + OffsetBabyParentZ), 1, 1, _maxTimerBabyJump);
        _holdBaby.Baby.gameObject.transform.position = new Vector3(_holdBaby.BasePositionBaby.transform.position.x + OffsetBabyParentX, _holdBaby.BasePositionBaby.transform.position.y + OffsetBabyParentY, _holdBaby.BasePositionBaby.transform.position.z + OffsetBabyParentZ);
        TPBabyPos = new Vector3(_holdBaby.BasePositionBaby.transform.position.x + OffsetBabyParentX, _holdBaby.BasePositionBaby.transform.position.y + OffsetBabyParentY, _holdBaby.BasePositionBaby.transform.position.z + OffsetBabyParentZ);
        _holdBaby.IsOnHisBack = true;
        /////////////////////////////////////////////////////////////////////
        //_holdBaby.Baby.GetComponent<Rigidbody>().isKinematic = true;
        /////////////////////////////////////////////////////////////////////

        _holdBaby.CanHoldBaby = false;
        CanBabyFollow = false;
        //_testBabyWalk.transform.rotation = TransformRotationBaby.rotation;
        AnimatorHonkJR.SetBool("OnBack", true);
        /////////////////////////////////////////////////////////////////////
        //_testBabyWalk.GetComponent<CharacterController>().enabled = false;
        /////////////////////////////////////////////////////////////////////
        _soundsList.Tremblement.PlaySound();
        print("patate");
    }
    public void TeleportToSpawnPoint()
    {
        _testBabyWalk = FindAnyObjectByType<TestBabyWalk>();

        //if (_testBabyWalk == null)
        //{
        //    //Debug.Log("PATATE");
        //    _testBabyWalk = Instantiate(_babyPrefab, BabyParent.gameObject.transform);
        //}
        //GameObject toDelete = _testBabyWalk.gameObject;
        //_testBabyWalk = null;
        //Destroy(toDelete);
        //_testBabyWalk = Instantiate(_babyPrefab, BabyParent.gameObject.transform);
        //AnimatorHonkJR = _testBabyWalk.gameObject.GetComponentInChildren<Animator>();

        //_testBabyWalk.GetComponent<CharacterController>().enabled = false;
        _testBabyWalk.gameObject.transform.parent = BabyParent.gameObject.transform;
        _holdBaby.Baby.gameObject.transform.parent = BabyParent.gameObject.transform;
        _testBabyWalk = GetComponentInChildren<TestBabyWalk>();

        CharaController.enabled = false;
        transform.position = SpawnPoint.transform.position;
        CharaController.enabled = true;

        CanBabyFollow = false;
        AnimatorHonkJR.SetBool("IsActive", false);
        AnimatorHonkJR.SetTrigger("ChangingState");

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        //_testBabyWalk.GetComponent<Rigidbody>().isKinematic = false;
        //_testBabyWalk.GetComponent<Rigidbody>().useGravity = true;
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        _testBabyWalk.gameObject.layer = _takeBabyLayer;
        //_testBabyWalk.gameObject.layer = 13;
        _testBabyWalk.gameObject.GetComponent<BoxCollider>().isTrigger = true;

        //_holdBaby.Baby.gameObject.transform.parent = BabyParent.gameObject.transform;
        _holdBaby.Baby.gameObject.transform.position = new Vector3(_holdBaby.BasePositionBaby.transform.position.x + OffsetBabyParentX, _holdBaby.BasePositionBaby.transform.position.y + OffsetBabyParentY, _holdBaby.BasePositionBaby.transform.position.z + OffsetBabyParentZ);
        _holdBaby.IsOnHisBack = true;
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        //_holdBaby.Baby.GetComponent<Rigidbody>().isKinematic = true;
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        _holdBaby.CanHoldBaby = false;
        CanBabyFollow = false;
        //_testBabyWalk.GetComponent<CharacterController>().enabled = true;
        AnimatorHonkJR.SetBool("OnBack", true);

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
                FXGround.Play();
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
    private void TimerBabyJump()
    {
        if (_canTimerBabyJump)
        {
            _currentTimerBabyJump += Time.deltaTime;
            Direction = new Vector3(0, 0, 0);
        }
        if (_currentTimerBabyJump >= _maxTimerBabyJump)
        {
            _currentTimerBabyJump = 0;
            _canTimerBabyJump = false;
            CanMove = true;
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
        float veloBaby = 0;
        if (IsGrounded() && Velocity < 0f)
        {
            Velocity = -1f;
        }
        else
        {
            Velocity += _gravity * _gravityMultiplier * Time.deltaTime;
        }
        Direction.y = Velocity;

        if (_testBabyWalk.IsGrounded() && veloBaby < 0f)
        {
            veloBaby = -1f;
        }
        else
        {
            veloBaby += _gravity * _gravityMultiplier * Time.deltaTime;
        }
        _testBabyWalk.Direction.y = veloBaby;
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
            //if (_baby.GetComponent<TestBabyWalk>().CanBabyInputs && _holdBaby.IsOnHisBack == false && CanBabyFollow)
            //{
            //    _baby.GetComponent<CharacterController>().Move(WalkingSpeed * Time.deltaTime);
            //    _baby.GetComponent<CharacterController>().enabled = true;
            //}
            GetComponent<CharacterController>().enabled = true;
            ////////////////////////////////////////////////////////////////////////////////////////////
            if (IsGrounded())
            {
                if (_soundsList.WalkInCave)
                {
                    _soundsList.PawGrotteSoundRandom.PlaySound();
                }
                else if (_soundsList.WalkInGrass)
                {
                    _soundsList.PawGrassSoundRandom.PlaySound();
                }
                else if (_soundsList.WalkInSnow)
                {
                    _soundsList.PawSnowSoundRandom.PlaySound();
                }
            }
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
        _testBabyWalk = FindAnyObjectByType<TestBabyWalk>();
        TransformRotationBaby = _testBabyWalk.gameObject.transform;
        _soundsList = FindAnyObjectByType<Liste_sound>();
        //_baby = FindAnyObjectByType<Baby>();
    }
    private void Start()
    {
        CanPlayerUseInputs = true;
        if (_hadSpawnPoint)
        {
            TeleportToSpawnPoint();
        }
        ActualSpeed = BaseSpeed;
        IsWalkingBools();
        PlayerOriginRotation = ModelPlayer.transform.rotation;

        //_baby.GetComponent<BabyMovements>().enabled = false;
        CanBabyFollow = false;
        CanPushObstacles = false;
        CanMove = true;
    }
    private void FixedUpdate()
    {
        //CheckIsGroundedCoyauteJump();
        //ApplySpeed();
    }
    private void Update()
    {
        TimerCoolDownSlope += Time.deltaTime;
        WalkingSpeed = Direction * ActualSpeed;
        _currentTimer += Time.deltaTime;
        ApplyMovement();
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
        //TimerBabyJump();
        //_testBabyWalk = FindObjectOfType<TestBabyWalk>();
        //Debug.Log(_testBabyWalk);
    }
}