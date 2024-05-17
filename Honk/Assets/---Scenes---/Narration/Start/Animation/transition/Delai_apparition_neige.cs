using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delai_apparition_neige : MonoBehaviour
{

    public ParticleSystem targetParticleSystem; // Le Particle System à faire apparaître
    public float delay = 5.0f; // Délai en secondes avant l'apparition

    void Start()
    {
        if (targetParticleSystem != null)
        {
            // Désactiver le GameObject contenant le Particle System initialement
            targetParticleSystem.gameObject.SetActive(false);
            // Commencer la coroutine pour le délai
            StartCoroutine(AppearAfterDelay());
        }
    }

    IEnumerator AppearAfterDelay()
    {
        // Attendre le délai spécifié
        yield return new WaitForSeconds(delay);
        // Activer le GameObject contenant le Particle System
        targetParticleSystem.gameObject.SetActive(true);
        // Jouer le Particle System
        targetParticleSystem.Play();
    }
}
