using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    private PlayerStateList stateList;

    void Awake() {
        
    }
    void Start()
    {
        stateList = GameManager.instance.stateList;
        
        Vector3 position = stateList.GetPosition();
        transform.position = position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
