using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerPoint : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float xDistance;
    // Start is called before the first frame update
    void Start()
    {

    }
    void OnEnable() {
        Vector3 playerPosition = playerTransform.position;
        transform.position = playerPosition + new Vector3(-xDistance, 0, 0);
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 playerPosition = playerTransform.position;
        transform.position = playerPosition + new Vector3(-xDistance, 0, 0);
    }
}
