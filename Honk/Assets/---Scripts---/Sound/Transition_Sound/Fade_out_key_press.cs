using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade_out_key_press : MonoBehaviour
{
    public AudioSource[] audioSources; // Tableau des AudioSources à gérer
    public float fadeDuration = 2.0f;  // Durée du fade out en secondes

    private bool isFading = false;

    void Update()
    {
        if (Input.anyKeyDown && !isFading)
        {
            StartCoroutine(FadeOutAll(fadeDuration));
        }
    }

    private IEnumerator FadeOutAll(float fadeDuration)
    {
        isFading = true;
        float[] startVolumes = new float[audioSources.Length];

        // Récupérer les volumes initiaux
        for (int i = 0; i < audioSources.Length; i++)
        {
            startVolumes[i] = audioSources[i].volume;
        }

        // Réduire progressivement le volume de chaque AudioSource
        while (true)
        {
            bool allAudiosStopped = true;
            for (int i = 0; i < audioSources.Length; i++)
            {
                if (audioSources[i].volume > 0)
                {
                    audioSources[i].volume -= startVolumes[i] * Time.deltaTime / fadeDuration;
                    allAudiosStopped = false;
                }
            }

            if (allAudiosStopped)
            {
                break;
            }

            yield return null;
        }

        // Arrêter toutes les AudioSources et réinitialiser leurs volumes
        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].Stop();
            audioSources[i].volume = startVolumes[i];
        }

        isFading = false;
    }
}
