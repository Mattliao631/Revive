using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    private PlayerStateList stateList;
    private float knockBackFactor = 1.2f;
    [Header("Enemy Status")]
    [SerializeField] private int initialHelth;
    [SerializeField] private int initialDamage;
    

    [HideInInspector] public int health;
    [HideInInspector] public int damage;


    [Header("Enemy Controller")]
    [SerializeField] protected Rigidbody2D rb2d;
    
    [SerializeField] protected GameObject player;


    [Header("Take Damage")]
    public float flash_time;
    [SerializeField] protected Vector3 repelVector;
    [SerializeField] protected float c1;
    [SerializeField] protected float c2;

    private SpriteRenderer sr;
    private Color origin_color;
    private StateConditioner conditioner;

    // Start is called before the first frame update
    public void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        origin_color = sr.color;
        health = initialHelth;
        damage = initialDamage;
        stateList = GameManager.instance.stateList;

        conditioner = GetComponent<StateConditioner>();
    }

    protected void OnEnable() {
        health = initialHelth;
        damage = initialDamage;
    }
    // Update is called once per frame
    public void Update()
    {
        detectDie();
        rotate();
    }

    protected virtual void rotate() {
        if (rb2d.velocity.x > 0.5f) {
            transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        } else if (rb2d.velocity.x < -0.5f) {
            transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }

    public virtual void respawn() {
        health = initialHelth;
        damage = initialDamage;
        gameObject.SetActive(true);
    }

    public void Take_Damage(int damage){
        health -= damage;
        flashColor(flash_time, Color.red);
        Repeled();

    }
    private void Repeled() {
        float factor = 1f;
        if (stateList.GetAbilityLearned("Knock Back")) {
            factor *= knockBackFactor;
        }
        Vector3 playerToSelf = (transform.position - player.transform.position);
        float dir = Mathf.Sign(playerToSelf.x);
        Vector3 totalRepel = factor * (c1 * new Vector3(dir, 0, 0) + c2 * new Vector3(dir * repelVector.x, repelVector.y, 0f));
        rb2d.AddForce(totalRepel);
    }

    protected void flashColor(float time, Color color){
        sr.color = color;
        Invoke("Reset_Color",time);
    }

    private void Reset_Color(){
        sr.color = origin_color;
    }

    private void detectDie() {
        if(health <= 0){
            die();
        }
    }
    
    protected virtual void die() {
        if (conditioner != null) {
            conditioner.StateConditionCall();
        }
        gameObject.SetActive(false);
    }

}
