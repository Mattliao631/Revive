using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    private enum STATES{
        Uping = 0,
        Downing = 1,
        Stop = 2
    }

    [Header("State")]
    [SerializeField] private STATES _state = STATES.Stop;
    [HideInInspector] public bool is_top = false;
    [HideInInspector] public bool is_bottom = false;

    [Header("Movement")]
    public float speed;
    private Rigidbody2D rb;
    [SerializeField] private CapsuleCollider2D roof;
    [SerializeField] private BoxCollider2D ground;

    [Header("Wheels")]
    public float spin_speed;
    [SerializeField] private GameObject[] other_wheels_left;
    [SerializeField] private GameObject[] other_wheels_right;
    [SerializeField] private GameObject wheel_left;
    [SerializeField] private GameObject wheel_right;

    [Header("Particles")]
    [SerializeField] private GameObject right_fire;
    [SerializeField] private GameObject right_fire2;
    [SerializeField] private GameObject left_fire;
    [SerializeField] private GameObject left_fire2;
    [SerializeField] private GameObject up_fire;
    [SerializeField] private GameObject up_fire2;
    [SerializeField] private GameObject down_fire;
    [SerializeField] private GameObject down_fire2;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate(){
        _act();
    }

    public void _toStateUping() {
        _state = STATES.Uping;
        is_bottom = false;
    }
    public void _toStateStop() {
        _state = STATES.Stop;
    }
    public void _toStateDowning() {
        _state = STATES.Downing;
        is_top = false;
    }

    private void _act(){
        switch(_state){
            case STATES.Uping:
                _up();
                particles_controls(1);
                break;
            case STATES.Downing:
                _down();
                particles_controls(2);
                break;
            case STATES.Stop:
                _stop();
                particles_controls(0);
                break;
            default:
                Debug.Log("Still need to be fix0");
                break;
        }
    }

    private void _up(){
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = new Vector2(0,speed);

        wheels_up();
    }
    private void _down(){
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = new Vector2(0,-1 * speed);
        wheels_down();
    }    

    private void _stop(){
        rb.velocity = new Vector2(0,0);
        rb.gravityScale = 0;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other == roof){
            _toStateStop();
            is_top = true;
        }else if(other == ground){
            _toStateStop();
            is_bottom = true;
        }
    }

    private void wheels_up(){
        wheel_left.GetComponent<Transform>().Rotate(Vector3.forward * spin_speed);
        wheel_right.GetComponent<Transform>().Rotate(Vector3.back * spin_speed);

        for(int i = 0;i<other_wheels_left.Length;i++){
            other_wheels_left[i].GetComponent<Transform>().Rotate(Vector3.back * spin_speed);
        }

        for(int i = 0;i<other_wheels_right.Length;i++){
            other_wheels_right[i].GetComponent<Transform>().Rotate(Vector3.forward * spin_speed);
        }
    }

    private void wheels_down(){
        wheel_left.GetComponent<Transform>().Rotate(Vector3.back * spin_speed);
        wheel_right.GetComponent<Transform>().Rotate(Vector3.forward * spin_speed);

        for(int i = 0;i<other_wheels_left.Length;i++){
            other_wheels_left[i].GetComponent<Transform>().Rotate(Vector3.forward * spin_speed);
        }

        for(int i = 0;i<other_wheels_right.Length;i++){
            other_wheels_right[i].GetComponent<Transform>().Rotate(Vector3.back * spin_speed);
        }
    }

    private void particles_controls(int status){
        bool up_bool = false;
        bool down_bool = false;

        switch(status){
            case 1:
                up_bool = true;
                break;
            case 2:
                down_bool = true;
                break;
            default:
                break;
        }

        right_fire.SetActive(up_bool);
        right_fire2.SetActive(up_bool);
        up_fire.SetActive(up_bool);
        up_fire2.SetActive(up_bool);

        left_fire.SetActive(down_bool);
        left_fire2.SetActive(down_bool);
        down_fire.SetActive(down_bool);
        down_fire2.SetActive(down_bool);
    }

}
