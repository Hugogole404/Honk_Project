using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Play_Awake : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;

    private void Start()
    {
        if (musicSource != null)
        {
            // Assurez-vous que le clip audio est assigné
            if (musicSource.clip == null)
            {
                Debug.LogError("Aucun clip audio assigné à l'AudioSource.");
                return;
            }

            // Réinitialiser la position de lecture
            musicSource.Stop();
            musicSource.time = 0;

            // Commencer la lecture immédiatement
            musicSource.Play();

            Debug.Log("Musique démarrée depuis le début.");
        }
        else
        {
            Debug.LogError("Aucune AudioSource assignée.");
        }
    }
}
