using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRabbit : RegionEnemy
{
    
    [Header("Strike Skill")]
    [SerializeField] protected float strikeCoolDown = 3f;
    
    [SerializeField] protected float strikeDelayTime = 0.3f;
    [SerializeField] protected float strikeDuration = 0.1f;
    [SerializeField] protected Collider2D strikeHitbox;

    
    [Header("Cast Control")]
    [SerializeField] protected float minCastInterval = 2f;

    private float lastSkillCast = 0f;

    protected bool canStrike = true;

    public bool findStrike = false;
    new void Start()
    {
        base.Start();
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

    protected virtual void _toStateStriking() {
        _state = STATES.Striking;
    }
    protected virtual void _exitStateStriking() {
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
            case STATES.Striking:
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
        if (canCast && canStrike && findStrike) {
            _strike();
            return;
        }

        _chase();
    }
    public override void respawn() {
        base.respawn();
        canStrike = true;
        strikeHitbox.enabled = false;
        lastSkillCast = Time.time;
        findStrike = false;
        _toStateRoaming();
    }

    protected virtual void _strike() {
        _toStateStriking();
        lastSkillCast = Time.time;
        canStrike = false;

        Coroutine waitingForRushCooldown = StartCoroutine(waitForStrikeCooldown());

        flashColor(strikeDelayTime, Color.green);
        Coroutine strike = StartCoroutine(strikeAction());
    }

    private IEnumerator strikeAction() {
        yield return new WaitForSeconds(strikeDelayTime);
        strikeHitbox.enabled = true;
        
        yield return disableStrikeHitbox();
        
        _exitStateStriking();
    }
    private IEnumerator waitForStrikeCooldown() {
        yield return new WaitForSeconds(strikeCoolDown);
        canStrike = true;
    }
    private IEnumerator disableStrikeHitbox() {
        yield return new WaitForSeconds(strikeDuration);
        strikeHitbox.enabled = false;
    }
}
