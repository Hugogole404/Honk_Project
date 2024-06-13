using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ouverture_sound : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource; // Assignez ici votre AudioSource dans l'inspecteur
    [SerializeField] private float fadeDuration = 2f; // Durée du fondu en secondes
    [SerializeField] private float delay = 2f; // Délai avant le début du fondu

    private float targetVolume; // Volume cible (le volume assigné à l'AudioSource)

    private void Start()
    {
        if (musicSource != null)
        {
            targetVolume = musicSource.volume;
            musicSource.volume = 0f;
            musicSource.Play();
            StartCoroutine(FadeInMusicWithDelay());
        }
    }

    private IEnumerator FadeInMusicWithDelay()
    {
        // Attendre le délai spécifié avant de commencer le fondu
        yield return new WaitForSeconds(delay);

        float startTime = Time.time;

        while (Time.time < startTime + fadeDuration)
        {
            musicSource.volume = Mathf.Lerp(0f, targetVolume, (Time.time - startTime) / fadeDuration);
            yield return null;
        }

        musicSource.volume = targetVolume; //  le volume est au volume cible après le fondu
    }
}
