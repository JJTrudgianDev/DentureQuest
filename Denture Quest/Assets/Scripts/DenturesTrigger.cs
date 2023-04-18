using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DenturesTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Dentures"))
        {
            SceneManager.LoadScene("LevelComplete");
            Debug.Log("Dentures function executed!");
        }
    }
}
