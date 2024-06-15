using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Transition_in_game : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private float fadeDuration = 1.0f;
    [SerializeField] private float delayBeforeFadeIn = 2.0f; // Delai avant le debut du fondu en reprise
    [SerializeField] private PlayableDirector playableDirector;

    private void Start()
    {
        if (playableDirector != null)
        {
            playableDirector.played += OnPlayableDirectorPlayed;
            playableDirector.stopped += OnPlayableDirectorStopped;
        }
    }

    private void OnDestroy()
    {
        if (playableDirector != null)
        {
            playableDirector.played -= OnPlayableDirectorPlayed;
            playableDirector.stopped -= OnPlayableDirectorStopped;
        }
    }

    private void OnPlayableDirectorPlayed(PlayableDirector director)
    {
        if (director == playableDirector)
        {
            Debug.Log("Timeline started, fading out music."); // Debug message
            StartCoroutine(FadeOutMusic());
        }
    }

    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        if (director == playableDirector)
        {
            Debug.Log("Timeline stopped, waiting to fade in music."); // Debug message
            StartCoroutine(DelayedFadeInMusic());
        }
    }

    private IEnumerator FadeOutMusic()
    {
        float startVolume = musicSource.volume;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        musicSource.volume = 0;
        musicSource.Pause();
    }

    private IEnumerator DelayedFadeInMusic()
    {
        // Attendre avant de commencer le fondu
        yield return new WaitForSeconds(delayBeforeFadeIn);
        Debug.Log("Delay complete, starting fade in music."); // Debug message

        musicSource.UnPause();  // Unpause instead of Play to keep position
        musicSource.volume = 0;

        float targetVolume = 1.0f;  // Define your target volume

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(0, targetVolume, t / fadeDuration);
            yield return null;
        }

        musicSource.volume = targetVolume;
    }
}
