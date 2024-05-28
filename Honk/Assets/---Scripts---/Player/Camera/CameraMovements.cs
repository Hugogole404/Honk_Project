using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

[HelpURL("https://app.milanote.com/1Rsc3R1SJGPM9g/cameramovement?p=5Aw4gcZ0pqp")]
public class CameraMovements : MonoBehaviour
{
    [SerializeField] private float offsetY, offsetZ, offsetX;
    private Slope Player;
    private PlayerMovements PlayerMovWalk;
    //[SerializeField] private Sphere Player;

    private void Awake()
    {
        Player = FindAnyObjectByType<Slope>();
        if (Player == null)
        {
            PlayerMovWalk = FindAnyObjectByType<PlayerMovements>();
        }
    }
    private void Update()
    {
        if (Player == null)
        {
            transform.position = new Vector3(PlayerMovWalk.transform.position.x + offsetX, PlayerMovWalk.transform.position.y + offsetY, PlayerMovWalk.transform.position.z - offsetZ);
        }
        else
        {
            transform.position = new Vector3(Player.transform.position.x + offsetX, Player.transform.position.y + offsetY, Player.transform.position.z - offsetZ);
        }
    }
}