using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

[HelpURL("https://app.milanote.com/1Rsc3R1SJGPM9g/cameramovement?p=5Aw4gcZ0pqp")]
public class CameraMovements : MonoBehaviour
{
    [SerializeField] private float offsetY, offsetZ;
    private Slope Player;
    //[SerializeField] private Sphere Player;

    private void Awake()
    {
        Player = FindAnyObjectByType<Slope>();
    }
    private void Update()
    {
        transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y + offsetY, Player.transform.position.z - offsetZ);
    }
}