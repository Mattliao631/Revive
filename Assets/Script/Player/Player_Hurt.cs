using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Hurt : MonoBehaviour
{
    [SerializeField] float originalSuperTime = 1.5f;
    [SerializeField] float feerlessSuperTime = 3f;
    private PlayerStateList stateList;
    [SerializeField] GameObject player;
    private SpriteRenderer sr;
    
    private Color origin_color;
    private float flash_times = 6;

    public bool super;
    private float super_time = 1.5f;

    private Vector3 check_point;
    private PlayerController playerController;

    private Collider2D hurt_collider;
    private Collider2D lastCheckField;

    private bool isInCheckPoint = false;
    [SerializeField] private AudioSource[] sounds;

    // Start is called before the first frame update
    void Awake() {
        
    }
    void Start()
    {
        stateList = GameManager.instance.stateList;
        
        sr = player.GetComponent<SpriteRenderer>();
        hurt_collider = gameObject.GetComponent<BoxCollider2D>();
        playerController = player.GetComponent<PlayerController>();
        origin_color = sr.color;
        super = false;
        check_point = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Dead();
    }

    public void Take_Damage(int damage){
        sounds[0].Play();
        stateList.SetCurrentHealth(stateList.GetCurrentHealth() - damage);

        player.GetComponent<Rigidbody2D>().velocity = new Vector3(0,0,0);
        playerController.SetAcceptInput(false);
        Invoke("Reset_Input",0.25f);
        
        StartCoroutine(Flash_Color());
        hurt_collider.enabled =false;
        if (stateList.GetAbilityLearned("Feerless")) {
            super_time = feerlessSuperTime;
        } else {
            super_time = originalSuperTime;
        }
        StartCoroutine(Reset_Collider());
        // Invoke("Reset_Collider",super_time);
        
    }

    private void Touch_Bramble(){
        if(!super){
            Take_Damage(1);
        }
    }

    public void Touch_Water(){
        sounds[1].Play();
        if(stateList.GetCurrentHealth() > 1){
            playerController.Respawn_From_Water();
            Take_Damage(1);
        }else{
            stateList.SetCurrentHealth(stateList.GetCurrentHealth() - 1);
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

    // public void Touch_HealingBox(GameObject heal){

    //     heal.SetActive(false);
    //     if(stateList.GetCurrentHealth() < stateList.GetMaxHealth()){
    //         stateList.SetCurrentHealth(stateList.GetCurrentHealth() + 1);
    //     }
    // }

    // public void Touch_ManaBox(GameObject mana){

    //     mana.SetActive(false);
    //     if(stateList.GetCurrentMana() < stateList.GetMaxMana()){
    //         stateList.SetCurrentMana(stateList.GetCurrentMana() + 1);
    //     }
    // }

    // public void TouchExtraHealth(GameObject extrahealth){
    //     extrahealth.SetActive(false);
    //     stateList.SetExtraHealth(stateList.GetExtraHealth() + 1);
    // }

    // public void TouchExtraMana(GameObject extramana){
    //     extramana.SetActive(false);
    //     stateList.SetExtraMana(stateList.GetExtraMana() + 1);
    // }

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
        }
        // else if (other.CompareTag("CheckPoint")) {
        //     isInCheckPoint = true;
        //     lastCheckField = other;
        // }
        // else if(other.CompareTag("HealingBox")){
        //     GameObject otherGameObj = other.gameObject;
        //     Touch_HealingBox(otherGameObj);
        // }else if(other.CompareTag("ManaBox")){
        //     GameObject otherGameObj = other.gameObject;
        //     Touch_ManaBox(otherGameObj);
        // }else if(other.CompareTag("ExtraHealth")){
        //     GameObject otherGameObj = other.gameObject;
        //     TouchExtraHealth(otherGameObj);
        // }else if(other.CompareTag("ExtraMana")){
        //     GameObject otherGameObj = other.gameObject;
        //     TouchExtraMana(otherGameObj);
        // }
    }
    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("CheckPoint")) {
            isInCheckPoint = false;
        }
    }

    IEnumerator Reset_Collider(){
        yield return new WaitForSeconds(super_time);
        if(player.GetComponent<PlayerController>().can_down_smash){
            hurt_collider.enabled =true;
            super = false;
        }
    }

    public void Set_Super(bool i){
        hurt_collider.enabled = !i;
        super = i;
    }

    private void Reset_Input(){
        playerController.SetAcceptInput(true);
    }

    // private void Respawn(){
    //     // this method need to be re-implemented after the reload system created.
    //     stateList.currentHealth = stateList.maxHealth;
    //     stateList.currentMana = stateList.maxMana;
    //     player.GetComponent<Transform>().position = check_point;
    //     UI_Mana.updateMana();
    //     UI_Health.updateHealth();
    // }

    private void Dead(){
        if(stateList.GetCurrentHealth() <= 0 && !GameManager.instance.dead){
            // GetComponent<BoxCollider2D>().enabled = false;
            playerController.SetAcceptInput(false);
            Debug.Log($"Hurt's current health: {stateList.GetCurrentHealth()}!");
            // SceneManager.LoadScene("MainMenu");
            // Debug.Log("dead");
            GameManager.instance.LoadInScene();
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
