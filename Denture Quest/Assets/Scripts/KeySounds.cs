using System.Collections;
using UnityEngine;

public class KeySounds : MonoBehaviour
{
    public AudioClip audioClip;
    public float minTime = 5f;
    public float maxTime = 10f;
    public float minDistance = 1f;
    public float maxDistance = 10f;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 1f; // Set audio source to 3D audio
        StartCoroutine(PlayAudioWithRandomDelay());
    }

    private IEnumerator PlayAudioWithRandomDelay()
    {
        while (true)
        {
            float randomTime = Random.Range(minTime, maxTime);
            Debug.Log("Waiting for " + randomTime + " seconds");
            yield return new WaitForSeconds(randomTime);

            if (audioClip != null)
            {
                audioSource.clip = audioClip;
                audioSource.Play();
                Debug.Log("Playing audio");
            }
        }
    }
}
