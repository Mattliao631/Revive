using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraMana : OneTimeUsage
{
    private PlayerStateList stateList;
    void Awake() {
        stateList = GameManager.instance.stateList;
    }
    public override void Play() {
        stateList.SetMaxMana(stateList.GetMaxMana() + 1);
        stateList.SetCurrentMana(stateList.GetMaxMana());
        gameObject.SetActive(false);
    }
}
