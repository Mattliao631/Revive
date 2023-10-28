using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleTest : MonoBehaviour
{
    [Header("Apple Skill")]
    [SerializeField] GameObject apple;
    [SerializeField] Vector3 appleOffset;
    [SerializeField] float forceScale = 10f;


    private void castApple() {

        apple.SetActive(true);
        apple.GetComponent<WitchBossApple>().elapsedTime = 0f;

        Vector3 offset = appleOffset;

        if (this.GetComponent<PlayerController>().face_Dir == -1) {
            offset = -appleOffset;
        }
        
        apple.transform.position = transform.position + offset;

        apple.GetComponent<Rigidbody2D>().velocity = new Vector3(0f, 0f, 0f);
        apple.GetComponent<Rigidbody2D>().AddForce(transform.right * forceScale);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("g")) {
            castApple();
        }
    }
}
