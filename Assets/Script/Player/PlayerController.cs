using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    private PlayerStateList stateList;
    private Rigidbody2D rb;
    private Animator animator;
    
    [Header("Player State")]
    public bool acceptInput = true;
    public bool pressing_jump = false;
    public bool isWallSliding;
    public bool isWallJumping;
    public bool down_smashing;
    // public bool canSave = true;

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
    
    [Header("Blood transmit")]
    [SerializeField] private GameObject bat;
    public Transform shot_pos;

    private Transform trans;

    [Header("Jump and Fall")]
    public float max_drop_velocity;

    
    private float wallSlidingSpeed = 2f;
    
    private float wallJumpingTime = 0.01f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.2f;
    private Vector2 wallJumpingPower = new Vector2(10f, 13f);

    private float last_s_timer;
    private float double_s_range = 0.2f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask waterLayer;

    [SerializeField] private LayerMask jumppadLayer;

    [Header("Water interaction")]
    [HideInInspector] public Vector3 respawn_from_water_point;
    [SerializeField] private Transform frontGroundDetectionPoint;
    [SerializeField] private Transform backGroundDetectionPoint;

    [Header("Camera parameters")]
    [SerializeField] private CameraFollowObj cameraFollowObj;

    [Header("Mana")]
    [SerializeField] private Player_Mana player_mana;

    [Header("Jump effects")]
    [SerializeField] private GameObject jump_effect;
    private Animator jump_effect_animator;
    [SerializeField] Transform jumpEffectTransform;

    [SerializeField] private GameObject jump_wings;

    [Header("Down smash effects")]
    [SerializeField] GameObject Sword_Effect;
    [SerializeField] GameObject Down_Smash_Effect;
    [SerializeField] GameObject Down_Smash_HitBox;
    public bool can_down_smash = true;
    private Animator down_smash_animator;
    [SerializeField] private GameObject player_health;
    public float down_smash_delay;

    [Header("Sound Effect")]
    [SerializeField] private AudioSource[] sounds;
    public bool lastGrounded;

    [SerializeField] private float groundSaveInterval = 5f;
    private float lastGroundSave;
    void Awake() {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        stateList = GameManager.instance.stateList;
        
        Vector3 position = stateList.GetPosition();
        transform.position = position;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        trans = GetComponent<Transform>();
        jump_effect_animator = jump_effect.GetComponent<Animator>();
        down_smash_animator = Down_Smash_HitBox.GetComponent<Animator>();
        Reset_Jump();
        respawn_from_water_point = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Check_Water();
        
        Checked_Grounded();
        // Fall_Reset();
        Fall_Velocity_Max();
        WallJump();
        WallSlide();
        Checked_Falling();
        Down_Smash();
        if (!acceptInput) {
            return;
        }
        Flip();
        Jump();
        shoot_bat();
        Run();
        
        
    }
    public void SetAcceptInput(bool value) {
        sounds[0].enabled = value;
        rb.velocity = new Vector2(0,0);
        acceptInput = value;
    }
    public bool GetAcceptInput() {
        return acceptInput;
    }
    private void Check_Water() {
        if (IsInWater()) {
            player_health.GetComponent<Player_Hurt>().Touch_Water();
        }
    }
    void FixedUpdate(){

    }

    void Run(){
        if(!isWallJumping){
            if(Input.GetKey("a") && !Input.GetKey("d")){
                move_Dir = -1.0f;
                face_Dir = -1;
            }else if(Input.GetKey("d") && !Input.GetKey("a")){
                move_Dir = 1.0f;
                face_Dir = 1;
            }else{move_Dir = 0.0f;}
        
            Vector2 playerVal = new Vector2(move_Dir * run_Speed, rb.velocity.y);
            rb.velocity = playerVal;

            bool player_is_running = (Mathf.Abs(rb.velocity.x) > Mathf.Epsilon);
            sounds[0].enabled = player_is_running && IsGrounded();
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
            if (!IsGrounded() && jump_keydown == 0 && !pressing_jump) {
                jump_keydown = 1;
                can_jump = false;
            } else if(IsGrounded() && jump_keydown != 0 && !pressing_jump){
                Reset_Jump();
            } else if ((Input.GetKey("l") || jump_buffer_counter > 0) && (jump_timer <= jump_time) && can_jump && !(OnWall() && move_Dir != 0 && !IsGrounded())){
                Vector2 playerVal = new Vector2(rb.velocity.x, jump_hight);
                rb.velocity = playerVal;
                jump_timer += Time.deltaTime;      
            } else if ((Input.GetKey("l") || jump_buffer_counter > 0) && (jump_timer <= jump_time) && can_double_jump && !(OnWall() && move_Dir != 0 && !IsGrounded())){
                Vector2 playerVal = new Vector2(rb.velocity.x, jump_hight);
                rb.velocity = playerVal;
                jump_timer += Time.deltaTime;
                jump_wings.SetActive(true);
            } else if (IsGrounded()){
                jump_wings.SetActive(false);
            }
        } else {
            animator.SetBool("jump",false);
            jump_effect_animator.SetBool("jump",false);
            jump_wings.SetActive(false);
        }

        if(Input.GetKeyDown("l")){
            jump_keydown += 1;
            pressing_jump = true;
            jump_buffer_counter = jump_buffer_time;
            if (can_jump || can_double_jump) {
                // Character animation to jump
                animator.SetBool("jump",true);

                // Display jump effect only on double jump
                if (jump_keydown==2) {
                    sounds[2].Play();
                    DoubleJumpEffect();
                } else {
                    sounds[1].Play();
                    jump_effect.SetActive(false);
                }
            } 
        }else if(jump_buffer_counter > -1){
            jump_buffer_counter -= Time.deltaTime;
        }
        
        if(Input.GetKeyUp("l")){
            Release_Jump_Or_Hurt_Fix();
        }
    }

    void DoubleJumpEffect() {
        jump_effect.SetActive(true);
        jump_effect.transform.position = jumpEffectTransform.position;
        jump_effect.transform.localRotation = jumpEffectTransform.localRotation;
        jump_effect_animator.SetBool("jump",true);
        StartCoroutine(WaitForJumpEffect());
    }

    IEnumerator WaitForJumpEffect() {
        yield return new WaitForSeconds(0.333f);
        jump_effect.SetActive(false);
    }
    
    public void Clear_Jump(){
        jump_timer = jump_time + 1;
    }

    public void Release_Jump_Or_Hurt_Fix() {

        pressing_jump = false;
        animator.SetBool("jump", false);
        jump_wings.SetActive(false);
        
        if (jump_keydown == 1) {
            can_jump = false;
            jump_timer = jump_time * (1 - 0.8f);
        } else if (jump_keydown >= 2) {
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
        can_double_jump = stateList.GetSkillLearned("Double Jump");
        can_jump = true;
        jump_effect.SetActive(false);
    }
    public void ResetDoubleJump() {
        jump_keydown = 1;
        jump_timer = 0;
        can_double_jump = stateList.GetSkillLearned("Double Jump");
        can_jump = false;
    }
    public void Set_Extra_Jump(){
            jump_keydown = 2;
            can_jump = stateList.GetSkillLearned("Attack Extra Jump");
            can_double_jump = false;
            jump_timer =jump_time * (1 - 0.8f);
            if(Input.GetKey("l")){
                jump_effect_animator.SetBool("jump",false);
                // jump_effect_animator.SetBool("jump",true);
            }
    }

    void Checked_Grounded(){
        bool isGrounded = IsGrounded();

        if (!lastGrounded && isGrounded) {
            sounds[3].Play();
        }
        animator.SetBool("is_ground", isGrounded);
        if (isGrounded && (Time.time > lastGroundSave + groundSaveInterval)) {
            if (Physics2D.OverlapCircle(backGroundDetectionPoint.position, 0.05f, groundLayer)) {
                respawn_from_water_point = backGroundDetectionPoint.position + new Vector3(0, (transform.position.y - backGroundDetectionPoint.position.y), 0f);
            } else if (Physics2D.OverlapCircle(frontGroundDetectionPoint.position, 0.05f, groundLayer)) {
                respawn_from_water_point = frontGroundDetectionPoint.position + new Vector3(0, (transform.position.y - frontGroundDetectionPoint.position.y), 0f);
            } else {
                respawn_from_water_point = transform.position;
            }
            lastGroundSave = Time.time;
        }
        lastGrounded = isGrounded;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.05f, groundLayer);
    }

    private bool IsOnJumpPad()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, jumppadLayer);
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
        sounds[4].enabled = isWallSliding;
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
            jump_effect_animator.SetBool("jump",true);
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
        if(Input.GetKeyDown("k") && stateList.GetSkillLearned("Bat Transmit")){

            if (bat.activeSelf) {
                transform.position = bat.transform.position;
                rb.velocity = new Vector2(rb.velocity.x,0);
                bat.SetActive(false);
            } else {
                bat.SetActive(true);
                Bat batComponent = bat.GetComponent<Bat>();
                batComponent.Flip();
                batComponent.lastGenerate = Time.time;
                bat.transform.position = shot_pos.position;
                
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
        }
        
    }

    void Down_Smash(){
        if (Input.GetKeyDown("s") && stateList.GetSkillLearned("Down Smash"))
        {
            float since_last_s = Time.time - last_s_timer;
            if(since_last_s <= double_s_range && can_down_smash){
                if(player_mana.cost_mana(1)){
                    // do the skill
                    player_health.GetComponent<Player_Hurt>().Set_Super(true);
                    can_down_smash = false;
                    acceptInput = false;
                    Debug.Log("down_smashing");
                    if(IsGrounded()){
                        StartCoroutine(Jump_Before_Down_Smash());
                    }else{
                        StartCoroutine(Do_Down_Smash()); 
                    }
                }              
            }
            last_s_timer = Time.time;
        }else if(down_smashing && IsGrounded()){
            
            StartCoroutine(Do_Down_Smash_Hitbox());
            
        }else if(down_smashing && (IsOnJumpPad() || IsInWater())){
            Down_Smash_Effect.SetActive(false);
            down_smashing = false;
            Reset_Down_Smash();
            StartCoroutine(Reload_Down_Smash());
        }
    }
    private bool IsInWater() {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, waterLayer);
    }

    IEnumerator Do_Down_Smash_Hitbox(){
        sounds[6].Play();
        Down_Smash_Effect.SetActive(false);
        down_smashing = false;

        Down_Smash_HitBox.SetActive(true);
        down_smash_animator.SetBool("hit_ground",true);
        yield return new WaitForSeconds(0.5f);

        Reset_Down_Smash();
        
        Down_Smash_HitBox.SetActive(false);
        down_smash_animator.SetBool("hit_ground",false);
        StartCoroutine(Reload_Down_Smash());
    }

    IEnumerator Jump_Before_Down_Smash(){
        rb.velocity = new Vector2(0, jump_hight *1.5f);
        yield return new WaitForSeconds(down_smash_delay);
        StartCoroutine(Do_Down_Smash());
    }


    IEnumerator Do_Down_Smash(){
        sounds[5].Play();
        Set_Save_Animator(true);
        float count =0;
        down_smashing = true;
        while(count < 0.3f){
            count += Time.deltaTime;
            rb.velocity =new Vector2(0,0);
            yield return null;
        }
        
        Sword_Effect.SetActive(true);
        Down_Smash_Effect.SetActive(true);

        max_drop_velocity = max_drop_velocity * 1.6f;
        Vector2 playerVal = new Vector2(rb.velocity.x, max_drop_velocity);
        rb.velocity = playerVal;
        max_drop_velocity = max_drop_velocity/1.6f;
    }

    private void Reset_Down_Smash(){
        acceptInput = true;
        Set_Save_Animator(false);
        Sword_Effect.SetActive(false);
    }

    IEnumerator Reload_Down_Smash(){
        yield return new WaitForSeconds(1);
        can_down_smash = true;
        player_health.GetComponent<Player_Hurt>().Set_Super(false);
    }

    public void Respawn_From_Water() {
        transform.position = respawn_from_water_point;
    }

    public void Set_Save_Animator(bool state){
        animator.SetBool("saving",state);
    }
}
