using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{


    [ContextMenu("Remove All Colliders")]
    void RemoveAllColliders()
    {
        // Get all collider components attached to the GameObject and its children
        Collider[] colliders = GetComponentsInChildren<Collider>();

        // Loop through each collider and destroy it
        foreach (Collider collider in colliders)
        {
            DestroyImmediate(collider);
        }

        Debug.Log("All colliders removed from the GameObject and its children.");
    }
}

