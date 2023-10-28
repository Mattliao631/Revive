using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    
    [Header("Player State")]
    public bool Accept_Input = true;
    public bool pressing_jump = false;
    public bool isWallSliding;
    public bool isWallJumping;
    public bool down_smashing;

    public float run_Speed;
    public float jump_hight;
    public float jump_time;

    private float move_Dir = 0.0f;
    public int face_Dir = 1;

    private int jump_keydown = 0;
    private bool can_jump;
    private bool can_double_jump;
    private float jump_timer = 0;
    private float jump_buffer_time = 0.1f;
    private float jump_buffer_counter;
    
    public GameObject bat_prefeb;
    private GameObject bat;
    public Transform shot_pos;

    private Transform trans;

    public float max_drop_velocity;

    
    private float wallSlidingSpeed = 2f;
    
    private float wallJumpingTime = 0.01f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.2f;
    private Vector2 wallJumpingPower = new Vector2(10f, 13f);

    private float last_s_timer;
    private float double_s_range = 0.2f;

    [HideInInspector] public Vector3 respawn_from_water_point;
    
    private bool isMaxFalling = false;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    [Header("Camera parameters")]
    [SerializeField] private CameraFollowObj cameraFollowObj;

    [SerializeField] private GameObject player_mana;

    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        trans = GetComponent<Transform>();
        Reset_Jump();
    }

    // Update is called once per frame
    void Update()
    {
        Flip();
        Checked_Grounded();
        // Fall_Reset();
        Fall_Velocity_Max();
        WallJump();
        WallSlide();
        Checked_Falling();
        Down_Smash();
        if(Accept_Input){         
            Jump();
            shoot_bat();
            Run(); 
        }
    }

    void FixedUpdate(){

    }

    void Run(){

        if(!isWallJumping){
            if(Input.GetKey("a")){
                move_Dir = -1.0f;
                face_Dir = -1;
            }else if(Input.GetKey("d")){
                move_Dir = 1.0f;
                face_Dir = 1;
            }else{move_Dir = 0.0f;}
        
            Vector2 playerVal = new Vector2(move_Dir * run_Speed, rb.velocity.y);
            rb.velocity = playerVal; 

            bool player_is_running = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
            animator.SetBool("run", player_is_running);    
        }
    }

    void Flip(){
        // if(isWallJumping){
        //     if(face_Dir == 1){transform.localRotation = Quaternion.Euler(0,180,0);}
        //     else{transform.localRotation = Quaternion.Euler(0,0,0);}
        // }
        // else {
        //     if(face_Dir == 1){transform.localRotation = Quaternion.Euler(0,0,0);}
        //     else{transform.localRotation = Quaternion.Euler(0,180,0);}
        // }

        if ((face_Dir == 1) ^ isWallJumping) {
            transform.localRotation = Quaternion.Euler(0,0,0);
        } else {
            transform.localRotation = Quaternion.Euler(0,180,0);
        }
        
        cameraFollowObj.callTurn(face_Dir);
    }

    void Jump(){
        if(!isWallSliding && !isWallJumping){
            if(IsGrounded() && jump_keydown != 0 && !pressing_jump){
                Reset_Jump();
            }else if((Input.GetKey("l") || jump_buffer_counter > 0) && (jump_timer <= jump_time) && (can_jump || can_double_jump) && !(OnWall()&&move_Dir !=0 && !IsGrounded())){
                Vector2 playerVal = new Vector2(rb.velocity.x, jump_hight);
                rb.velocity = playerVal; 
                jump_timer += Time.deltaTime;
                // animator.SetBool("jump",true);      
            }
        }else{animator.SetBool("jump",false);}

        if(Input.GetKeyDown("l")){
            jump_keydown += 1;         
            pressing_jump = true;
            jump_buffer_counter = jump_buffer_time;
            if(can_jump || can_double_jump){
               animator.SetBool("jump",true);  
            } 
        }else if(jump_buffer_counter > -1){
            jump_buffer_counter -= Time.deltaTime;
        }
        
        if(Input.GetKeyUp("l")){
            Release_Jump_Or_Hurt_Fix();
        }
    }
    
    public void Release_Jump_Or_Hurt_Fix() {

        pressing_jump = false;
        animator.SetBool("jump", false);
        
        if(jump_keydown == 1){
                can_jump = false;
                jump_timer = jump_time * (1 - 0.8f);
            }else if(jump_keydown >= 2){
                can_jump = false;
                can_double_jump = false;
                jump_timer =0;
            }
    }

    public void Reset_Jump_Buffer() {
        jump_buffer_counter = 0;
    }

    private void Reset_Jump(){
        jump_keydown= 0;
        jump_timer= 0;
        can_double_jump = true;
        can_jump = true;
    }
    
    public void Set_Extra_Jump(){
            jump_keydown = 1;
            can_double_jump = true;
            jump_timer =jump_time * (1 - 0.8f);   
    }

    void Checked_Grounded(){
        bool isGrounded = IsGrounded();
        animator.SetBool("is_ground", isGrounded);
        if (isGrounded) {
            respawn_from_water_point = transform.position;
        }
        
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.05f, groundLayer);
    }
    
    void Checked_Falling(){
        if(rb.velocity.y <= -1.0f){
            animator.SetBool("falling", true);
        }else{
            animator.SetBool("falling", false);
        }
    }

    private bool OnWall()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.05f, wallLayer);
    }

    private void WallSlide(){
        if (OnWall() && !IsGrounded() && move_Dir != 0 && !isWallJumping){
            isWallSliding = true;
            animator.SetBool("is_sliding",true);
            rb.velocity = new Vector3(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue),0.0f);
            if(!pressing_jump){Reset_Jump();}
        }else{
            isWallSliding = false;
            animator.SetBool("is_sliding",false);
        }
    }

    private void WallJump(){
        if (isWallSliding){
            isWallJumping = false;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }else{wallJumpingCounter -= Time.deltaTime;}

        if (Input.GetKeyDown("l") && wallJumpingCounter > 0f){   
            isWallJumping = true;
            animator.SetBool("is_sliding",false);
            animator.SetBool("jump",true);
            rb.velocity = new Vector2(face_Dir * -1 * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;
            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }
    
    void shoot_bat(){
        if(Input.GetKeyDown("k")){
            bat = GameObject.FindGameObjectWithTag("Bat");

            if(bat == null){
                Instantiate(bat_prefeb,new Vector3(shot_pos.position.x,shot_pos.position.y,0),Quaternion.identity);
            }else{
                transform.position = bat.transform.position;
                bat.GetComponent<Bat>().destroy();
            }            
        }
    }

    void Fall_Reset(){
        if(trans.position.y <= -20){
            trans.position = new Vector3(trans.position.x,5,trans.position.z);
        }
    }

    void Fall_Velocity_Max(){
        if(rb.velocity.y <= max_drop_velocity && !down_smashing){
            Vector2 playerVal = new Vector2(rb.velocity.x, max_drop_velocity);
            rb.velocity = playerVal;
            isMaxFalling = true;
        } else {
            isMaxFalling = false;
        }
        
    }

    void Down_Smash(){
        if (Input.GetKeyDown("s") && (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") || animator.GetCurrentAnimatorStateInfo(0).IsName("Fall")))
        {
            float since_last_s = Time.time - last_s_timer;
            if(since_last_s <= double_s_range && player_mana.GetComponent<Player_Mana>().cost_mana(1)){
                // do the skill
                Debug.Log("down_smashing");
                down_smashing = true;
                Accept_Input = false;

                max_drop_velocity = max_drop_velocity * 2;
                Vector2 playerVal = new Vector2(rb.velocity.x, max_drop_velocity);
                rb.velocity = playerVal;
                max_drop_velocity = max_drop_velocity/2;
            }
            last_s_timer = Time.time;
        }else if(IsGrounded()){
            down_smashing = false;
            Accept_Input = true;
        }
    }

    public void Respawn_From_Water() {
        transform.position = respawn_from_water_point;
    }
}
