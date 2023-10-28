using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy_Eagle : Enemy
{


    private Transform player_pos;
    private  Vector3 origin_pos;
    private Animator anime;

    public float detect_distance;
    public float attack_distance;
    public float bound_distance;
    public float move_speed;

    new void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
        player_pos = player.GetComponent<Transform>();
        origin_pos = transform.position;
        anime = GetComponent<Animator>();

    }

    new void Update()
    {
        base.Update();
        Detect_Circle();
    }

    private void Detect_Circle(){
        if(Vector3.Distance(player_pos.position,origin_pos) > bound_distance){
            back_to_origin();
        }else if(Vector3.Distance(transform.position,player_pos.position) < attack_distance){
            attack();
        }else if(Vector3.Distance(transform.position,player_pos.position) < detect_distance){
            chase();
        }else{
            idle();
        }
    }

    private void chase(){
        transform.position = Vector3.MoveTowards(transform.position,player_pos.position,move_speed * Time.deltaTime);
        anime.SetBool("chase",false);
    }

    private void attack(){
        transform.position = Vector3.MoveTowards(transform.position,player_pos.position,move_speed * 2 * Time.deltaTime);
        anime.SetBool("chase",true);
    }

    private void back_to_origin(){
        transform.position = Vector3.MoveTowards(transform.position,origin_pos,move_speed *2* Time.deltaTime);
        anime.SetBool("chase",false);
    }

    private void idle(){
        transform.position = Vector3.MoveTowards(transform.position,origin_pos,move_speed * Time.deltaTime);
        anime.SetBool("chase",false);
    }

}
