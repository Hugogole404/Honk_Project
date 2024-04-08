using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    
    public CharacterController CharaController;

    // Start is called before the first frame update
    private void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
    }
    // Update is called once per frame

    public Animator m_Animator;

    void Update()
    {
        //A mettre dans l'update de base
        if (CharaController.isGrounded == true && m_Animator.GetBool("isJumping") == true)
        {
            m_Animator.SetBool("isJumping", false);
        }
        //A mettre au moment de l'input du jump
        m_Animator.SetBool("isJumping", true);

        // A mettre au moment de l'input de marche
        m_Animator.SetBool("isMoving", true);

        // A mettre au moment de l'arret de l'input de marche
        m_Animator.SetBool("isMoving", false);
    }
}