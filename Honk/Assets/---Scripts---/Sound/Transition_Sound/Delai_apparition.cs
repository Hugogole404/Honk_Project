using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delai_apparition : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private float delay = 2.0f; // Délai en secondes avant de démarrer la musique

    private void Start()
    {
        Invoke("StartMusic", delay); // Appel la méthode StartMusic() après le délai spécifié
    }

    private void StartMusic()
    {
        if (musicSource != null)
        {
            musicSource.Play(); // Démarrage de la musique
        }
        else
        {
            Debug.LogWarning("AudioSource is not assigned."); // Avertissement si l'AudioSource n'est pas assigné
        }
    }
}
