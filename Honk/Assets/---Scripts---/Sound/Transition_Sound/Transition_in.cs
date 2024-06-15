using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition_in : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private float fadeInDuration = 2.0f; // Duree du fondu en secondes

    private void Start()
    {
        if (musicSource != null)
        {
            float targetVolume = musicSource.volume; // Recupere le volume défini dans l'editeur
            musicSource.volume = 0; // Commence avec le volume à 0
            musicSource.Play(); // Demarre la musique
            StartCoroutine(FadeInMusic(targetVolume));
        }
    }

    private IEnumerator FadeInMusic(float targetVolume)
    {
        float startVolume = 0.0f;
        float elapsedTime = 0.0f;

        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsedTime / fadeInDuration);
            yield return null;
        }

        musicSource.volume = targetVolume; // Assure que le volume soit a la valeur definie à la fin
    }
}
