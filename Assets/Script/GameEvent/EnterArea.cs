using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterArea : MonoBehaviour
{
    [SerializeField] private int areaId;
    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine(WaitForCollider());
    }
    // IEnumerator WaitForCollider() {
    //     yield return new WaitForSeconds(5f);
    //     GetComponent<PolygonCollider2D>().enabled = true;
    // }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            AreaNameManager.instance.Display(areaId);
        }
    }
}
