using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paw : MonoBehaviour
{
    public AudioSource AudioSourceSound;
    public AudioClip[] paw_snow_sounds;

    void Start()
    {
        // assigner les sons de pas dans l'�diteur Unity
        AudioSourceSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!AudioSourceSound.isPlaying && (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0))
        {
            // son de pas al�atoire
            int randomIndex = Random.Range(0, paw_snow_sounds.Length);
            AudioSourceSound.clip = paw_snow_sounds[randomIndex];

            // Jouez le son choisi
            AudioSourceSound.Play();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            // Arr�te le son du pas si la barre espace est enfonc�e
            AudioSourceSound.Stop();
        }
    }
}
