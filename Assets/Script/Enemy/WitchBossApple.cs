using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchBossApple : MonoBehaviour
{
    
    [Header("Size Control")]
    [SerializeField] private float existTime = 10f;
    [SerializeField] private float startScale = 1f;
    [SerializeField] private float endScale = 3f;
    
    // [Header("Reset by caller")]
    [HideInInspector] public float elapsedTime = 0f;

    void Awake()
    {
        startScale = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (elapsedTime > existTime) {
            // Generate effect
            transform.localScale = new Vector3(startScale, startScale, 1);
            gameObject.SetActive(false);
        }

        elapsedTime += Time.deltaTime;

        float scale = Mathf.Lerp(startScale, endScale, (elapsedTime / existTime));
        
        transform.localScale = new Vector3(scale, scale, 1);
        

    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            Debug.Log("Hit player!");
        }
    }
}
