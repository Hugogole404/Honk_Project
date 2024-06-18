using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Declencheur_sound : MonoBehaviour
{
    public AudioSource collisionSoundSource;  // L'AudioSource à utiliser pour jouer le son
    private bool hasPlayed;  // Variable pour vérifier si le son a déjà été joué

    void Start()
    {
        // Assurez-vous que l'AudioSource est attaché à ce GameObject
        if (collisionSoundSource == null)
        {
            collisionSoundSource = gameObject.AddComponent<AudioSource>();
        }

        // Désactiver le son au démarrage
        collisionSoundSource.Stop();
        hasPlayed = false;
    }

    void OnTriggerEnter(Collider other)
    {
        // Vérifier si l'objet en collision est le joueur avec le nom "PlayerWalkModif"
        if (other.gameObject.name == "PlayerWalkModif" && !hasPlayed)
        {
            // Jouer le son
            collisionSoundSource.Play();
            hasPlayed = true;  // Marquer que le son a été joué
        }
    }
}
