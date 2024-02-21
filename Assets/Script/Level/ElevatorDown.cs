using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorDown : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D player_attack;
    [SerializeField] private GameObject elevator;

    void OnTriggerEnter2D(Collider2D other){
        if(other == player_attack && !elevator.GetComponent<Elevator>().is_bottom){
                elevator.GetComponent<Elevator>()._toStateDowning();
        }
    }
}
