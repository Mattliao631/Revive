using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaBoxRegion : MonoBehaviour
{
    [SerializeField] private GameObject manabox;

    void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("Player")){
            Respawn_ManaBox();
        }
    }

    private void Respawn_ManaBox(){
        manabox.SetActive(true);
    }
}
