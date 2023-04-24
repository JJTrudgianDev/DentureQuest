using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoSceneSwap : MonoBehaviour
{

    public float timeToChange;
    public string sceneName;

    private void Start()
    {
        Invoke("LoadMap", timeToChange);
    }

    void LoadMap()
    {
        SceneManager.LoadScene(sceneName);
    }

}
