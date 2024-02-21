using System.Collections;
using UnityEngine;

public class DownSmashBlock : MonoBehaviour
{
    [SerializeField] private float fallDelay = 0.1f;
    [SerializeField] private float destroyDelay = 0.1f;
 
    private bool falling = false;

    [SerializeField] private GameObject player;
 
    [SerializeField] private Rigidbody2D rb;
 
    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     // Avoid calling the coroutine multiple times if it's already been called (falling)
    //     if (falling)
    //         return; 
 
    //     // If the player landed on the platform, start falling
    //     if (collision.transform.tag == "Player")
    //     {
    //         if(player.GetComponent<PlayerController>().down_smashing){
    //             StartCoroutine(StartFall());
    //         }
            
    //     }
    // }
    private void OnTriggerEnter2D(Collider2D other) {
        if (falling)
            return;
        if (other.CompareTag("PlayerSkill")) {
            StartCoroutine(StartFall());
        }
    }
 
    private IEnumerator StartFall()
    {
        falling = true; 
 
        // Wait for a few seconds before dropping
        yield return new WaitForSeconds(fallDelay);
 
        // Enable rigidbody and destroy after a few seconds
        rb.bodyType = RigidbodyType2D.Dynamic;
        Destroy(gameObject, destroyDelay);
    }
}
