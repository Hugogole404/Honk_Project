using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineTrigger : MonoBehaviour
{
    public PlayableDirector playableDirector;

    private void OnTriggerEnter(Collider other)
    {
        // Vérifie si l'objet entrant est le joueur, ou ajoute une condition spécifique ici
        if (other.CompareTag("Player"))
        {
            if (playableDirector != null)
            {
                playableDirector.Play();
                
            }
            else
            {
                Debug.LogError("PlayableDirector is not assigned!");
            }

        }


    }
    void OnPlayableDirectorStopped(PlayableDirector director)
    {
        if (director == playableDirector)
        {
            
        }
    }
}

