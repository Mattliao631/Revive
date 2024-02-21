using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineAreaControl : MonoBehaviour
{
    void Start()
    {
        this.GetComponent<CinemachineVirtualCamera>().enabled = false;
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            this.GetComponent<CinemachineVirtualCamera>().enabled = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Player") { 
            this.GetComponent<CinemachineVirtualCamera>().enabled = false;
        }
    }
}
