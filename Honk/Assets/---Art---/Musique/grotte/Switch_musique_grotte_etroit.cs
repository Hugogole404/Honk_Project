using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch_musique_grotte_etroit : MonoBehaviour
{
    public AudioSource mainMusic;
    public AudioSource secondaryMusic;
    public float fadeDuration = 1.0f;
    public string playerName = "PlayerWalkModif"; // Le nom de l'objet du joueur

    private float mainMusicInitialVolume;
    private float secondaryMusicInitialVolume;

    void Start()
    {
        // Enregistrer les volumes initiaux des musiques
        mainMusicInitialVolume = mainMusic.volume;
        secondaryMusicInitialVolume = secondaryMusic.volume;

        // Assurez-vous que les musiques commencent en boucle
        mainMusic.loop = true;
        secondaryMusic.loop = true;

        // Démarrer les musiques
        mainMusic.Play();
        secondaryMusic.Play();

        // Mettre le volume de la musique secondaire à 0 au début
        secondaryMusic.volume = 0f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == playerName)
        {
            StopCoroutine("FadeOutMusic");
            StartCoroutine(FadeOutMusic(mainMusic, fadeDuration));
            StopCoroutine("FadeInMusic");
            StartCoroutine(FadeInMusic(secondaryMusic, fadeDuration, secondaryMusicInitialVolume));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == playerName)
        {
            StopCoroutine("FadeOutMusic");
            StartCoroutine(FadeOutMusic(secondaryMusic, fadeDuration));
            StopCoroutine("FadeInMusic");
            StartCoroutine(FadeInMusic(mainMusic, fadeDuration, mainMusicInitialVolume));
        }
    }

    private IEnumerator FadeOutMusic(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / duration);
            yield return null;
        }

        audioSource.volume = 0;
    }

    private IEnumerator FadeInMusic(AudioSource audioSource, float duration, float targetVolume)
    {
        float startVolume = audioSource.volume;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, t / duration);
            yield return null;
        }

        audioSource.volume = targetVolume;
    }
}
