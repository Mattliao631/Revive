using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Hurt : MonoBehaviour
{

    public int health;
    public int max_health = 4; 

    [SerializeField] GameObject player;
    private SpriteRenderer sr;
    
    private Color origin_color;
    private float flash_times = 6;

    public bool super;
    private float super_time = 1;

    private Vector3 check_point;
    private PlayerController playerController;

    private Collider2D hurt_collider;
    private Collider2D lastCheckField;

    private bool isInCheckPoint = false;
    // Start is called before the first frame update
    void Start()
    {   
        sr = player.GetComponent<SpriteRenderer>();
        hurt_collider = gameObject.GetComponent<BoxCollider2D>();
        playerController = player.GetComponent<PlayerController>();
        origin_color = sr.color;
        health = max_health;
        super = false;
        check_point = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Dead();
        if(Input.GetKeyDown("r") && isInCheckPoint){
            check_point = lastCheckField.transform.position;
            health = max_health;
            Debug.Log("set checkpoint");
        }
    }

    public void Take_Damage(int damage){
        health -= damage;
        
        player.GetComponent<Rigidbody2D>().velocity = new Vector3(0,0,0);
        playerController.Accept_Input = false;
        Invoke("Reset_Input",0.2f);
        
        StartCoroutine(Flash_Color());
        hurt_collider.enabled =false;
        Invoke("Reset_Collider",super_time);
    }

    private void Touch_Bramble(){
        if(!super){
            Take_Damage(1);
        }
    }

    private void Touch_Water(){
        if(health > 1){
            playerController.Respawn_From_Water();
            Take_Damage(1);
        }else{
            health -= 1;
            Dead();
        }
    }

    public void Touch_Enemy(int damage){
        if(!super){
            Take_Damage(damage);
            playerController.Release_Jump_Or_Hurt_Fix();
            playerController.Reset_Jump_Buffer(); 
        }
        
    }

    void OnTriggerEnter2D(Collider2D other){
        
        if(other.CompareTag("Bramble")){    
            Touch_Bramble();
            super = true;
        }else if(other.CompareTag("Water")){
            Touch_Water();
        }else if(other.CompareTag("Enemy") || other.CompareTag("EnemySkill")){
            
            GameObject otherGameObj = other.gameObject;
            GameObject otherParent = otherGameObj.transform.parent.gameObject;
            Touch_Enemy(otherParent.GetComponent<Enemy>().damage);
            
            super = true;

        } else if (other.CompareTag("CheckPoint")) {
            isInCheckPoint = true;
            lastCheckField = other;
        }
    }
    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("CheckPoint")) {
            isInCheckPoint = false;
        }
    }



    private void Reset_Collider(){
        hurt_collider.enabled =true;
        super = false;
    }

    private void Reset_Input(){
        playerController.Accept_Input = true;
    }

    private void Respawn(){
        health = max_health;
        player.GetComponent<Transform>().position = check_point;
    }

    private void Dead(){
        if(health<=0){
            Respawn();
            Debug.Log("dead");
        }
    }

    public IEnumerator Flash_Color(){
        for(int i = 0;i<flash_times;i++){
           sr.color = Color.red;
           yield return new WaitForSeconds(super_time/(flash_times*2));
           Reset_Color();
           yield return new WaitForSeconds(super_time/(flash_times*2));
        }
        
    }

    public void Reset_Color(){
        sr.color = origin_color;
    }
}
