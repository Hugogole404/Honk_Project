using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Declencheur_sound : MonoBehaviour
{
    public AudioSource collisionSoundSource;  // L'AudioSource � utiliser pour jouer le son
    private bool hasPlayed;  // Variable pour v�rifier si le son a d�j� �t� jou�

    void Start()
    {
        // Assurez-vous que l'AudioSource est attach� � ce GameObject
        if (collisionSoundSource == null)
        {
            collisionSoundSource = gameObject.AddComponent<AudioSource>();
        }

        // D�sactiver le son au d�marrage
        collisionSoundSource.Stop();
        hasPlayed = false;
    }

    void OnTriggerEnter(Collider other)
    {
        // V�rifier si l'objet en collision est le joueur avec le nom "PlayerWalkModif"
        if (other.gameObject.name == "PlayerWalkModif" && !hasPlayed)
        {
            // Jouer le son
            collisionSoundSource.Play();
            hasPlayed = true;  // Marquer que le son a �t� jou�
        }
    }
}
