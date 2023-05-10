using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectUnlockManager : MonoBehaviour
{
    [System.Serializable]
    public struct KeyLockPair
    {
        public GameObject lockedObject;
        public GameObject keyObject;
        public string keyTag;
        public bool isUnlocked;
        public Image unlockImage;
    }

    [SerializeField] private List<KeyLockPair> keyLockPairs;
    [SerializeField] private Transform player;

    private void Start()
    {
        foreach (KeyLockPair pair in keyLockPairs)
        {
            if (pair.lockedObject != null)
                pair.lockedObject.SetActive(true);

            if (pair.unlockImage != null)
                pair.unlockImage.enabled = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            CheckForKeyAndUnlockObject();
        }
    }

    private void CheckForKeyAndUnlockObject()
    {
        Collider[] hitColliders = new Collider[1]; // Adjust the size according to your needs
        int numColliders = Physics.OverlapSphereNonAlloc(player.position, 5f, hitColliders);

        for (int i = 0; i < numColliders; i++)
        {
            Collider hitCollider = hitColliders[i];

            SphereCollider sphereCollider = hitCollider.GetComponent<SphereCollider>();
            if (sphereCollider != null)
            {
                for (int j = 0; j < keyLockPairs.Count; j++)
                {
                    if (hitCollider.CompareTag(keyLockPairs[j].keyTag) && !keyLockPairs[j].isUnlocked)
                    {
                        UnlockObject(j);
                        hitCollider.gameObject.SetActive(false);
                        break;
                    }
                    else if (hitCollider.CompareTag(keyLockPairs[j].keyTag) && keyLockPairs[j].isUnlocked)
                    {
                        DestroyLock(j);
                        break;
                    }
                }
            }
        }
    }

    private void UnlockObject(int index)
    {
        KeyLockPair updatedPair = keyLockPairs[index];
        updatedPair.isUnlocked = true;
        keyLockPairs[index] = updatedPair;

        // Set the locked object's tag to the key's tag
        keyLockPairs[index].lockedObject.tag = keyLockPairs[index].keyTag;

        // Enable the unlock image in the UI
        keyLockPairs[index].unlockImage.enabled = true;
    }

    private void DestroyLock(int index)
    {
        Destroy(keyLockPairs[index].lockedObject);
    }
}
