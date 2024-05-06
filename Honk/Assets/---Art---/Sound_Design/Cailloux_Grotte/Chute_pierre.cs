using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chute_pierre : MonoBehaviour
{
    public AudioSource AudioSourceSound;
    public AudioClip[] chute_cailloux;

    public float delaiInitial = 5f; // Temps initial avant la première chute
    public float intervalleEntreChutes = 5f; // Temps entre chaque chute

    void Start()
    {
        // Assurez-vous d'assigner votre AudioSource dans l'éditeur Unity
        AudioSourceSound = GetComponent<AudioSource>();

        // Démarrez la chute de cailloux après un délai initial
        Invoke("ChuteDeCailloux", delaiInitial);
    }

    void ChuteDeCailloux()
    {
        // Appelé à chaque chute de cailloux
        ChuterUnCaillou();

        // Répétez la chute de cailloux avec un délai entre chaque répétition
        InvokeRepeating("ChuterUnCaillou", intervalleEntreChutes, intervalleEntreChutes);
    }

    void ChuterUnCaillou()
    {
        // Son de chute de cailloux aléatoire
        int randomIndex = Random.Range(0, chute_cailloux.Length);
        AudioSourceSound.clip = chute_cailloux[randomIndex];

        // Jouez le son choisi
        AudioSourceSound.Play();
    }
}