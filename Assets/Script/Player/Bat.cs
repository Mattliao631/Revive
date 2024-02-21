using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private SpriteRenderer sprite;
    private PolygonCollider2D pol_Col2D;

    [SerializeField] private int direction = 1;
    public float delta_x;
    [HideInInspector] public float lastGenerate;

    [SerializeField] private float remainTime;
    [SerializeField] private LayerMask nonPenetratableLayers;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        pol_Col2D = GetComponent<PolygonCollider2D>();
    }

    void Update() {
        if (GetComponent<Collider2D>().IsTouchingLayers(nonPenetratableLayers)) {
            gameObject.SetActive(false);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Fly();
        
    }

    public void Flip(){
        direction = player.GetComponent<PlayerController>().face_Dir;
        transform.localRotation = player.transform.rotation;
    }

    void Fly(){
        transform.position += new Vector3(delta_x * direction, 0, 0);
        if (Time.time >= lastGenerate + remainTime) gameObject.SetActive(false);
    }
    // delay for the right attack moment

    // is trigger will check by itself 
    
    // void OnTriggerEnter2D(Collider2D other){
        
    // }
}
