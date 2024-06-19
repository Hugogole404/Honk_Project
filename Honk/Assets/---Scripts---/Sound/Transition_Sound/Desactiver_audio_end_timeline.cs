using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Desactiver_audio_end_timeline : MonoBehaviour
{
    public PlayableDirector HonkJr_WalkingTimeline;
    public AudioSource audioSource1;
    public AudioSource audioSource2;
    public float fadeDuration = 1.0f; // Durée du fondu en secondes

    void Start()
    {
        // Abonnez-vous à l'événement "stopped" du PlayableDirector
        if (HonkJr_WalkingTimeline != null)
        {
            HonkJr_WalkingTimeline.stopped += OnTimelineStopped;
        }
        else
        {
            Debug.LogError("HonkJr_WalkingTimeline n'est pas assigné !");
        }
    }

    void OnDestroy()
    {
        // Désabonnez-vous de l'événement "stopped" du PlayableDirector
        if (HonkJr_WalkingTimeline != null)
        {
            HonkJr_WalkingTimeline.stopped -= OnTimelineStopped;
        }
    }

    void OnTimelineStopped(PlayableDirector director)
    {
        StartCoroutine(FadeOutAudioSources());
    }

    private IEnumerator FadeOutAudioSources()
    {
        float startVolume1 = audioSource1.volume;
        float startVolume2 = audioSource2.volume;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource1.volume = Mathf.Lerp(startVolume1, 0, t / fadeDuration);
            audioSource2.volume = Mathf.Lerp(startVolume2, 0, t / fadeDuration);
            yield return null;
        }

        audioSource1.volume = 0;
        audioSource2.volume = 0;
        audioSource1.Stop();
        audioSource2.Stop();
    }
}
