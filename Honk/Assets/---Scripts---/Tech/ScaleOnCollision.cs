using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScaleOnCollision : MonoBehaviour
{

    public Vector3 targetScale = new Vector3(2f, 2f, 2f);
    public float duration = 1f;
    public float returnDelay = 2f;
    public ParticleSystem FXleaf;

    private Vector3 initialScale;

    private void Start()
    {
        initialScale = transform.localScale;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {

            

            Sequence mySequence = DOTween.Sequence();

            mySequence.Append(transform.DOScale(targetScale, duration));
            FXleaf.Play();
            mySequence.AppendInterval(returnDelay);
            mySequence.Append(transform.DOScale(initialScale, duration));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        { 
            
            //FXleaf.Play();
            
            //Sequence mySequence = DOTween.Sequence();

            //mySequence.Append(transform.DOScale(targetScale, duration));
            //mySequence.AppendInterval(returnDelay);
            //mySequence.Append(transform.DOScale(initialScale, duration));
        }
    }
}
