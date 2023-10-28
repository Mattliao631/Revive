using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionMonsterSense : MonoBehaviour
{
    [SerializeField] private RegionEnemy monster;


    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            monster.seePlayer = true;
        }
    }
    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            monster.seePlayer = false;
        }
    }
}
