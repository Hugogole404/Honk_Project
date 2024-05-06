using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crie_Honk : MonoBehaviour
{
    public AudioSource AudioSourceSound;
    public AudioClip[] crie_honk;

    void Start()
    {
        // assigner les sons de pas dans l'éditeur Unity
        AudioSourceSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            // son de pas aleatoire
            int randomIndex = Random.Range(0, crie_honk.Length);
            AudioSourceSound.clip = crie_honk[randomIndex];

            // Jouez le son choisi
            AudioSourceSound.Play();
        }
    }
}
