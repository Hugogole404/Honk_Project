using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paw_Desactive : MonoBehaviour
{
    public GameObject paw_grotte; // Le premier GameObject avec les sons de pas de la grotte
    public GameObject paw_grotte_grow; // Le deuxi�me GameObject avec les sons de pas grow

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PlayerWalkModif")
        {
            // R�active le premier GameObject
            if (paw_grotte != null)
            {
                paw_grotte.SetActive(true);
            }

            // D�sactive le deuxi�me GameObject
            if (paw_grotte_grow != null)
            {
                paw_grotte_grow.SetActive(false);
            }
        }
    }
}
