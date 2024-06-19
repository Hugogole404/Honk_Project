using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Declencheur_sound_in_out_timeline : MonoBehaviour
{
    public AudioSource audioSource; // R�f�rence � l'AudioSource � g�rer
    public PlayableDirector playableDirector; // R�f�rence au PlayableDirector

    void Start()
    {
        if (playableDirector != null)
        {
            // S'abonner aux �v�nements played et stopped du PlayableDirector
            playableDirector.played += OnPlayableDirectorPlayed;
            playableDirector.stopped += OnPlayableDirectorStopped;
        }
    }

    void OnPlayableDirectorPlayed(PlayableDirector director)
    {
        if (audioSource != null)
        {
            // D�sactiver l'AudioSource lorsque la timeline commence
            audioSource.enabled = false;
        }
    }

    void OnPlayableDirectorStopped(PlayableDirector director)
    {
        if (audioSource != null)
        {
            // R�activer l'AudioSource lorsque la timeline se termine
            audioSource.enabled = true;
        }
    }

    void OnDestroy()
    {
        if (playableDirector != null)
        {
            // Se d�sabonner des �v�nements pour �viter les erreurs
            playableDirector.played -= OnPlayableDirectorPlayed;
            playableDirector.stopped -= OnPlayableDirectorStopped;
        }
    }
}
