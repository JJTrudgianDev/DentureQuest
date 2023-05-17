using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoSceneSwap : MonoBehaviour
{

    public float timeToChange;
    public string sceneName;
    public string mainMenu;

    private void Start()
    {
        //Invoke("LoadMap", timeToChange);
    }

    public void LoadMap()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(mainMenu);
    }
}
