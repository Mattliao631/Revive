using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraHealth : OneTimeUsage
{
    private PlayerStateList stateList;
    void Awake() {
        stateList = GameManager.instance.stateList;
    }
    public override void Play() {
        stateList.SetMaxHealth(stateList.GetMaxHealth() + 1);
        stateList.SetCurrentHealth(stateList.GetMaxHealth());
        gameObject.SetActive(false);
    }
}
