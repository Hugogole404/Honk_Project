using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paw : MonoBehaviour
{
    public AudioSource[] paw_snow_sounds;

    void Start()
    {
        // assigner les sons de pas dans l'éditeur Unity
        paw_snow_sounds = GetComponents<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D))
        {
            // son de pas aleatoire
            int randomIndex = Random.Range(0, paw_snow_sounds.Length);
            AudioSource randomSound = paw_snow_sounds[randomIndex];

            // Jouez le son choisi
            randomSound.Play();
        }
    }
}
