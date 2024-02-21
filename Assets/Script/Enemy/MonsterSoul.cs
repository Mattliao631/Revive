using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSoul : CollectableObject
{
    public int index;
    public int exp;
    private float soulUtilizationFactor = 1.5f;
    private float startTime;
    public float existTime = 30f;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    void OnEnable() {
        startTime = Time.time;
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime >= existTime) {
            SoulManager.instance.SoulReturn(index);
        }
    }
    protected override void TouchedPlayer() {
        float factor = 1f;
        if (stateList.GetAbilityLearned("Soul Utilization")) {
            factor *= soulUtilizationFactor;
        }
        stateList.SetSoul(stateList.GetSoul() + (int)(factor * exp));
        SoulManager.instance.SoulReturn(index);
    }
}
