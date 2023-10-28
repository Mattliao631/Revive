using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearJumpDetection : MonoBehaviour
{
    [SerializeField] private EnemyBear bear;
    

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            bear.findJump = true;
        }
    }
    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            bear.findJump = false;
        }
    }
}
