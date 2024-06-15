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
            // Assurez-vous que le clip audio est assign�
            if (musicSource.clip == null)
            {
                Debug.LogError("Aucun clip audio assign� � l'AudioSource.");
                return;
            }

            // R�initialiser la position de lecture
            musicSource.Stop();
            musicSource.time = 0;

            // Commencer la lecture imm�diatement
            musicSource.Play();

            Debug.Log("Musique d�marr�e depuis le d�but.");
        }
        else
        {
            Debug.LogError("Aucune AudioSource assign�e.");
        }
    }
}
