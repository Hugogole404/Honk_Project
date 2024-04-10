using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SwapSceneBuild : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SceneManager.LoadScene("Forest");
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene("Cave");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            SceneManager.LoadScene("PartieGlissade");
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            SceneManager.LoadScene("PartieMarche");
        }
    }
}