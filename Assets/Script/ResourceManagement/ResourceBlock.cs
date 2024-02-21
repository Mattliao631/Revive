using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceBlock : MonoBehaviour
{
    [SerializeField] private GameObject[] resources;

    void Start() {
        DeactivateResources();
    }
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            ActivateResources();
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            DeactivateResources();
        }
    }

    void ActivateResources() {
        for (int i = 0; i < resources.Length; i++) {
            resources[i].SetActive(true);
        }
    }
    void DeactivateResources() {
        for (int i = 0; i < resources.Length; i++) {
            resources[i].SetActive(false);
        }
    }
}
