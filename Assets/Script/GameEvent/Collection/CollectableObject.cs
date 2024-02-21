using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObject : MonoBehaviour
{
    protected PlayerStateList stateList;

    protected void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            TouchedPlayer();
            gameObject.SetActive(false);
        }
    }
    protected void Start() {
        stateList = GameManager.instance.stateList;
    }
    protected virtual void TouchedPlayer() {}
}
