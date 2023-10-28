using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearRushDetection : MonoBehaviour
{
    [SerializeField] private EnemyBear bear;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            bear.findRush = true;
        }
    }
    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            bear.findRush = false;
        }
    }
    //
}
