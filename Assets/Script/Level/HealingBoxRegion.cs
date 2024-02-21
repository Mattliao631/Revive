using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealingBoxRegion : MonoBehaviour
{
    [SerializeField] private GameObject healingbox;

    void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("Player")){
            Respawn_HealingBox();
        }
    }

    private void Respawn_HealingBox(){
        healingbox.SetActive(true);
    }

}
