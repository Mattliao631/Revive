using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{

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
    
    

    private SpriteRenderer sr;
    private Color origin_color;

    // Start is called before the first frame update
    public void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        origin_color = sr.color;
        health = initialHelth;
        damage = initialDamage;
    }

    // Update is called once per frame
    public void Update()
    {
        detectDie();
        rotate();
    }

    private void rotate() {
        if (rb2d.velocity.x > 0.3f) {
            transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        } else if (rb2d.velocity.x < -0.3f) {
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
        gameObject.SetActive(false);
    }

}
