using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Stop_sound_start_timeline : MonoBehaviour
{
    public AudioSource audioSource; // La source audio à gérer
    public PlayableDirector playableDirector; // Le PlayableDirector de la Timeline
    public float fadeDuration = 1.0f; // Durée du fondu en secondes

    private bool isFading = false;
    private float fadeTimer = 0.0f;

    void Start()
    {
        if (playableDirector != null)
        {
            playableDirector.played += OnTimelinePlay;
        }
    }

    void Update()
    {
        if (isFading)
        {
            if (fadeTimer < fadeDuration)
            {
                fadeTimer += Time.deltaTime;
                float volume = Mathf.Lerp(1.0f, 0.0f, fadeTimer / fadeDuration);
                audioSource.volume = volume;
            }
            else
            {
                audioSource.Stop();
                isFading = false;
            }
        }
    }

    void OnTimelinePlay(PlayableDirector director)
    {
        if (audioSource.isPlaying)
        {
            isFading = true;
            fadeTimer = 0.0f;
        }
    }

    void OnDestroy()
    {
        if (playableDirector != null)
        {
            playableDirector.played -= OnTimelinePlay;
        }
    }
}
