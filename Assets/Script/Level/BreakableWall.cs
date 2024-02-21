using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{

    [SerializeField] private int initialHelth;
    [SerializeField] private int health;

    [SerializeField] private Sprite origin_sprite;
    [SerializeField] private Sprite sprite1;
    [SerializeField] private Sprite sprite2;
    [SerializeField] private float respawn_time;
    

    public float flash_time;

    private SpriteRenderer sr;
    private BoxCollider2D boxCollider2D;
    private Color origin_color;
    [SerializeField] private AudioSource[] sounds;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        origin_color = sr.color;
        health = initialHelth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Take_Damage(int damage){
        health -= damage;
        // flashColor(flash_time, Color.gray);
        detectHealth();
    }

    protected void flashColor(float time, Color color){
        sr.color = color;
        Invoke("Reset_Color",time);
    }

    private void Reset_Color(){
        sr.color = origin_color;
    }

    private void detectHealth() {
        
        if(health <= 0){
            die();
        }else if(health <= 5){
            sounds[1].Play();
            sr.sprite = sprite2;
        }else {
            sounds[0].Play();
            sr.sprite = sprite1;
        }
    }
    
    private void die() {
        sounds[2].Play();
        sr.enabled = false;
        boxCollider2D.enabled =false;
        StartCoroutine(respawn());
    }

    private IEnumerator respawn(){

        yield return new WaitForSeconds(respawn_time);
        health = initialHelth;
        sr.sprite = origin_sprite;
        sr.enabled = true;
        boxCollider2D.enabled =true;
    }
}
