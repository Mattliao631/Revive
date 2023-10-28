using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitStrikeSense : MonoBehaviour
{
    [SerializeField] private EnemyRabbit rabbit;
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            rabbit.findStrike = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            rabbit.findStrike = false;
        }
    }
}
