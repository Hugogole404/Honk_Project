using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delai_apparition_neige : MonoBehaviour
{

    public ParticleSystem targetParticleSystem; // Le Particle System � faire appara�tre
    public float delay = 5.0f; // D�lai en secondes avant l'apparition

    void Start()
    {
        if (targetParticleSystem != null)
        {
            // D�sactiver le GameObject contenant le Particle System initialement
            targetParticleSystem.gameObject.SetActive(false);
            // Commencer la coroutine pour le d�lai
            StartCoroutine(AppearAfterDelay());
        }
    }

    IEnumerator AppearAfterDelay()
    {
        // Attendre le d�lai sp�cifi�
        yield return new WaitForSeconds(delay);
        // Activer le GameObject contenant le Particle System
        targetParticleSystem.gameObject.SetActive(true);
        // Jouer le Particle System
        targetParticleSystem.Play();
    }
}
