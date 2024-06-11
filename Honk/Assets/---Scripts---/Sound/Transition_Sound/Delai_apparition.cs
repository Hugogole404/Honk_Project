using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delai_apparition : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private float delay = 2.0f; // D�lai en secondes avant de d�marrer la musique

    private void Start()
    {
        Invoke("StartMusic", delay); // Appel la m�thode StartMusic() apr�s le d�lai sp�cifi�
    }

    private void StartMusic()
    {
        if (musicSource != null)
        {
            musicSource.Play(); // D�marrage de la musique
        }
        else
        {
            Debug.LogWarning("AudioSource is not assigned."); // Avertissement si l'AudioSource n'est pas assign�
        }
    }
}
