using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class particle : MonoBehaviour
{
    [SerializeField] private GameObject[] particles;

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            for(int i=0;i<particles.Length;i++){
                particles[i].SetActive(true);
            }  
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")){
            for(int i=0;i<particles.Length;i++){
                particles[i].SetActive(false);
            }
        }
    }
}
