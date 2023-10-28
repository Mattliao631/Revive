using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_one_way_platform : MonoBehaviour
{
    private GameObject currentOneWayPlatform;
    [SerializeField] private CapsuleCollider2D playerColider;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("s")){
            if(currentOneWayPlatform != null){
                StartCoroutine(DisableCollision());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("OneWayPlatform")){
            currentOneWayPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision){
        if(collision.gameObject.CompareTag("OneWayPlatform")){
            currentOneWayPlatform = null;
        }
    }

    private IEnumerator DisableCollision(){
        BoxCollider2D platfromCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(playerColider,platfromCollider);
        yield return new WaitForSeconds(0.25f);
        Physics2D.IgnoreCollision(playerColider,platfromCollider, false);
    
    }
}
