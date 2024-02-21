using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDelete : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            GetComponent<StateConditioner>().StateConditionCall();
            gameObject.SetActive(false);
        }
    }
}
