using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSlides : MonoBehaviour
{
    public bool IsSliding;
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
        Debug.Log("Slide");
    }
}