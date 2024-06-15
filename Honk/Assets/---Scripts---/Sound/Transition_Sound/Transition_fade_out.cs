using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition_fade_out : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private float fadeDuration = 1.0f;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision detected with: " + other.gameObject.name); // Message de debogage

        if (other.gameObject.name == "PlayerWalkModif")
        {
            Debug.Log("Collision with PlayerWalkModif detected."); // Message de debogage
            StartCoroutine(FadeOutMusic());
        }
        else if (other.gameObject.name == "BallSlopePlayer")
        {
            Debug.Log("Collision with BallSlopePlayer detected."); // Message de debogage
            StartCoroutine(FadeOutMusic());
        }
    }

    private IEnumerator FadeOutMusic()
    {
        Debug.Log("Starting fade out music."); // Message de debogage
        float startVolume = musicSource.volume;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
            yield return null;
        }

        musicSource.volume = 0;
        musicSource.Stop();
        Debug.Log("Music faded out and stopped."); // Message de debogage
    }
}
