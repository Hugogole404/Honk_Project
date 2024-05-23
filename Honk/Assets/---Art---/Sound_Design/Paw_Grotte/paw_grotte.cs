using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class paw_grotte : MonoBehaviour
{
    public AudioSource AudioSourceSound;
    public AudioClip[] paw_grotte_sounds;

    void Start()
    {
        // assigner les sons de pas dans l'éditeur Unity
        AudioSourceSound = GetComponent<AudioSource>();
    }
    public void PlaySound()
    {
        if (!AudioSourceSound.isPlaying && (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0))
        {
            // son de pas aléatoire
            int randomIndex = Random.Range(0, paw_grotte_sounds.Length);
            AudioSourceSound.clip = paw_grotte_sounds[randomIndex];

            // Jouez le son choisi
            AudioSourceSound.Play();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            // Arrête le son du pas si la barre espace est enfoncée
            AudioSourceSound.Stop();
        }
    }
}