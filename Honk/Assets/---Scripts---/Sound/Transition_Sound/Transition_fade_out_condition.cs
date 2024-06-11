using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition_fade_out_condition : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private float fadeDuration = 1.0f;
    [SerializeField] private float fadeInSpeed = 1.0f;

    private bool isFadingOut = false;
    private bool isFadingIn = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PlayerWalkModif")
        {
            isFadingOut = true;
            isFadingIn = false;
            StartCoroutine(FadeOutMusic());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "PlayerWalkModif")
        {
            isFadingOut = false;
            isFadingIn = true;
            StartCoroutine(FadeInMusic());
        }
    }

    private IEnumerator FadeOutMusic()
    {
        float startVolume = musicSource.volume;

        while (musicSource.volume > 0 && isFadingOut)
        {
            musicSource.volume -= Time.deltaTime / fadeDuration;
            yield return null;
        }

        musicSource.volume = 0;

        if (isFadingOut)
        {
            musicSource.Pause(); // Met en pause la musique au lieu de l'arrêter complètement
        }
    }

    private IEnumerator FadeInMusic()
    {
        musicSource.UnPause(); // Reprend la lecture de la musique
        musicSource.volume = 0;

        while (musicSource.volume < 1 && isFadingIn)
        {
            musicSource.volume += Time.deltaTime / fadeInSpeed;
            yield return null;
        }

        musicSource.volume = 1;
    }
}
