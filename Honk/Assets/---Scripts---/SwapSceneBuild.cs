using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SwapSceneBuild : MonoBehaviour
{
    public List<SceneToSwap> ListScene = new List<SceneToSwap>();
    private void Update()
    {
        foreach (SceneToSwap list in ListScene)
        {
            if (Input.GetKeyUp(list.KeyCodeScene))
            {
                SceneManager.LoadScene(list.SceneName);
            }
        }
    }
}

[System.Serializable]
public class SceneToSwap
{
    public string SceneName;
    public KeyCode KeyCodeScene;
}