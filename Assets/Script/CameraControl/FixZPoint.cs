using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixZPoint : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform rotateObj;
    [SerializeField] private float xOffset;
    [SerializeField] private float yOffset;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = rotateObj.position + rotateObj.right * xOffset + new Vector3(0, yOffset, 0);
        float x = position.x;
        float y = position.y;
        transform.position = new Vector3(x, y, 0);
    }
}
