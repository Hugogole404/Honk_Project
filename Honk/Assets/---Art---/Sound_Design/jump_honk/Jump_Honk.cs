using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump_Honk : MonoBehaviour
{
    public AudioSource AudioSourceSound;
    public AudioClip[] jump;

    void Start()
    {
        // assigner les sons de pas dans l'éditeur Unity
        AudioSourceSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            // son de pas aleatoire
            int randomIndex = Random.Range(0, jump.Length);
            AudioSourceSound.clip = jump[randomIndex];

            // Jouez le son choisi
            AudioSourceSound.Play();
        }
    }
}