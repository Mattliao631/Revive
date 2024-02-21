using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Random = UnityEngine.Random;

public class Butterfly : RegionEnemy
{
    [Header("Navigation")]
    [SerializeField] private AIDestinationSetter aIDestinationSetter;
    [SerializeField] private Transform[] roamingPoints;
    private float detectDist = 6;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        soulColor = Color.yellow;

        repelVector = new Vector3(5,5,0);
        c1 = 1000;
        c2 = 20;

        exp = 6;

        minDecisionTime = 3f;
        maxDecisionTime = 3.1f;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if (Vector3.Distance(respawnPoint.position, transform.position) < 0.3) {
            outOfRegion = false;
        }

    }
    new void FixedUpdate() {
        base.FixedUpdate();
        seePlayer = Vector3.Distance(player.transform.position, transform.position) < detectDist;
    }

    protected override void _act() {
        switch(_state) {
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
                Debug.Log("Still need to be fix butterfly.");
                break;
        }
    }
    protected override void _updateState() {
        if (outOfRegion) {
            _state = STATES.Backing;
        } else if (seePlayer) {
            _state = STATES.Fighting;
        } else {
            _state = STATES.Roaming;
        }
    }

    protected override void _roam() {
        float decisionTime = Random.Range(minDecisionTime, maxDecisionTime);
        if (Time.time > lastTime + decisionTime) {
            lastTime = Time.time;
            int index = Random.Range(0, roamingPoints.Length);
            if(aIDestinationSetter.target == roamingPoints[index]){
                switch(index){
                    case 0:
                        index = 1;
                        break;
                    case 1:
                        index = 2;
                        break;
                    case 2:
                        index = 0;
                        break;
                }
            }
            
            aIDestinationSetter.target = roamingPoints[index];
        } 
    }

    protected override void _chase() {
        if (aIDestinationSetter.target == player.transform) return;
        aIDestinationSetter.target = player.transform;
    }
    protected override void _back() {
        if (!outOfRegion) {
            return;
        }
        aIDestinationSetter.target = respawnPoint;
    }

    protected override void rotate() {
        float xDist = aIDestinationSetter.target.position.x - transform.position.x;
        if (xDist > 0f) {
            transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        } else {
            transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }
}
