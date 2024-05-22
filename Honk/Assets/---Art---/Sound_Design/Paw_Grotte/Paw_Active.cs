using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paw_Active : MonoBehaviour
{
    public GameObject paw_grotte; // Le premier GameObject avec les sons de pas de la grotte
    public GameObject paw_grotte_grow; // Le deuxième GameObject avec les sons de pas grow

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PlayerWalkModif")
        {
            // Désactive le premier GameObject
            if (paw_grotte != null)
            {
                paw_grotte.SetActive(false);
            }

            // Active le deuxième GameObject
            if (paw_grotte_grow != null)
            {
                paw_grotte_grow.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "PlayerWalkModif")
        {
            // Réactive le premier GameObject
            if (paw_grotte != null)
            {
                paw_grotte.SetActive(true);
            }

            // Désactive le deuxième GameObject
            if (paw_grotte_grow != null)
            {
                paw_grotte_grow.SetActive(false);
            }
        }
    }
}
