using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[HelpURL("https://app.milanote.com/1RCpEj14cuel2K/spawn-point-area?p=5Aw4gcZ0pqp")]
public class AreaSpawnPoint : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerMovements>() != null)
        {
            other.GetComponent<PlayerMovements>().SpawnPoint = _spawnPoint;
        }
    }
}