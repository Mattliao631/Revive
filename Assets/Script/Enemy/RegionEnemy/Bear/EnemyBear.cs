using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBear : RegionEnemy
{
    [Header("Rush Skill")]
    [SerializeField] private float rushTime = 5f;
    // [SerializeField] private float jumpTime = 0.8f;
    [SerializeField] private GameObject roar_effect;
    [SerializeField] private float rushCooldown = 8f;
    [SerializeField] protected float rushDelayTime = 0.3f;
    [SerializeField] protected float rushScale = 2f;

    [Header("Jump Skill")]
    [SerializeField] private float jumpCoolDown = 0f;
    
    [SerializeField] protected float jumpDelayTime = 0.4f;

    [SerializeField] protected float jumpForce = 2f;
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float strikeDuration = 0.3f;
    [SerializeField] private GameObject strikeHitbox;
    [SerializeField] private bool isGrounded = true;
    private static float cubeForce;
    
    [Header("Cast Control")]
    [SerializeField] protected float minCastInterval = 5f;

    private float lastSkillCast = 0f;

    protected bool canRush = true;
    protected bool canJump = true;

    public bool findRush = false;
    public bool findJump = false;
    [Header("Sound Effect")]
    [SerializeField] private AudioSource[] sounds;
    [SerializeField] private MusicBlock bearMusic;

    private Animator animator;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        cubeForce = jumpForce * jumpForce * jumpForce;
        animator = GetComponent<Animator>();
        soulColor = Color.gray;


    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
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

    private void PlayIdleSound() {
        sounds[0].enabled = UIManager.instance.IsGameRunning();
    }
    protected override void _updateState() {
        switch(_state) {
            case STATES.Roaming:
            case STATES.Fighting:
                if (outOfRegion) {
                    _state = STATES.Backing;
                } else if (seePlayer) {
                    PlayIdleSound();
                    _state = STATES.Fighting;
                } else {
                    PlayIdleSound();
                    _state = STATES.Roaming;
                }
                break;
            case STATES.Backing:
                if (seePlayer) {
                    _state = STATES.Fighting;
                } else {
                    _state = STATES.Roaming;
                }
                PlayIdleSound();
                break;
            case STATES.Rushing:
            case STATES.Jumping:
                sounds[0].enabled = false;
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
    new protected void OnEnable() {
        base.OnEnable();
        bearMusic.gameObject.SetActive(true);
    }
    public override void respawn() {
        base.respawn();
        canJump = true;
        canRush = true;
        isGrounded = true;
        lastSkillCast = Time.time;
        _toStateRoaming();
    }
    protected override void die() {
        base.die();
        bearMusic.GetComponent<AudioSource>().Pause();
        bearMusic.Cut();
        bearMusic.gameObject.SetActive(false);
    }

    protected virtual void _rush() {

        _toStateRushing();
        lastSkillCast = Time.time;
        canRush = false;

        Coroutine waitingForRushCooldown = StartCoroutine(waitForRushCooldown());
        
        // flashColor(rushDelayTime, Color.yellow);
        float dir = Mathf.Sign(player.transform.position.x - transform.position.x);
        rb2d.velocity = new Vector3(0,0,0);
        animator.SetBool("rush_ready",true);
        sounds[3].Play();
        roar_effect.SetActive(true);
        Coroutine rush = StartCoroutine(rushAction(dir));
    }
    private IEnumerator rushAction(float dir) {
        
        yield return new WaitForSeconds(rushDelayTime);

        float startTime = Time.time;
        
        roar_effect.SetActive(false);
        animator.SetBool("rushing",true);
        animator.SetBool("rush_ready",false);
        sounds[4].Play();

        while (Time.time - startTime < rushTime) {
            rb2d.velocity = new Vector3(dir, 0, 0) * rushScale;
            yield return null;
        }
        rb2d.velocity = Vector3.zero;
        animator.SetBool("rush_end",true);
        animator.SetBool("rushing",false);
        sounds[5].Play();
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("rush_end",false);
        
        yield return new WaitForSeconds(1f);
        _exitStateRushing();
    }
    
    protected virtual void _jump() {

        _toStateJumping();
        lastSkillCast = Time.time;
        canJump = false;

        Coroutine waitingForJumpCooldown = StartCoroutine(waitForJumpCooldown());
        
        Vector3 dirVec = player.transform.position - transform.position;
        float extraMomentum = Mathf.Sqrt(Mathf.Abs(dirVec.x)) * 2f;
        // flashColor(jumpDelayTime, Color.blue);
        rb2d.velocity = new Vector3(0,0,0);
        animator.SetBool("jump_ready",true);
        sounds[1].Play();
        Coroutine jump = StartCoroutine(jumpAction(dirVec, extraMomentum));
        
    }

    private IEnumerator jumpAction(Vector3 dirVec, float extraMomentum) {

        yield return new WaitForSeconds(jumpDelayTime);

        
        rb2d.AddForce(dirVec * extraMomentum * jumpForce + transform.up * cubeForce);

        animator.SetBool("jump_ready",false);
        animator.SetBool("jumping",true);
        float start_time = Time.time;
        isGrounded = false;

        while (!isGrounded && (start_time + 5f >Time.time)) {
            yield return new WaitForSeconds(0.1f);
            groundCheck();
        }

        strikeHitbox.SetActive(true);
        rb2d.velocity = Vector3.zero;

        animator.SetBool("jumping",false);
        animator.SetBool("jump_end",true);
        sounds[2].Play();
        yield return disableStrikeHitbox();

        yield return new WaitForSeconds(1f);
        animator.SetBool("jump_end",false);
        _exitStateJumping();
    }

    private IEnumerator waitForRushCooldown() {
        yield return new WaitForSeconds(rushCooldown);
        canRush = true;
        rushCooldown = Random.Range(0,9);
    }
    private IEnumerator waitForJumpCooldown() {
        yield return new WaitForSeconds(jumpCoolDown);
        canJump = true;
    }

    private IEnumerator disableStrikeHitbox() {
        yield return new WaitForSeconds(strikeDuration);
        strikeHitbox.SetActive(false);
    }

    private void groundCheck() {
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, 0.01f, groundLayer);
    }
}
