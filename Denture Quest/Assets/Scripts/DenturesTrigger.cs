using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DenturesTrigger : MonoBehaviour
{

    public int nextLevelToUnlock;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Dentures"))
        {
            PlayerPrefs.SetInt("LevelUnlocked", nextLevelToUnlock);

            SceneManager.LoadScene("LevelComplete");
            Debug.Log("Dentures function executed!");
        }
    }
}
