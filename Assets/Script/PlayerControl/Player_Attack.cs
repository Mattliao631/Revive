using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    public int damage;

    [SerializeField] private GameObject player;
    private Animator playerAnimator;
    private Animator attackEffectAnimator;
    private SpriteRenderer sr;

    [Header("Attack trigger")]
    private PolygonCollider2D pol_Col2D;
    public float collider_start_time;
    public float collider_remain_time;
    [Header("Attack control")]
    public bool can_attack;
    [SerializeField] private Transform attackTransform;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = player.GetComponent<Animator>();
        attackEffectAnimator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        pol_Col2D = GetComponent<PolygonCollider2D>();
        can_attack = true;
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }

    void Attack(){
        if(Input.GetKeyDown("j") && can_attack && player.GetComponent<PlayerController>().Accept_Input && !player.GetComponent<PlayerController>().isWallSliding){
            can_attack = false;
            StartCoroutine(Start_Attack());
        }
    }

    // delay for the right attack moment
    IEnumerator Start_Attack(){
        yield return new WaitForSeconds(collider_start_time);
        transform.position = attackTransform.position;
        transform.localRotation = player.transform.localRotation;
        pol_Col2D.enabled = true;
        sr.enabled = true;
        attackEffectAnimator.SetBool("attacking", true);
        playerAnimator.SetTrigger("attack");
        playerAnimator.SetBool("attacking",true);
        player.GetComponent<PlayerController>().Accept_Input = false;
        StartCoroutine(Disable_Hitbox());
    }

    IEnumerator Disable_Hitbox(){
        yield return new WaitForSeconds(collider_remain_time);
        sr.enabled = false;
        pol_Col2D.enabled = false;
        playerAnimator.SetBool("attacking",false);
        attackEffectAnimator.SetBool("attacking", false);
        can_attack = true;
        player.GetComponent<PlayerController>().Accept_Input = true;
    }

    // is trigger will check by itself 
    
    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Enemy")){
            GameObject otherGameObj = other.gameObject;
            GameObject otherParent = otherGameObj.transform.parent.gameObject;
            otherParent.GetComponent<Enemy>().Take_Damage(damage);
            // other.gameObject.transform.parent.gameObject.GetComponent<Enemy>().Take_Damage(damage);
            player.GetComponent<PlayerController>().Set_Extra_Jump();
            CameraManager.instance.shake(0);
        }
    }
}
