using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpingPad : MonoBehaviour
{
    [SerializeField] private float bounce_force = 20f;
    [SerializeField] private float enemyBounceFactor = 0.6f;
    private void OnCollisionEnter2D(Collision2D collision){
        
        if(collision.gameObject.CompareTag("Player")){
            Vector2 player_velocity = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(player_velocity.x, bounce_force);
            collision.gameObject.GetComponent<PlayerController>().ResetDoubleJump();
        } else {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null) {
                Rigidbody2D rb2d = enemy.gameObject.GetComponent<Rigidbody2D>();
                Vector3 velocity = rb2d.velocity;
                rb2d.velocity = new Vector3(velocity.x, bounce_force * enemyBounceFactor);
            }
        }
    }
}
