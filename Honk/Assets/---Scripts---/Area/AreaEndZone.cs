using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaEndZone : MonoBehaviour
{
    public string NameSceneToLoad;
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerMovements>() != null)
        {
            SceneManager.LoadScene(NameSceneToLoad);
        }
        if(other.GetComponent<Slope>() != null)
        {
            SceneManager.LoadScene(NameSceneToLoad);
        }
    }
}