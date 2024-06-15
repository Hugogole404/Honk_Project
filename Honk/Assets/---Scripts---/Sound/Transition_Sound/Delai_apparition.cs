using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delai_apparition : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private float delay = 2.0f; // Delai en secondes avant de d�marrer la musique

    private void Start()
    {
        Invoke("StartMusic", delay); // Appel la methode StartMusic() apr�s le d�lai specifie
    }

    private void StartMusic()
    {
        if (musicSource != null)
        {
            musicSource.Play(); // Demarrage de la musique
        }
        else
        {
            Debug.LogWarning("AudioSource is not assigned."); // Avertissement si lAudioSource nest pas assigne
        }
    }
}
