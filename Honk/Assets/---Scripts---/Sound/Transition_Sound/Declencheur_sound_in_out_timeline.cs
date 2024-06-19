using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Declencheur_sound_in_out_timeline : MonoBehaviour
{
    public AudioSource audioSource; // Référence à l'AudioSource à gérer
    public PlayableDirector playableDirector; // Référence au PlayableDirector

    void Start()
    {
        if (playableDirector != null)
        {
            // S'abonner aux événements played et stopped du PlayableDirector
            playableDirector.played += OnPlayableDirectorPlayed;
            playableDirector.stopped += OnPlayableDirectorStopped;
        }
    }

    void OnPlayableDirectorPlayed(PlayableDirector director)
    {
        if (audioSource != null)
        {
            // Désactiver l'AudioSource lorsque la timeline commence
            audioSource.enabled = false;
        }
    }

    void OnPlayableDirectorStopped(PlayableDirector director)
    {
        if (audioSource != null)
        {
            // Réactiver l'AudioSource lorsque la timeline se termine
            audioSource.enabled = true;
        }
    }

    void OnDestroy()
    {
        if (playableDirector != null)
        {
            // Se désabonner des événements pour éviter les erreurs
            playableDirector.played -= OnPlayableDirectorPlayed;
            playableDirector.stopped -= OnPlayableDirectorStopped;
        }
    }
}
