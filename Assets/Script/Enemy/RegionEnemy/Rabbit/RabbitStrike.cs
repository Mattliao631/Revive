using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitStrike : MonoBehaviour
{
    [SerializeField] private EnemyRabbit rabbit;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            Player_Hurt hurt = other.gameObject.GetComponent<Player_Hurt>();
            if (hurt != null) {
                hurt.Touch_Enemy(rabbit.damage);
            }
        }
    }
}
