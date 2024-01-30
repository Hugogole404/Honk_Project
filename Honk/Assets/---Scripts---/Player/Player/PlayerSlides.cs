using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using VolumetricLightsDemo;

public class PlayerSlides : MonoBehaviour
{
    [Header("Sliding")]
    public float slopeMaxAngle;
    public float slopeMinAngle;
    [Range(0.0f, 1.0f)][SerializeField] private float slidingSpeed;

    [HideInInspector] public bool isPressed = false;
    public Vector3 currentSpeed;
    public bool isSliding = false;
    public float slideMult = 1;
    public float forceSlideTime = 1f;
    public float downSpeed;
    public float upSpeed;
    public float _currentVelocity;
    private float slopeSpeed;
    private float speed;
    private float speedDecrease;
    private float slopeAngle;
    private float forceSlideCounter;
    private Vector3 normalAngle;
    private Vector3 speedGain;
    private Vector3 _slideBSpeed;
    private Vector3 _slidingDir;
    private Vector3 _lastPos;
    [SerializeField] private float _slideBoost;
    [SerializeField] private float snowFriction;
    private RaycastHit _hit;
    private PlayerSwim _playerSwim;

    public bool IsSliding;
    public float AngleSlide;
    private PlayerMovements _playerMovement;
    public float BaseSpeedSlide;

    private void ApplyMovement()
    {
        if (IsSliding)
        {
            _playerMovement.CharacterController.Move(_playerMovement.Direction * _playerMovement.ActualSpeed * Time.deltaTime);
        }
    }
    public void Slide(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            //_playerMovement.IsWaking = true;
            //_playerMovement.IsSliding = false;
            return;
        }
        _playerMovement.IsWaking = false;
        //_playerMovement.IsSliding = true;
        _playerMovement.transform.rotation = Quaternion.EulerRotation(AngleSlide, _playerMovement.transform.rotation.y, _playerMovement.transform.rotation.z);
        //_playerMovement.Direction = new Vector3(-_playerMovement.Direction.x * BaseSpeedSlide * _playerMovement.Velocity, _playerMovement.transform.position.y, -_playerMovement.Direction.z * BaseSpeedSlide * _playerMovement.Velocity);




        if (context.started && !_playerMovement.IsSwimming)
        {
            isSliding = true;
            isPressed = true;
            _slideBSpeed = _playerMovement.CharacterController.velocity.normalized * BaseSpeedSlide;
            currentSpeed = new Vector3(_slideBSpeed.x * _slideBoost, 0, _slideBSpeed.z * _slideBoost);
            transform.rotation = Quaternion.Euler(90f, 0, 0);
        }
        if (context.canceled)
        {
            isSliding = false;
            isPressed = false;
            currentSpeed = Vector3.zero;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovements>();
        _playerSwim = GetComponent<PlayerSwim>();
    }
    void Start()
    {
        forceSlideCounter = forceSlideTime;
    }
    private void FixedUpdate()
    {
        ApplyMovement();

        slopeAngle = Mathf.Deg2Rad * Vector3.Angle(Vector3.up, _hit.normal);
        Ray myRay = new Ray(transform.position, Vector3.down);

        if (_playerMovement.CharacterController.isGrounded)
        {
            if (Physics.Raycast(myRay, out _hit, 10))
            {
                //DoIStartSlide(_hit);
            }
        }

        if (isSliding == true)
        {
            OnSlide();
            //if (_gamepad != null)
            //{
            //    _gamepad.SetMotorSpeeds(0.075f, 0.134f);
            //}

        }
    }

    public void OnSlide()
    {

        float speedAngle = slopeAngle - 90;
        transform.rotation = Quaternion.LookRotation(new Vector3(_slidingDir.x, -90 + -normalAngle.z, _slidingDir.z));
        speed = slopeAngle * Mathf.Deg2Rad * slidingSpeed * slideMult;

        normalAngle = new Vector3(_hit.normal.x, Mathf.Max(_hit.normal.y - 1.5f, -1f), _hit.normal.z);
        currentSpeed = normalAngle * speed + currentSpeed / snowFriction;
        //if (_playerMovement.CharacterController.isGrounded)
        //{

        //}
        _playerMovement.CharacterController.Move((currentSpeed * Time.deltaTime));
        _slidingDir = currentSpeed.normalized;

        Vector3 positionActuel = transform.position;

        if (positionActuel.y > _lastPos.y)
        {
            slideMult = upSpeed;
        }
        else
        {
            slideMult = downSpeed;
        }

        _lastPos = positionActuel;

        Debug.DrawLine(transform.position, transform.position + normalAngle * 8, Color.red, 8f);
        if (isSliding && isPressed == false)
        {
            if (slopeAngle <= slopeMinAngle * Mathf.Deg2Rad)
            {
                Debug.Log(forceSlideCounter);
                forceSlideCounter -= Time.deltaTime;
                currentSpeed = currentSpeed / 1.005f;
                if (forceSlideCounter < 0.0f)
                {
                    isSliding = false;
                    currentSpeed = Vector3.zero;
                    transform.rotation = Quaternion.LookRotation(Vector3.zero);
                }
            }
            else
            {
                forceSlideCounter = forceSlideTime;

            }
            //var targetAngle = Mathf.Atan2(slidingDir.x, slidingDir.z) * Mathf.Rad2Deg;
            //var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, 0.05f);
            //transform.rotation = Quaternion.Euler(transform.rotation.x, angle, 0.0f);
        }

        //private bool DoIStartSlide(RaycastHit hit)
        //{

        //    if (hit.collider.gameObject.tag == "Ground")
        //    {
        //        slopeAngle = Mathf.Deg2Rad * Vector3.Angle(Vector3.up, hit.normal);
        //        //Debug.Log(SlopeAngle);

        //        float radius = Mathf.Abs(transform.position.y / Mathf.Sin(slopeAngle)); //peux causer bug
        //        if (slopeAngle >= slopeMaxAngle * Mathf.Deg2Rad)
        //        {
        //            isSliding = true;
        //            return true;
        //        }
        //    }
        //    return false;
        //}
    }
}