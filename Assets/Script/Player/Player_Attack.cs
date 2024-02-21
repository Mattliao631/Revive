using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    private PlayerStateList stateList;
    private int initialDamage = 5;
    private int extraDamage = 0;
    [SerializeField] private Vector3 originalAttackRangeScale = new Vector3(2f, 1f, 1f);
    [SerializeField] private Vector3 extendAttackRangeScale = new Vector3(10f, 2.3f, 1f);
    private int hitCount = 0;
    private float lastHit = 0f;
    [SerializeField] private float hitCountResetInterval = 5f;
    [SerializeField] private int bigAttackHitCount = 3;
    [SerializeField] private int polishedDamage = 2;

    [SerializeField] private GameObject player;
    private Animator playerAnimator;
    private Animator attackEffectAnimator;
    private SpriteRenderer sr;

    [Header("Attack Hit")]
    [SerializeField] private GameObject attack_hit;
    [SerializeField] private GameObject attack_hit_2;
    public Vector3 attack_hit_pos_offset;

    [Header("Attack Trigger")]
    public float collider_start_time;
    public float collider_remain_time;
    private PolygonCollider2D pol_Col2D;

    [Header("Attack Control")]
    public bool can_attack;

    public int face;
    private int attack_face;

    [SerializeField] private GameObject attack_light;
    [SerializeField] private AudioSource[] sounds;

    void Awake() {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        stateList = GameManager.instance.stateList;

        playerAnimator = player.GetComponent<Animator>();
        attackEffectAnimator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        pol_Col2D = GetComponent<PolygonCollider2D>();
        can_attack = true;
        
    }
    public int GetDamage() {
        if (stateList.GetAbilityLearned("Polished")) {
            extraDamage = polishedDamage;
        }
        return initialDamage + extraDamage;
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
        face = player.GetComponent<PlayerController>().face_Dir;
        if (Time.time >= lastHit + hitCountResetInterval) {
            hitCount = 0;
        }
    }

    void Attack(){
        if(Input.GetKeyDown("j") && can_attack && player.GetComponent<PlayerController>().GetAcceptInput() && !player.GetComponent<PlayerController>().isWallSliding){
            can_attack = false;
            if (hitCount >= bigAttackHitCount && stateList.GetAbilityLearned("Blade Extend")) {
                hitCount = 0;
                transform.localScale = extendAttackRangeScale;
                sr.color = Color.black;
                attack_hit.GetComponent<SpriteRenderer>().color = Color.black;
                attack_hit_2.GetComponent<SpriteRenderer>().color = Color.black;
            } else {
                transform.localScale = originalAttackRangeScale;
                sr.color = Color.white;
                attack_hit.GetComponent<SpriteRenderer>().color =Color.white;
                attack_hit_2.GetComponent<SpriteRenderer>().color = Color.white;

            }
            StartCoroutine(Start_Attack());
        }
    }

    // delay for the right attack moment
    IEnumerator Start_Attack(){
        yield return new WaitForSeconds(collider_start_time);
        // transform.position = attackTransform.position;
        // transform.localRotation = player.transform.localRotation;
        sounds[0].Play();
        pol_Col2D.enabled = true;
        sr.enabled = true;
        attackEffectAnimator.SetBool("attacking", true);
        playerAnimator.SetTrigger("attack");
        playerAnimator.SetBool("attacking",true);
        attack_face = face;
        // attack_light.SetActive(true);
        

        if(Input.GetKey("l")){
            player.GetComponent<PlayerController>().Clear_Jump();
        }
        StartCoroutine(Disable_Hitbox());
        StartCoroutine(Disable_Hit_Effect());
    }

    IEnumerator Disable_Hitbox(){
        float count = 0;
        while(count < collider_remain_time){
            count += Time.deltaTime;
            if(attack_face != face){
                // Debug.Log("have break");
                break;
            }
            yield return null;
        }
        sr.enabled = false;
        pol_Col2D.enabled = false;
        playerAnimator.SetBool("attacking",false);
        attackEffectAnimator.SetBool("attacking", false);
        can_attack = true;
        // attack_light.SetActive(false);
    }

    IEnumerator Disable_Hit_Effect(){
        float count =0;
        while(count < collider_remain_time){
            count += Time.deltaTime;
            yield return null;
        }

        attack_hit.SetActive(false);
        attack_hit.GetComponent<Animator>().SetBool("attack_hit",false);
        // attack_light.SetActive(false);

        attack_hit_2.GetComponent<Animator>().SetBool("attack_hit",false);

    }

    // is trigger will check by itself 
    void Attack_Hit(){
        int dir = 1;
        if(player.transform.localRotation.y == -1){
            dir = -1;
        }

        attack_hit.transform.position = player.GetComponent<Transform>().position + dir * attack_hit_pos_offset;
        attack_hit.transform.localRotation = player.transform.localRotation;
        attack_hit.SetActive(true);
        attack_hit.GetComponent<Animator>().SetBool("attack_hit",true);
        // attack_hit_2.GetComponent<Animator>().SetBool("attack_hit",true);
    }
    void Attack_Hit(Transform hit) {
        // int dir = 1;
        // if(player.transform.localRotation.y == -1){
        //     dir = -1;
        // }

        attack_hit.transform.position = hit.position;
        attack_hit.transform.localRotation = player.transform.localRotation;
        attack_hit.SetActive(true);
        attack_hit.GetComponent<Animator>().SetBool("attack_hit",true);
        // attack_hit_2.GetComponent<Animator>().SetBool("attack_hit",true);
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Enemy")){
            GameObject otherGameObj = other.gameObject;
            GameObject otherParent = otherGameObj.transform.parent.gameObject;
            otherParent.GetComponent<Enemy>().Take_Damage(GetDamage());
            // other.gameObject.transform.parent.gameObject.GetComponent<Enemy>().Take_Damage(damage);

            if(stateList.GetSkillLearned("Attack Extra Jump")){
               player.GetComponent<PlayerController>().Set_Extra_Jump(); 
            }
            // Attack_Hit();
            sounds[1].Play();
            Attack_Hit(otherParent.transform);
            hitCount += 1;
            lastHit = Time.time;
            // CameraManager.instance.shake(0);

        }else if(other.CompareTag("Lamp")){
            if(stateList.GetSkillLearned("Attack Extra Jump")){
               player.GetComponent<PlayerController>().Set_Extra_Jump();
               // Attack_Hit();
               sounds[2].Play();
               Attack_Hit(other.gameObject.transform); 
            }
        }else if(other.CompareTag("BreakableWall")){
            other.GetComponent<BreakableWall>().Take_Damage(GetDamage());
            // Attack_Hit();
            sounds[1].Play();
            Attack_Hit(other.gameObject.transform);
        }else{
            // Attack_Hit();
        }
    }
}
