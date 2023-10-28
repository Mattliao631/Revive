using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    private GameObject player;
    private SpriteRenderer sprite;
    private PolygonCollider2D pol_Col2D;

    private int direction;
    public float delta_x;
    public float remain_time;
    private float timer;

    public float collider_start_time;
    public float collider_remain_time;
    private int damage;

    // Start is called before the first frame update
    void Start()
    {
        damage = 1;
        direction = 1;
        sprite = GetComponent<SpriteRenderer>();
        pol_Col2D = GetComponent<PolygonCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        Flip();
        StartCoroutine(Start_Attack());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Fly();
    }

    void Flip(){
        direction = player.GetComponent<PlayerController>().face_Dir;
        if(direction == -1){sprite.flipX = true;}
    }

    void Fly(){
        transform.position = new Vector3(transform.position.x + delta_x * direction,transform.position.y,transform.position.z);
        timer += Time.deltaTime;
        if(timer >= remain_time){destroy();}
    }

    public void destroy(){
        Destroy(gameObject);
    }

    // delay for the right attack moment
    IEnumerator Start_Attack(){
        yield return new WaitForSeconds(collider_start_time);
        pol_Col2D.enabled = true;
        StartCoroutine(Disable_Hitbox());
    }

    IEnumerator Disable_Hitbox(){
        yield return new WaitForSeconds(collider_remain_time);
        pol_Col2D.enabled = false;
    }

    // is trigger will check by itself 
    
    // void OnTriggerEnter2D(Collider2D other){
        
    // }
}
