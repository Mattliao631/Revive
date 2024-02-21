using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMover : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.2f;
    [SerializeField] private float scaleSpeed = 0.2f;
    private float size = 79f;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W)) {
            transform.position += new Vector3(0, Time.deltaTime * moveSpeed, 0);
        }
        if (Input.GetKey(KeyCode.A)) {
            transform.position += new Vector3(-Time.deltaTime * moveSpeed, 0, 0);
        }
        if (Input.GetKey(KeyCode.S)) {
            transform.position += new Vector3(0, -Time.deltaTime * moveSpeed, 0);
        }
        if (Input.GetKey(KeyCode.D)) {
            transform.position += new Vector3(Time.deltaTime * moveSpeed, 0, 0);
            
        }

        if (Input.GetKey(KeyCode.Q)) {
            size += Time.deltaTime * scaleSpeed;
        } else if (Input.GetKey(KeyCode.E)) {
            size -= Time.deltaTime * scaleSpeed;
            if (size < 0f) {
                size = 0f;
            }
        }
        GetComponent<Camera>().fieldOfView = size;
    }
}
