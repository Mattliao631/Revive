using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpingPad : MonoBehaviour
{
    private float bounce_force = 20f;

    private void OnCollisionEnter2D(Collision2D collision){
        
        if(collision.gameObject.CompareTag("Player")){
            Vector2 player_velocity = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(player_velocity.x, bounce_force);
            collision.gameObject.GetComponent<PlayerController>().Set_Extra_Jump();
        }else if(collision.gameObject.CompareTag("Enemy")){
            Vector2 enemy_velocity = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(enemy_velocity.x, bounce_force * 0.6f);
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            
        }
    }
}
