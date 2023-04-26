using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectUnlockManager : MonoBehaviour
{
    [System.Serializable]
    public struct KeyLockPair
    {
        public GameObject lockedObject;
        public string keyTag;
        public bool isUnlocked;
    }

    [SerializeField] private List<KeyLockPair> keyLockPairs;
    [SerializeField] private Transform player;

    private void Start()
    {
        foreach (KeyLockPair pair in keyLockPairs)
        {
            pair.lockedObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckForKeyAndUnlockObject();
        }
    }

    private void CheckForKeyAndUnlockObject()
    {
        Collider[] hitColliders = Physics.OverlapSphere(player.position, 1f);
        foreach (Collider hitCollider in hitColliders)
        {
            for (int i = 0; i < keyLockPairs.Count; i++)
            {
                if (hitCollider.CompareTag(keyLockPairs[i].keyTag) && !keyLockPairs[i].isUnlocked)
                {
                    UnlockObject(i);
                    hitCollider.gameObject.SetActive(false);
                    break;
                }
                else if (hitCollider.CompareTag(keyLockPairs[i].keyTag) && keyLockPairs[i].isUnlocked)
                {
                    DestroyLock(i);
                    break;
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
    }

    private void DestroyLock(int index)
    {
        Destroy(keyLockPairs[index].lockedObject);
    }
}
