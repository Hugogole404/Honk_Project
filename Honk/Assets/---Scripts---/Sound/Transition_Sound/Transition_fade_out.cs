using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition_fade_out : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private float fadeDuration = 1.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PlayerWalkModif")
        {
            StartCoroutine(FadeOutMusic());
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
        musicSource.Stop();
    }
}
