using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public Dictionary<string, RawImage> keyUIElements = new Dictionary<string, RawImage>(); // Dictionary of lock tags and corresponding UI elements

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddKey(string lockTag, RawImage keyUIImage)
    {
        if (HasKey(lockTag))
        {
            return;
        }

        keyUIElements.Add(lockTag, keyUIImage);
        keyUIImage.enabled = true;
    }

    public void RemoveKey(string lockTag)
    {
        if (!HasKey(lockTag))
        {
            return;
        }

        RawImage keyUIImage = keyUIElements[lockTag];
        keyUIElements.Remove(lockTag);
        keyUIImage.enabled = false;
    }

    public bool HasKey(string lockTag)
    {
        return keyUIElements.ContainsKey(lockTag);
    }
}
