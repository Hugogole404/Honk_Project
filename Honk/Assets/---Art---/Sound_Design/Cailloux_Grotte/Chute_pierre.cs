using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chute_pierre : MonoBehaviour
{
    public AudioSource AudioSourceSound;
    public AudioClip[] chute_cailloux;

    public float delaiInitial = 5f; // Temps initial avant la premi�re chute
    public float intervalleEntreChutes = 5f; // Temps entre chaque chute

    void Start()
    {
        // Assurez-vous d'assigner votre AudioSource dans l'�diteur Unity
        AudioSourceSound = GetComponent<AudioSource>();

        // D�marrez la chute de cailloux apr�s un d�lai initial
        Invoke("ChuteDeCailloux", delaiInitial);
    }

    void ChuteDeCailloux()
    {
        // Appel� � chaque chute de cailloux
        ChuterUnCaillou();

        // R�p�tez la chute de cailloux avec un d�lai entre chaque r�p�tition
        InvokeRepeating("ChuterUnCaillou", intervalleEntreChutes, intervalleEntreChutes);
    }

    void ChuterUnCaillou()
    {
        // Son de chute de cailloux al�atoire
        int randomIndex = Random.Range(0, chute_cailloux.Length);
        AudioSourceSound.clip = chute_cailloux[randomIndex];

        // Jouez le son choisi
        AudioSourceSound.Play();
    }
}