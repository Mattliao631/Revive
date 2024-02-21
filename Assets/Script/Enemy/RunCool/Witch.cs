using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witch : MonoBehaviour
{
    private enum STATES{
        Idle = 0,
        Prepare = 1,
        Charge = 2
    }
    private STATES state = STATES.Idle;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform followPoint;
    [SerializeField] private float followSpeed;
    [SerializeField] private float chargeCooldown;
    [SerializeField] private float chargeVelocity;
    [SerializeField] private float chargeDuration;
    [SerializeField] private float prepareDuration;
    [SerializeField] private float backYOffset;
    [SerializeField] private int damage;
    [SerializeField] private BoxCollider2D player_hurt;
    private float lastChargeStart;
    private Vector3 chargeDirection;
    private Animator animator;
    [SerializeField] private AudioSource[] sounds;

    public void Reset() {
        state = STATES.Idle;

        animator.SetBool("idle", true);
        animator.SetBool("prepare", false);
        animator.SetBool("smashing", false);
        
        
        transform.position = followPoint.position;
    }

    private void UpdateState() {
        switch(state) {
            case STATES.Idle:
                if (Time.time - lastChargeStart >= chargeCooldown) {
                    lastChargeStart = Time.time;
                    chargeDirection = Vector3.Normalize(playerTransform.position - transform.position);
                    state = STATES.Prepare;

                    animator.SetBool("idle",false);
                    animator.SetBool("prepare",true);
                }
                break;
            case STATES.Prepare:
                if (Time.time - lastChargeStart >= prepareDuration) {
                    lastChargeStart = Time.time;
                    // chargeDirection = Vector3.Normalize(playerTransform.position - transform.position);
                    state = STATES.Charge;

                    animator.SetBool("prepare",false);
                    animator.SetBool("smashing",true);
                    sounds[0].Play();
                }
                break;
            case STATES.Charge:
                if (Time.time - lastChargeStart >= chargeDuration) {
                    transform.position = followPoint.position + new Vector3(0, backYOffset, 0);
                    state = STATES.Idle;

                    animator.SetBool("smashing",false);
                    animator.SetBool("idle",true);
                }
                break;
            default:
                break;
        }
    }
    private void Act() {
        switch(state) {
            case STATES.Idle:
                transform.position = Vector3.Lerp(transform.position, followPoint.position, Time.deltaTime * followSpeed);
                break;
            case STATES.Prepare:
                transform.position = Vector3.Lerp(transform.position, followPoint.position, Time.deltaTime * followSpeed);
                break;
            case STATES.Charge:
                transform.position += chargeDirection * chargeVelocity * Time.deltaTime;
                break;
            default:
                break;
        }
    }

    // Start is called before the first frame update
    void Awake() {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
        Act();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other == player_hurt) {
            Player_Hurt h = other.gameObject.GetComponent<Player_Hurt>();
            if (h != null) {
                h.Take_Damage(damage);
            }
        }
    }
}
