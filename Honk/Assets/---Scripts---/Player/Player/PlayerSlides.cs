using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSlides : MonoBehaviour
{
    #region Variables
    [Header("Sliding")]
    public float slopeMaxAngle;
    public float slopeMinAngle;
    [Range(0.0f, 1.0f)][SerializeField] private float slidingSpeed;

    public float SlideMultiplicator = 1;
    public float ForceSlideTime = 1f;
    public float DownSpeed;
    public float UpSpeed;
    public Vector3 CurrentSpeed;
    [SerializeField] private float _slideBoost;
    [SerializeField] private float _snowFriction;
    private float _speed;
    private float _slopeAngle;
    private float _forceSlideCounter;
    private Vector3 _normalAngle;
    private Vector3 _slidingDir;
    private RaycastHit _hit;

    public float AngleSlide;
    public float BaseSpeedSlide;
    private PlayerSwim _playerSwim;
    private PlayerMovements _playerMovement;
    #endregion

    public void Slide(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            return;
        }

        //_playerMovement.transform.rotation = Quaternion.EulerRotation(AngleSlide, _playerMovement.transform.rotation.y, _playerMovement.transform.rotation.z);

        if (context.performed && !_playerMovement.IsSwimming)
        {
            _playerMovement.IsSlidingBools();
        }
        if (context.canceled)
        {
            _playerMovement.IsWalkingBools();
        }
    }

    private void OnSlide()
    {
        float speedAngle = _slopeAngle - 90;
        transform.rotation = Quaternion.LookRotation(new Vector3(_slidingDir.x, -90 + -_normalAngle.z, _slidingDir.z));
        _speed = _slopeAngle * Mathf.Deg2Rad * slidingSpeed * SlideMultiplicator;

        _normalAngle = new Vector3(_hit.normal.x, Mathf.Max(_hit.normal.y - 1.5f, -1f), _hit.normal.z);
        CurrentSpeed = _normalAngle * _speed + CurrentSpeed / _snowFriction;

        _playerMovement.CharacterController.Move((CurrentSpeed * Time.deltaTime));
        _slidingDir = CurrentSpeed.normalized;

        Vector3 positionActuel = transform.position;

        if (positionActuel.y > _playerMovement.LastPos.y)
        {
            SlideMultiplicator = UpSpeed;
        }
        else
        {
            SlideMultiplicator = DownSpeed;
        }

        _playerMovement.LastPos = positionActuel;

        Debug.DrawLine(transform.position, transform.position + _normalAngle * 8, Color.red, 8f);
        if (_playerMovement.IsSliding)
        {
            if (_slopeAngle <= slopeMinAngle * Mathf.Deg2Rad)
            {
                _forceSlideCounter -= Time.deltaTime;
                CurrentSpeed = CurrentSpeed / 1.005f;
                if (_forceSlideCounter < 0.0f)
                {
                    _playerMovement.IsSliding = false;
                    CurrentSpeed = Vector3.zero;
                    //transform.rotation = Quaternion.LookRotation(Vector3.zero);
                }
            }
            else
            {
                _forceSlideCounter = ForceSlideTime;

            }
            var targetAngle = Mathf.Atan2(_slidingDir.x, _slidingDir.z) * Mathf.Rad2Deg;
            var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _playerMovement.CurrentVelocity, 0.05f);
            transform.rotation = Quaternion.Euler(transform.rotation.x, angle, 0.0f);
        }
    }
    private bool DoIStartSlide(RaycastHit hit)
    {

        if (hit.collider.gameObject.tag == "Ground")
        {
            float slopeAngle = Mathf.Deg2Rad * Vector3.Angle(Vector3.up, hit.normal);
            //Debug.Log(SlopeAngle);

            float radius = Mathf.Abs(transform.position.y / Mathf.Sin(slopeAngle)); //peux causer bug
            if (slopeAngle >= slopeMaxAngle * Mathf.Deg2Rad)
            {
                _playerMovement.IsSliding = true;
                return true;
            }
        }
        return false;
    }

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovements>();
        _playerSwim = GetComponent<PlayerSwim>();
    }
    private void Start()
    {
        _forceSlideCounter = ForceSlideTime;
    }
    private void FixedUpdate()
    {
        //_slopeAngle = Mathf.Deg2Rad * Vector3.Angle(Vector3.up, _hit.normal);
        Ray myRay = new Ray(transform.position, Vector3.down);

        if (_playerMovement.IsGrounded())
        {
            if (Physics.Raycast(myRay, out _hit, 10))
            {
                DoIStartSlide(_hit);
            }
        }

        if (_playerMovement.IsSliding == true)
        {
            //OnSlide();
            //if (_gamepad != null)
            //{
            //    _gamepad.SetMotorSpeeds(0.075f, 0.134f);
            //}

        }
    }
}