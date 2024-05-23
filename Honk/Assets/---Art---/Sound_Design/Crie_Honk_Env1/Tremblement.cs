using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Tremblement : MonoBehaviour
{
    public AudioSource AudioSourceSound;
    public AudioClip[] tremblement;

    void Start()
    {
        // assigner les sons de pas dans l'éditeur Unity
        AudioSourceSound = GetComponent<AudioSource>();
    }
    public void PlaySound()
    {
        int randomIndex = Random.Range(0, tremblement.Length);
        AudioSourceSound.clip = tremblement[randomIndex];

        // Jouez le son choisi
        AudioSourceSound.Play();
    }
}
