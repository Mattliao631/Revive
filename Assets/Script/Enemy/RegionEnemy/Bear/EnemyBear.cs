using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBear : RegionEnemy
{
    [Header("Rush Skill")]
    [SerializeField] private float rushTime = 5f;
    // [SerializeField] private float jumpTime = 0.8f;

    [SerializeField] protected float rushCooldown = 5f;
    [SerializeField] protected float rushDelayTime = 0.3f;
    [SerializeField] protected float rushScale = 2f;

    [Header("Jump Skill")]
    [SerializeField] protected float jumpCoolDown = 5f;
    
    [SerializeField] protected float jumpDelayTime = 0.4f;

    [SerializeField] protected float jumpForce = 2f;
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float strikeDuration = 0.1f;
    [SerializeField] private Collider2D strikeHitbox;
    [SerializeField] private bool isGrounded = true;
    private static float cubeForce;
    
    [Header("Cast Control")]
    [SerializeField] protected float minCastInterval = 2f;

    private float lastSkillCast = 0f;

    protected bool canRush = true;
    protected bool canJump = true;

    public bool findRush = false;
    public bool findJump = false;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        cubeForce = jumpForce * jumpForce * jumpForce;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    void FixedUpdate() {
        _updateState();
        _act();
    }

    protected virtual void _toStateRushing() {
        if (_state == STATES.Fighting) {
            _state = STATES.Rushing;
        } else {
            Debug.Log("Still need to fix3");
        }
    }

    protected virtual void _toStateJumping() {
        if (_state == STATES.Fighting) {
            _state = STATES.Jumping;
        } else {
            Debug.Log("Still need to fix4");
        }
    }

    protected virtual void _exitStateRushing() {
        if (seePlayer) {
            _toStateFighting();
        } else {
            _toStateRoaming();
        }
    }

    protected virtual void _exitStateJumping() {
        if (seePlayer) {
            _toStateFighting();
        } else {
            _toStateRoaming();
        }
    }
    protected override void _updateState() {
        switch(_state) {
            case STATES.Roaming:
            case STATES.Fighting:
                if (outOfRegion) {
                    _state = STATES.Backing;
                } else if (seePlayer) {
                    _state = STATES.Fighting;
                } else {
                    _state = STATES.Roaming;
                }
                break;
            case STATES.Backing:
                if (seePlayer) {
                    _state = STATES.Fighting;
                } else {
                    _state = STATES.Roaming;
                }
                break;
            case STATES.Rushing:
            case STATES.Jumping:
                break;
            default:
                break;
        }
    }

    protected override void _act() {
        switch(_state) {
            case STATES.Backing:
                _back();
                break;
            case STATES.Fighting:
                _fight();
                break;
            case STATES.Roaming:
                _roam();
                break;
            default:
                break;
        }
    }

    protected virtual void _fight() {
        bool canCast = Time.time - lastSkillCast > minCastInterval;
        int available = 0;
        if (canRush && canCast && findRush) {
            available++;
        }
        if (canJump && canCast && findJump) {
            available+=2;
        }
        
        if (available == 0) {
            _chase();
        } else {
            switch(available) {
                case 1:
                    _rush();
                    break;
                case 2:
                    _jump();
                    break;
                case 3:
                    int r = Random.Range(0, 2);
                    if (r == 0) {
                        _rush();
                    } else {
                        _jump();
                    }
                    break;
                default:
                    break;
            }
        }
    }
    public override void respawn() {
        base.respawn();
        canJump = true;
        canRush = true;
        isGrounded = true;
        lastSkillCast = Time.time;
        _toStateRoaming();
    }

    protected virtual void _rush() {

        _toStateRushing();
        lastSkillCast = Time.time;
        canRush = false;

        Coroutine waitingForRushCooldown = StartCoroutine(waitForRushCooldown());
        
        flashColor(rushDelayTime, Color.yellow);
        Coroutine rush = StartCoroutine(rushAction());
        
        
    }
    private IEnumerator rushAction() {

        yield return new WaitForSeconds(rushDelayTime);

        float startTime = Time.time;
        float dir = Mathf.Sign(player.transform.position.x - transform.position.x);
        while (Time.time - startTime < rushTime) {
            rb2d.velocity = new Vector3(dir, 0, 0) * rushScale;
            yield return null;
        }
        rb2d.velocity = Vector3.zero;
        yield return new WaitForSeconds(1);
        _exitStateRushing();
    }
    
    protected virtual void _jump() {

        _toStateJumping();
        lastSkillCast = Time.time;
        canJump = false;

        Coroutine waitingForJumpCooldown = StartCoroutine(waitForJumpCooldown());

        flashColor(jumpDelayTime, Color.blue);
        Coroutine jump = StartCoroutine(jumpAction());
        
    }

    private IEnumerator jumpAction() {

        yield return new WaitForSeconds(jumpDelayTime);

        Vector3 dirVec = player.transform.position - transform.position;
        float extraMomentum = Mathf.Sqrt(Mathf.Abs(dirVec.x)) * 2f;
        rb2d.AddForce(dirVec * extraMomentum * jumpForce + transform.up * cubeForce);

        isGrounded = false;

        while (!isGrounded) {
            yield return new WaitForSeconds(0.1f);
            groundCheck();
        }

        strikeHitbox.enabled = true;
        
        yield return disableStrikeHitbox();
        rb2d.velocity = Vector3.zero;
        yield return new WaitForSeconds(1);
        
        _exitStateJumping();
    }

    private IEnumerator waitForRushCooldown() {
        yield return new WaitForSeconds(rushCooldown);
        canRush = true;
    }
    private IEnumerator waitForJumpCooldown() {
        yield return new WaitForSeconds(jumpCoolDown);
        canJump = true;
    }

    private IEnumerator disableStrikeHitbox() {
        yield return new WaitForSeconds(strikeDuration);
        strikeHitbox.enabled = false;
    }

    private void groundCheck() {
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, 0.01f, groundLayer);
    }
}
