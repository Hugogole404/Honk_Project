using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Declenchement_sound_buisson : MonoBehaviour
{
    public AudioClip[] soundClips;  // Tableau de clips audio � jouer al�atoirement
    private AudioSource audioSource;  // La source audio qui jouera le son

    void Start()
    {
        // Ajouter un composant AudioSource au GameObject s'il n'y en a pas d�j� un
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        // V�rifier si l'objet entrant est "PlayerWalkModif"
        if (other.gameObject.name == "PlayerWalkModif")
        {
            // Jouer un son al�atoire
            PlayRandomSound();
        }
    }

    void PlayRandomSound()
    {
        if (soundClips.Length > 0)
        {
            // S�lectionner un clip al�atoire � partir du tableau
            int randomIndex = Random.Range(0, soundClips.Length);
            audioSource.clip = soundClips[randomIndex];
            audioSource.Play();
        }
    }
}
