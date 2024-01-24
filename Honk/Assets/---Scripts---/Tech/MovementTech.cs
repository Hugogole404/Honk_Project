using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTech : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        // Obtenez les entr�es du clavier
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculez la direction de d�placement
        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);
        movement.Normalize(); // Pour s'assurer que la vitesse de d�placement reste constante dans toutes les directions

        // Appliquez le d�placement � la position du joueur
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }
}
