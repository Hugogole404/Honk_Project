using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PLayerRigid : MonoBehaviour
{
    public Rigidbody Rb;

    public bool IsWaking;
    public bool IsSliding;
    public bool IsSwimming;

    public Vector3 Direction;

    public Vector2 Input, look;

    public float GroundDetection;
    public float Speed;

    public float _maxTimer;
    private float _currentTimer;
    private bool _canJump;
    private float _jumpPower;
    [SerializeField] private float _smoothTime;
    [SerializeField] public float CurrentVelocity;
    public GameObject Player;
    public float MaxForce;

    public void Move(InputAction.CallbackContext context)
    {
        if (IsWaking)
        {
            Input = context.ReadValue<Vector2>();
            //Direction = new Vector3(Input.x, Direction.y, Input.y);
        }
        if (IsSliding)
        {
            Input = context.ReadValue<Vector2>();
        }
        if (IsSwimming)
        {
            Input = context.ReadValue<Vector2>();
            //Direction = new Vector3(Input.x, Direction.y, Input.y) / 3;
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
            Rb.AddForce(0, _jumpPower, 0);
        }
    }
    public void Slide(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsSliding = true;
            IsWaking = false;
            IsSwimming = false;

            //Rb.AddForce(new Vector3(Direction.x * Speed * Time.deltaTime, transform.position.y, Direction.z * Speed * Time.deltaTime));
        }
        if (context.canceled)
        {
            IsSliding = false;
            IsWaking = true;
            IsSwimming = false;
        }
    }
    public bool IsGrounded()
    {
        RaycastHit hit;
        GroundDetection = 1.1f;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, GroundDetection))
        {
            return true;
        }
        return false;
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
            var targetAngle = Mathf.Atan2(Direction.x, Direction.z) * Mathf.Rad2Deg;
            var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref CurrentVelocity, _smoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }
    private void ApplyMovement()
    {
        //if (IsWaking)
        //{
        //    Player.Move(Speed * Time.deltaTime);
        //}
        //if (IsSliding)
        //{
        //    CharaController.Move(Speed * Time.deltaTime);
        //}
        //if (IsSwimming)
        //{
        //    CharaController.Move(new Vector3(Direction.x * Speed / 10, CurrentVelocity, Direction.z * Speed / 10) * Speed * Time.deltaTime);
        //}
        Vector3 currentVelo = Rb.velocity;
        Vector3 targetVelo = new Vector3(Input.x, /*transform.position.y*/-0.1f, Input.y);
        targetVelo *= Speed;
        targetVelo = transform.TransformDirection(targetVelo);
        Vector3 veloChange = targetVelo - currentVelo;
        Vector3.ClampMagnitude(veloChange, MaxForce);
        Rb.AddForce(veloChange, ForceMode.VelocityChange);
    }


    private void Start()
    {
        IsWaking = true;
    }
    private void Update()
    {
        //ApplyRotation();
        ApplyMovement();
    }
}