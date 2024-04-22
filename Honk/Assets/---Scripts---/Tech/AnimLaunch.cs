using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimLaunch : MonoBehaviour
{
    [SerializeField] string _animationNameTrigger;
    [SerializeField] Animator _animator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>() != null)
        {
            _animator.SetBool(_animationNameTrigger, true);
        }
    }
}
