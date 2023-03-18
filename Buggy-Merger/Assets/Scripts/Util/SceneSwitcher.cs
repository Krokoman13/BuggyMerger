using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService;
using UnityEngine;

using UnityEngine.SceneManagement;

class SceneSwitcher : MonoBehaviour
{
    static public void SwitchToSceneString(string scene)
    {
        Debug.Log("Loading scene: " + scene);

        SceneManager.LoadScene(scene);
    }

    static public void SwitchToSceneInt(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }


    static public void ExitGame()
    {
        Application.Quit();
    }

    static public void ReloadCurrentScene()
    {
        Debug.Log("Reloading scene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
