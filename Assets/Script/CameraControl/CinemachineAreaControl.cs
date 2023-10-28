using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineAreaControl : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        this.GetComponent<CinemachineVirtualCamera>().enabled = false;
    }

    // Update is called once per frame
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
