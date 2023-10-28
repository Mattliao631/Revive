using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionEnemy : Enemy
{
    protected enum STATES{
        Backing = 0,
        Roaming = 1,
        Fighting = 2,
        Rushing = 3,
        Jumping = 4,
        Striking = 5
    }
    [Header("State")]
    [SerializeField] protected STATES _state = STATES.Roaming;
    public bool seePlayer = false;
    public bool outOfRegion = false;
    
    [Header("Region Management")]
    [SerializeField] protected RegionManager regionManager;
    public Transform respawnPoint;
    public float respawnTime = 5f;
    public GameObject soul;


    [Header("Roaming Setting")]
    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected float minDecisionTime = 3f;
    [SerializeField] protected float maxDecisionTime = 8f;


    [Header("Chasing Setting")]
    [SerializeField] protected float chaseSpeedFactor = 2f;
    protected float chaseVelocity = 0f;


    


    private int dir = 0;
    private float lastTime = 0f;


    // states of enemy ai


    
    protected virtual void _toStateBacking() {
        _state = STATES.Backing;
    }
    protected virtual void _toStateRoaming() {
        _state = STATES.Roaming;
    }
    protected virtual void _toStateFighting() {
        _state = STATES.Fighting;
    }

    protected virtual void _exitStateFighting() {
        _state = STATES.Roaming;
    }
    

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        
    }

    protected virtual void _updateState() {
        if (outOfRegion) {
            _state = STATES.Backing;
        } else if (seePlayer) {
            _state = STATES.Fighting;
        } else {
            _state = STATES.Roaming;
        }
    }

    void FixedUpdate() {
        // Debug.Log(_state);
        _updateState();
        _act();
    }
    
    protected virtual void _act() {
        switch(_state){
            case STATES.Backing:
                _back();
                break;
            case STATES.Fighting:
                _chase();
                break;
            case STATES.Roaming:
                _roam();
                break;
            default:
                Debug.Log("Still need to be fix0");
                break;
        }
    }

    public override void respawn() {
        base.respawn();
        gameObject.transform.position = respawnPoint.position;
        outOfRegion = false;
    }
    protected override void die() {
        base.die();
        regionManager.respawnMonster(this);
    }
    
    protected virtual void _back() {
        if (!outOfRegion) {
            return;
        }
        regionManager.recallMonster(this);
    }
    protected virtual void _roam() {
        float decisionTime = Random.Range(minDecisionTime, maxDecisionTime);
        if (Time.time > lastTime + decisionTime) {
            dir = Random.Range(-1,2);
            lastTime = Time.time;
        }
        
        rb2d.velocity = new Vector3(dir * moveSpeed, 0, 0);
    }

    protected virtual void _chase() {
        float decisionTime = 0.5f;
        if (Time.time > lastTime + decisionTime) {
            chaseVelocity = moveSpeed * chaseSpeedFactor * (player.transform.position.x > transform.position.x ? 1 : -1);
            lastTime = Time.time;
        }

        
        rb2d.velocity = new Vector3(chaseVelocity, 0, 0);
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject == regionManager.gameObject) {
            outOfRegion = true;
        }
    }
}
