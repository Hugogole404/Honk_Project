using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSlides : MonoBehaviour
{
    public bool IsSliding;
    public float AngleSlide;
    private PlayerMovements _playerMovement;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovements>();
    }
    public void Slide(InputAction.CallbackContext context)
    {
        if(!context.performed)
        {
            return;
        }
        _playerMovement.transform.rotation = Quaternion.EulerRotation(AngleSlide, _playerMovement.transform.rotation.y, _playerMovement.transform.rotation.z);
    }
}